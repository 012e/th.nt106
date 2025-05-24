using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using THMang1.Models;

namespace THMang1.Server.Services
{
    public class GameHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameHub> _logger;
        private const string AdminGroup = "GameAdmins"; // Define a group name for admins

        public GameHub(IGameService gameService, ILogger<GameHub> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        // Method for an admin client to join the admin group
        public async Task RegisterAdminClient()
        {
            // In a real app, you'd have authentication/authorization here.
            // For this project, we'll assume any client calling this is the admin.
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup);
            _logger.LogInformation($"Admin client {Context.ConnectionId} registered and joined group '{AdminGroup}'.");
            // Send initial state to the newly registered admin
            await Clients.Caller.SendAsync("AdminInitialState", new AdminDashboardData(_gameService));
        }

        // Method for admin client to request the current dashboard state
        public async Task RequestAdminDashboardState()
        {
            // Ensure caller is an admin (optional check if RegisterAdminClient is always called first)
            // For simplicity, we send it. In a real app, check group membership or a claim.
            _logger.LogInformation($"Admin client {Context.ConnectionId} requested dashboard state.");
            await Clients.Caller.SendAsync("AdminDashboardUpdate", new AdminDashboardData(_gameService));
        }


        // --- Helper method to broadcast admin updates ---
        private async Task BroadcastAdminUpdate(string messageType, object payload = null!)
        {
            _logger.LogInformation($"Broadcasting admin update: {messageType}");
            if (payload == null)
            {
                // Send a full dashboard update if no specific payload
                await Clients.Group(AdminGroup).SendAsync("AdminDashboardUpdate", new AdminDashboardData(_gameService));
            }
            else
            {
                await Clients.Group(AdminGroup).SendAsync(messageType, payload);
            }
        }

        // Overload to send full state update
        private async Task BroadcastAdminFullStateUpdate()
        {
            _logger.LogInformation("Broadcasting full admin dashboard state update.");
            await Clients.Group(AdminGroup).SendAsync("AdminDashboardUpdate", new AdminDashboardData(_gameService));
        }


        // --- Existing Methods Modified/Enhanced to notify Admins ---

        public async Task JoinGame(string playerName)
        {
            _logger.LogInformation($"Player {playerName} attempting to join with ConnectionId {Context.ConnectionId}.");
            var player = _gameService.RegisterPlayer(playerName, Context.ConnectionId);

            if (player == null)
            {
                await Clients.Caller.SendAsync("JoinFailed", "Player name might be taken or another issue occurred.");
                _logger.LogWarning($"Player {playerName} failed to join (name taken or other).");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, "GamePlayers");
            await Clients.Caller.SendAsync("JoinedSuccess", playerName, _gameService.MinRange, _gameService.MaxRange, _gameService.IsRoundActive, _gameService.CurrentRoundNumber);
            await Clients.Group("GamePlayers").SendAsync("PlayerJoined", playerName, _gameService.ConnectedPlayers.Count);
            _logger.LogInformation($"Player {player.PlayerName} joined. Total players: {_gameService.ConnectedPlayers.Count}");

            await BroadcastAdminFullStateUpdate(); // Player joined, update admin

            if (!_gameService.IsRoundActive && !_gameService.IsGameFinished && _gameService.ConnectedPlayers.Any())
            {
                var (roundStarted, message) = _gameService.StartNewRound();
                if (roundStarted)
                {
                    _logger.LogInformation($"New round automatically started. Range: {_gameService.MinRange}-{_gameService.MaxRange}, Secret: {_gameService.SecretNumber}");
                    await Clients.Group("GamePlayers").SendAsync("NewRound", _gameService.MinRange, _gameService.MaxRange, _gameService.CurrentRoundNumber);
                    await BroadcastAdminFullStateUpdate(); // New round started, update admin
                }
                else
                {
                    _logger.LogWarning($"Could not auto-start new round: {message}");
                    if (_gameService.IsGameFinished)
                    {
                        await HandleGameFinished(); // This also broadcasts admin update
                    }
                }
            }
            else if (_gameService.IsRoundActive)
            {
                await Clients.Caller.SendAsync("NewRound", _gameService.MinRange, _gameService.MaxRange, _gameService.CurrentRoundNumber);
            }
            else if (_gameService.IsGameFinished)
            {
                await HandleGameFinishedForCaller();
            }
        }

