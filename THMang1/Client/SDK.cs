using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using THMang1.Models;

namespace THMang1.Client;

public enum ConnectionStatus
{
    Disconnected,
    Connecting,
    Connected,
    Reconnecting,
    Failed
}

public class GameClientSDK : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    private string _currentPlayerName = string.Empty;

    public ConnectionStatus Status { get; private set; } = ConnectionStatus.Disconnected;

    // --- Events for the UI to subscribe to ---
    /// <summary>
    /// Fired when the client successfully joins the game.
    /// Parameters: playerName, initialMinRange, initialMaxRange, isRoundInitiallyActive, initialCurrentRoundNumber
    /// </summary>
    public event Func<string, int, int, bool, int, Task>? JoinedSuccess;

    /// <summary>
    /// Fired if joining the game fails.
    /// Parameter: reason
    /// </summary>
    public event Func<string, Task>? JoinFailed;

    /// <summary>
    /// Fired when a new round starts.
    /// Parameters: minRange, maxRange, currentRoundNumber
    /// </summary>
    public event Func<int, int, int, Task>? NewRoundStarted;

    /// <summary>
    /// Fired with feedback after a guess.
    /// Parameters: feedbackMessage, isCorrect, isRoundOver, winnerName (if round is over and correct)
    /// </summary>
    public event Func<string, bool, bool, string?, Task>? GuessFeedbackReceived;

    /// <summary>
    /// Fired when a round is over because a player guessed correctly.
    /// Parameters: winnerName, secretNumberGuessed, roundNumberOfWinner
    /// </summary>
    public event Func<string, int, int, Task>? RoundOver;

    /// <summary>
    /// Fired when another player joins the game.
    /// Parameters: playerName, newPlayerCount
    /// </summary>
    public event Func<string, int, Task>? PlayerJoinedGame;

    /// <summary>
    /// Fired when another player leaves the game.
    /// Parameters: playerName, newPlayerCount
    /// </summary>
    public event Func<string, int, Task>? PlayerLeftGame;

    /// <summary>
    /// Fired when a player makes a guess (for all clients to see).
    /// Parameters: playerName, guessedNumber, feedbackMessage
    /// </summary>
    public event Func<string, int, string, Task>? PlayerGuessed;

    /// <summary>
    /// Fired when the entire game is finished.
    /// Parameters: overallWinnerName, historyUrl (placeholder for now), historyDetails (for client to save)
    /// </summary>
    public event Func<string?, string, string, Task>? GameFinished;

    /// <summary>
    /// Fired for general errors from the server.
    /// Parameter: errorMessage
    /// </summary>
    public event Func<string, Task>? ErrorReceived;

    /// <summary>
    /// Fired when the connection to the server is closed.
    /// Parameter: error message if any
    /// </summary>
    public event Func<Exception?, Task>? ConnectionClosed;

    /// <summary>
    /// Fired when the client is attempting to reconnect.
    /// </summary>
    public event Func<Task>? Reconnecting;

    /// <summary>
    /// Fired when the client has successfully reconnected.
    /// Parameter: new connection ID (string, can be null)
    /// </summary>
    public event Func<string?, Task>? Reconnected;


    public GameClientSDK(string hubUrl)
    {
        if (string.IsNullOrWhiteSpace(hubUrl))
            throw new ArgumentNullException(nameof(hubUrl));
        _hubUrl = hubUrl;
    }

    private void InitializeHubConnection()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl, options =>
            {
                // Configure options if needed, e.g., for transport types or timeouts
                // options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) }) // Example reconnect policy
            .Build();

        // --- Register handlers for server-invoked methods ---
        _hubConnection.On<string, int, int, bool, int>("JoinedSuccess", async (playerName, min, max, isActive, roundNum) =>
            JoinedSuccess?.Invoke(playerName, min, max, isActive, roundNum));

        _hubConnection.On<string>("JoinFailed", async (reason) =>
            JoinFailed?.Invoke(reason));

        _hubConnection.On<int, int, int>("NewRound", async (min, max, roundNum) =>
            NewRoundStarted?.Invoke(min, max, roundNum));

        _hubConnection.On<string, bool, bool, string?>("GuessFeedback", async (feedback, isCorrect, roundOver, winner) =>
            GuessFeedbackReceived?.Invoke(feedback, isCorrect, roundOver, winner));

        _hubConnection.On<string, int, int>("RoundOver", async (winner, secret, roundNum) =>
            RoundOver?.Invoke(winner, secret, roundNum));

        _hubConnection.On<string, int>("PlayerJoined", async (name, count) =>
            PlayerJoinedGame?.Invoke(name, count));

        _hubConnection.On<string, int>("PlayerLeft", async (name, count) =>
            PlayerLeftGame?.Invoke(name, count));

        _hubConnection.On<string, int, string>("PlayerGuessed", async (name, guess, feedback) =>
            PlayerGuessed?.Invoke(name, guess, feedback));

        _hubConnection.On<string?, string, string>("GameFinished", async (winner, url, details) =>
        {
            GameFinished?.Invoke(winner, url, details);
        });

        _hubConnection.On<string>("Error", async (errorMsg) => // Generic error handler
            ErrorReceived?.Invoke(errorMsg));

        _hubConnection.On<AdminDashboardData>("AdminInitialState", async (dashboardData) =>
        {
            Console.WriteLine(dashboardData);
        });



        // --- Handle connection lifecycle events ---
        _hubConnection.Closed += async (error) =>
        {
            Status = ConnectionStatus.Disconnected;
            await (ConnectionClosed?.Invoke(error) ?? Task.CompletedTask);
        };

        _hubConnection.Reconnecting += async (error) => // error can be null
        {
            Status = ConnectionStatus.Reconnecting;
            await (Reconnecting?.Invoke() ?? Task.CompletedTask);
        };

        _hubConnection.Reconnected += async (connectionId) =>
        {
            Status = ConnectionStatus.Connected;
            // Important: If reconnected, the server might not know the player name for this new connectionId.
            // A robust SDK might need to re-register the player.
            // For now, we'll just notify. The WinForms app might need to re-join or re-identify.
            // Or, better, if _currentPlayerName is set, try to rejoin.
            if (!string.IsNullOrWhiteSpace(_currentPlayerName))
            {
                try
                {
                    // This assumes the server's JoinGame can handle rejoining with an existing name
                    // if the previous connection ID was lost. The current GameService
                    // might need adjustment for this (e.g., update connection ID for existing player).
                    // For now, let's rely on the server's `RegisterPlayer` logic which checks for name collision.
                    // A more complex re-join might involve a different server method.
                    await _hubConnection.InvokeAsync("JoinGame", _currentPlayerName);
                    // The JoinedSuccess/JoinFailed events will then be triggered by the server's response
                }
                catch (Exception ex)
                {
                    ErrorReceived?.Invoke($"Error re-joining after reconnect: {ex.Message}");
                }
            }
            await (Reconnected?.Invoke(connectionId) ?? Task.CompletedTask);
        };
    }

    public async Task<bool> ConnectAsync(string playerName)
    {
        if (Status == ConnectionStatus.Connected || Status == ConnectionStatus.Connecting)
            return true; // Or false if you want to prevent multiple connect calls

        _currentPlayerName = playerName; // Store for potential re-connection
        InitializeHubConnection(); // Initialize (or re-initialize if DisconnectAsync was called)

        if (_hubConnection == null) return false; // Should not happen if InitializeHubConnection works

        Status = ConnectionStatus.Connecting;
        try
        {
            await _hubConnection.StartAsync();
            Status = ConnectionStatus.Connected;

            // After connection, tell the server we want to join the game.
            // The server will respond with JoinedSuccess or JoinFailed.
            await _hubConnection.InvokeAsync("JoinGame", playerName);
            return true;
        }
        catch (Exception ex)
        {
            Status = ConnectionStatus.Failed;
            ErrorReceived?.Invoke($"Connection failed: {ex.Message}");
            // You might want to call ConnectionClosed here as well or ensure the UI knows.
            ConnectionClosed?.Invoke(ex);
            return false;
        }
    }

    public async Task SubmitGuessAsync(int guessedNumber)
    {
        if (Status != ConnectionStatus.Connected || _hubConnection == null)
        {
            ErrorReceived?.Invoke("Not connected to the server.");
            return;
        }

        try
        {
            await _hubConnection.InvokeAsync("SubmitGuess", guessedNumber);
        }
        catch (Exception ex)
        {
            ErrorReceived?.Invoke($"Error submitting guess: {ex.Message}");
        }
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            // _hubConnection.DisposeAsync() will be called by IAsyncDisposable
        }
        // No need to set _currentPlayerName to empty here, as it might be useful for reconnect if the app UI allows.
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
        GC.SuppressFinalize(this);
    }
}

