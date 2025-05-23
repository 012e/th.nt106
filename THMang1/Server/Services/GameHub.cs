using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THMang1.Server.Services
{
    public class GameHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameHub> _logger;

        public GameHub(IGameService gameService, ILogger<GameHub> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        public async Task JoinGame(string playerName)
        {
            _logger.LogInformation($"Player {playerName} attempting to join with ConnectionId {Context.ConnectionId}.");
            var player = _gameService.RegisterPlayer(playerName, Context.ConnectionId);

            if (player == null)
            {
                // Name might be taken, or another registration issue.
                await Clients.Caller.SendAsync("JoinFailed", "Player name might be taken or another issue occurred.");
                _logger.LogWarning($"Player {playerName} failed to join (name taken or other).");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, "GamePlayers");
            await Clients.Caller.SendAsync("JoinedSuccess", playerName, _gameService.MinRange, _gameService.MaxRange, _gameService.IsRoundActive, _gameService.CurrentRoundNumber);
            await Clients.Group("GamePlayers").SendAsync("PlayerJoined", playerName, _gameService.ConnectedPlayers.Count);
            _logger.LogInformation($"Player {player.PlayerName} joined. Total players: {_gameService.ConnectedPlayers.Count}");


            // If no round is active and this is the first player (or meets some criteria), start a round.
            // Or you can have a separate "Start Game" button on a client/admin UI.
            // For simplicity, let's try starting a round if one isn't active and there's at least one player.
            if (!_gameService.IsRoundActive && !_gameService.IsGameFinished && _gameService.ConnectedPlayers.Any())
            {
                var (roundStarted, message) = _gameService.StartNewRound();
                if (roundStarted)
                {
                    _logger.LogInformation($"New round automatically started. Range: {_gameService.MinRange}-{_gameService.MaxRange}, Secret: {_gameService.SecretNumber}");
                    await Clients.Group("GamePlayers").SendAsync("NewRound", _gameService.MinRange, _gameService.MaxRange, _gameService.CurrentRoundNumber);
                }
                else
                {
                    _logger.LogWarning($"Could not auto-start new round: {message}");
                    if (_gameService.IsGameFinished)
                    {
                        await HandleGameFinished();
                    }
                }
            }
            else if (_gameService.IsRoundActive) // If round is already active, send current state to new player
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

            // Send specific feedback to the caller
            await Clients.Caller.SendAsync("GuessFeedback", feedback, isCorrect, roundOver, winnerName);
            // Broadcast the guess attempt to all (optional, for transparency)
            await Clients.Group("GamePlayers").SendAsync("PlayerGuessed", player.PlayerName, guessedNumber, feedback);


            if (roundOver && isCorrect)
            {
                _logger.LogInformation($"Round {_gameService.CurrentRoundNumber} over. Winner: {winnerName}. Secret was {_gameService.SecretNumber}");
                await Clients.Group("GamePlayers").SendAsync("RoundOver", winnerName, _gameService.SecretNumber, _gameService.CurrentRoundNumber);

                if (_gameService.IsGameFinished)
                {
                    await HandleGameFinished();
                }
                else
                {
                    // Optionally add a delay before starting the next round
                    // For now, let's allow client to trigger or auto-start based on some logic
                    // Let's try auto-starting next round after a short delay
                    await Task.Delay(3000); // 3 second delay
                    var (roundStarted, message) = _gameService.StartNewRound();
                    if (roundStarted)
                    {
                        _logger.LogInformation($"New round automatically started after win. Range: {_gameService.MinRange}-{_gameService.MaxRange}, Secret: {_gameService.SecretNumber}");
                        await Clients.Group("GamePlayers").SendAsync("NewRound", _gameService.MinRange, _gameService.MaxRange, _gameService.CurrentRoundNumber);
                    }
                    else
                    {
                        _logger.LogWarning($"Could not auto-start new round after win: {message}");
                        if (_gameService.IsGameFinished) // Double check if EndGame was called
                        {
                            await HandleGameFinished();
                        }
                    }
                }
            }
        }

        private async Task HandleGameFinished()
        {
            _logger.LogInformation("Game has finished. Calculating overall winner and notifying clients.");
            string? overallWinner = _gameService.GetOverallWinner();
            // In a real scenario, you'd upload history here and get a URL.
            // For now, we'll send a placeholder or skip the URL.
            // string historyUrl = await UploadHistoryToCtxtIo(_gameService.FormatHistoryForUpload());
            string historyDetails = _gameService.FormatHistoryForUpload(); // For client to save.
                                                                           // Server will also log this or upload it.

            // TODO: Implement actual upload and get URL
            string historyUrl = "History upload to ctxt.io not implemented in this step.";
            _logger.LogInformation($"Overall Winner: {overallWinner ?? "N/A"}. History URL: {historyUrl}");
            _logger.LogInformation($"Formatted History:\n{historyDetails}");


            await Clients.Group("GamePlayers").SendAsync("GameFinished", overallWinner, historyUrl, historyDetails);

            // Server side cleanup or shutdown initiation would go here.
            // For now, the game service state is simply IsGameFinished = true.
        }
        private async Task HandleGameFinishedForCaller()
        {
            _logger.LogInformation($"Notifying joining player {Context.ConnectionId} that game has finished.");
            string? overallWinner = _gameService.GetOverallWinner();
            string historyDetails = _gameService.FormatHistoryForUpload();
            string historyUrl = "History upload to ctxt.io not implemented in this step.";
            await Clients.Caller.SendAsync("GameFinished", overallWinner, historyUrl, historyDetails);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = _gameService.RemovePlayer(Context.ConnectionId);
            if (player != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "GamePlayers");
                await Clients.Group("GamePlayers").SendAsync("PlayerLeft", player.PlayerName, _gameService.ConnectedPlayers.Count);
                _logger.LogInformation($"Player {player.PlayerName} disconnected. Total players: {_gameService.ConnectedPlayers.Count}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