        public async Task SubmitGuess(int guessedNumber)
        {
            var player = _gameService.GetPlayerByConnectionId(Context.ConnectionId);
            if (player == null)
            {
                await Clients.Caller.SendAsync("Error", "You are not registered in the game.");
                return;
            }

            if (_gameService.IsGameFinished)
            {
                await Clients.Caller.SendAsync("GuessFeedback", "The game has already finished.", false, true, null);
                return;
            }
            if (!_gameService.IsRoundActive)
            {
                await Clients.Caller.SendAsync("GuessFeedback", "No round is currently active.", false, false, null);
                return;
            }

            _logger.LogInformation($"Player {player.PlayerName} guessed {guessedNumber}. Current Secret: {_gameService.SecretNumber}");
            var (feedback, isCorrect, roundOver, winnerName) = _gameService.ProcessGuess(player.PlayerName, guessedNumber);

            await Clients.Caller.SendAsync("GuessFeedback", feedback, isCorrect, roundOver, winnerName);
            await Clients.Group("GamePlayers").SendAsync("PlayerGuessed", player.PlayerName, guessedNumber, feedback);

            // Admin Update for guess
            // We can send a specific guess update or the full state. Full state is simpler for now.
            await BroadcastAdminFullStateUpdate(); // Guess made, update admin

            if (roundOver && isCorrect)
            {
                _logger.LogInformation($"Round {_gameService.CurrentRoundNumber} over. Winner: {winnerName}. Secret was {_gameService.SecretNumber}");
                await Clients.Group("GamePlayers").SendAsync("RoundOver", winnerName, _gameService.SecretNumber, _gameService.CurrentRoundNumber);

                // Admin update for round over happens here as well via BroadcastAdminFullStateUpdate or a specific message
                // This is already covered by the BroadcastAdminFullStateUpdate() after ProcessGuess
                // await BroadcastAdminUpdate("AdminRoundEnded", new { Winner = winnerName, Secret = _gameService.SecretNumber, RoundData = _gameService.GameHistory.LastOrDefault() });

                if (_gameService.IsGameFinished)
                {
                    await HandleGameFinished(); // This also broadcasts admin update
                }
                else
                {
                    await Task.Delay(3000);
                    var (roundStarted, message) = _gameService.StartNewRound();
                    if (roundStarted)
                    {
                        _logger.LogInformation($"New round automatically started after win. Range: {_gameService.MinRange}-{_gameService.MaxRange}, Secret: {_gameService.SecretNumber}");
                        await Clients.Group("GamePlayers").SendAsync("NewRound", _gameService.MinRange, _gameService.MaxRange, _gameService.CurrentRoundNumber);
                        await BroadcastAdminFullStateUpdate(); // New round started, update admin
                    }
                    else
                    {
                        _logger.LogWarning($"Could not auto-start new round after win: {message}");
                        if (_gameService.IsGameFinished)
                        {
                            await HandleGameFinished(); // This also broadcasts admin update
                        }
                    }
                }
            }
        }

        private async Task<string> UploadHistoryToHastebinAsync(string content)
        {
            using var httpClient = new HttpClient();

            const string hastebinApiToken = "7fa140911b0034a9a1587910f00d7a33ff78bacd1eda82cd8e87bacc4e8d8feac73e76a9db34e04603c9ba3b3e2903e9e76f564d3fd532517e23e40ccb750386";


            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", hastebinApiToken);

            using var stringContent = new StringContent(content, Encoding.UTF8, "text/plain");

            try
            {
                var response = await httpClient.PostAsync("https://hastebin.com/documents", stringContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Upload failed to Hastebin. Status: {StatusCode}. Error Content: {ErrorContent}", response.StatusCode, errorContent);

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401
                    {
                        _logger.LogError("Hastebin API Error: Unauthorized. Check your API token.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) // 403
                    {
                        _logger.LogError("Hastebin API Error: Forbidden. Your account might be blocked or T&C not accepted.");
                    }
                    else if (response.StatusCode == (System.Net.HttpStatusCode)413) // Payload Too Large
                    {
                        _logger.LogError("Hastebin API Error: Payload too large. Document limit is 400,000 characters or 1MB.");
                    }
                    return "Upload failed";
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Hastebin Raw JSON Response: {Json}", json);

                var jsonDoc = System.Text.Json.JsonDocument.Parse(json);

                if (jsonDoc.RootElement.TryGetProperty("key", out var keyElement))
                {
                    string key = keyElement.GetString() ?? "";
                    // URL chuẩn để xem paste: https://hastebin.com/{key}
                    string url = $"https://hastebin.com/share/{key}";
                    return url;
                }
                else
                {
                    _logger.LogError("Upload to Hastebin succeeded but no 'key' returned in JSON. Full JSON: {Json}", json);
                    return "Upload failed: Missing key in response";
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP Request Exception while uploading to Hastebin: {Message}", httpEx.Message);
                return "Upload failed: Network or HTTP error";
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON Parsing Exception while uploading to Hastebin: {Message}", jsonEx.Message);
                return "Upload failed: Invalid JSON response";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General Exception while uploading to Hastebin: {Message}", ex.Message);
                return "Upload failed: An unexpected error occurred";
            }
        }


        private async Task HandleGameFinished()
        {
            _logger.LogInformation("Game has finished. Calculating overall winner and notifying clients.");
            string? overallWinner = _gameService.GetOverallWinner();
            string historyDetails = _gameService.FormatHistoryForUpload();

            string historyUrl = await UploadHistoryToHastebinAsync(historyDetails);

            _logger.LogInformation($"Overall Winner: {overallWinner ?? "N/A"}. History URL: {historyUrl}");
            _logger.LogInformation($"Formatted History:\n{historyDetails}");

            try
            {
                // Open browser to the history URL
                Process.Start(new ProcessStartInfo
                {
                    FileName = historyUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't open browser: {ex.Message}");
            }

            await Clients.Group("GamePlayers").SendAsync("GameFinished", overallWinner, historyUrl, historyDetails);

            await BroadcastAdminFullStateUpdate();
        }

        private async Task HandleGameFinishedForCaller() // When a player joins a finished game
        {
            _logger.LogInformation($"Notifying joining player {Context.ConnectionId} that game has finished.");
            string? overallWinner = _gameService.GetOverallWinner();
            string historyDetails = _gameService.FormatHistoryForUpload();
            string historyUrl = await UploadHistoryToHastebinAsync(historyDetails);
            await Clients.Caller.SendAsync("GameFinished", overallWinner, historyUrl, historyDetails);
            // No admin broadcast needed here as the game state hasn't changed for admins
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = _gameService.RemovePlayer(Context.ConnectionId);
            if (player != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "GamePlayers");
                await Clients.Group("GamePlayers").SendAsync("PlayerLeft", player.PlayerName, _gameService.ConnectedPlayers.Count);
                _logger.LogInformation($"Player {player.PlayerName} disconnected. Total players: {_gameService.ConnectedPlayers.Count}");
                await BroadcastAdminFullStateUpdate(); // Player left, update admin
            }
            // Handle admin disconnection if admin was specifically tracked by connection ID
            // If using groups, SignalR handles group membership removal automatically.
            // We don't remove from AdminGroup here as that's the client's responsibility to rejoin if it reconnects
            // or the admin UI explicitly leaves.
            _logger.LogInformation($"Client {Context.ConnectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
