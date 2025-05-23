using Microsoft.AspNetCore.SignalR.Client;

namespace THMang1.Server;

// Re-define or reference ConnectionStatus enum (same as GameClientSDK)
public enum ConnectionStatus
{
    Disconnected,
    Connecting,
    Connected,
    Reconnecting,
    Failed
}

// Placeholder for data models if not referenced from a shared library
// Ensure these match the server-side definitions exactly.
#region Data Models (Placeholders - Ideally reference from a shared library)
// --- Models/PlayerGuess.cs ---
public class PlayerGuess
{
    public string PlayerName { get; set; } = string.Empty;
    public int GuessedNumber { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsCorrect { get; set; }
    public string Feedback { get; set; } = string.Empty;
}

// --- Models/GameRound.cs ---
public class GameRound
{
    public int RoundNumber { get; set; }
    public int MinRange { get; set; }
    public int MaxRange { get; set; }
    public int SecretNumber { get; set; }
    public string? WinnerName { get; set; }
    public List<PlayerGuess> Guesses { get; set; } = new List<PlayerGuess>();
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

// --- Models/AdminDashboardData.cs ---
public class AdminDashboardData
{
    public int? CurrentTargetNumber { get; set; }
    public int CurrentMinRange { get; set; }
    public int CurrentMaxRange { get; set; }
    public bool IsRoundActive { get; set; }
    public int CurrentRoundNumber { get; set; }
    public int TotalRoundsToPlay { get; set; }
    public bool IsGameFinished { get; set; }
    public string? OverallWinner { get; set; }
    public List<string> ConnectedPlayerNames { get; set; } = new List<string>();
    public int PlayerCount { get; set; }
    public List<GameRound> FullGameHistory { get; set; } = new List<GameRound>();
}
#endregion

public class AdminClientSDK : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    private bool _isRegisteredAsAdmin = false;

    public ConnectionStatus Status { get; private set; } = ConnectionStatus.Disconnected;

    // --- Events for the Admin UI to subscribe to ---
    /// <summary>
    /// Fired when the admin dashboard data is updated by the server.
    /// This is triggered by both "AdminInitialState" and "AdminDashboardUpdate" messages.
    /// Parameter: The AdminDashboardData object.
    /// </summary>
    public event Func<AdminDashboardData, Task>? AdminDashboardUpdated;

    /// <summary>
    /// Fired for general errors received from the server or connection issues.
    /// Parameter: errorMessage.
    /// </summary>
    public event Func<string, Task>? ErrorOccurred;

    /// <summary>
    /// Fired when the connection to the server is closed.
    /// Parameter: Exception if the closure was due to an error, null otherwise.
    /// </summary>
    public event Func<Exception?, Task>? ConnectionClosed;

    /// <summary>
    /// Fired when the client is attempting to reconnect.
    /// </summary>
    public event Func<Task>? Reconnecting;

    /// <summary>
    /// Fired when the client has successfully reconnected.
    /// The SDK will attempt to re-register as admin.
    /// Parameter: New connection ID (string, can be null).
    /// </summary>
    public event Func<string?, Task>? Reconnected;


    public AdminClientSDK(string hubUrl)
    {
        if (string.IsNullOrWhiteSpace(hubUrl))
            throw new ArgumentNullException(nameof(hubUrl));
        _hubUrl = hubUrl;
    }

    private void InitializeHubConnection()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
            .Build();

        // --- Register handlers for server-invoked methods ---

        // Handler for "AdminInitialState" (after successful registration)
        _hubConnection.On<AdminDashboardData>("AdminInitialState", async (dashboardData) =>
        {
            _isRegisteredAsAdmin = true; // Mark as registered
            await (AdminDashboardUpdated?.Invoke(dashboardData) ?? Task.CompletedTask);
        });

        // Handler for "AdminDashboardUpdate" (for ongoing updates)
        _hubConnection.On<AdminDashboardData>("AdminDashboardUpdate", async (dashboardData) =>
        {
            // Ensure we are still considered registered, or simply update if data is received
            await (AdminDashboardUpdated?.Invoke(dashboardData) ?? Task.CompletedTask);
        });


        // --- Handle connection lifecycle events ---
        _hubConnection.Closed += async (error) =>
        {
            Status = ConnectionStatus.Disconnected;
            _isRegisteredAsAdmin = false; // Reset admin registration status
            await (ConnectionClosed?.Invoke(error) ?? Task.CompletedTask);
        };

        _hubConnection.Reconnecting += async (error) =>
        {
            Status = ConnectionStatus.Reconnecting;
            _isRegisteredAsAdmin = false; // Will need to re-register
            await (Reconnecting?.Invoke() ?? Task.CompletedTask);
        };

        _hubConnection.Reconnected += async (connectionId) =>
        {
            Status = ConnectionStatus.Connected; // Connection is back
                                                 // Attempt to re-register as admin
            try
            {
                if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.InvokeAsync("RegisterAdminClient");
                    // The "AdminInitialState" event will confirm re-registration and send data.
                }
            }
            catch (Exception ex)
            {
                await (ErrorOccurred?.Invoke($"Failed to re-register as admin after reconnect: {ex.Message}") ?? Task.CompletedTask);
                // If re-registration fails, the UI needs to be aware that it might not receive admin updates.
            }
            await (Reconnected?.Invoke(connectionId) ?? Task.CompletedTask);
        };
    }

    /// <summary>
    /// Connects to the server and registers this client as an admin.
    /// </summary>
    /// <returns>True if connection and registration attempt was successful, false otherwise.</returns>
    public async Task<bool> ConnectAndRegisterAdminAsync()
    {
        if (Status == ConnectionStatus.Connected && _isRegisteredAsAdmin)
            return true;
        if (Status == ConnectionStatus.Connecting)
            return false; // Already trying

        InitializeHubConnection(); // Ensures a fresh HubConnection object if previously disposed

        if (_hubConnection == null) return false;

        Status = ConnectionStatus.Connecting;
        try
        {
            await _hubConnection.StartAsync();
            Status = ConnectionStatus.Connected;

            // After connection, register as admin.
            // The server will respond with "AdminInitialState" if successful.
            await _hubConnection.InvokeAsync("RegisterAdminClient");
            // Note: Success of registration is confirmed by the AdminInitialState event handler setting _isRegisteredAsAdmin.
            // This method returns true if the InvokeAsync call itself didn't throw.
            // A short delay might be needed if you want to check _isRegisteredAsAdmin before returning,
            // but it's better to rely on the event-driven nature.
            return true;
        }
        catch (Exception ex)
        {
            Status = ConnectionStatus.Failed;
            await (ErrorOccurred?.Invoke($"Admin connection or registration failed: {ex.Message}") ?? Task.CompletedTask);
            await (ConnectionClosed?.Invoke(ex) ?? Task.CompletedTask); // Also signal connection closed due to error
            return false;
        }
    }

    /// <summary>
    /// Requests a manual refresh of the admin dashboard state from the server.
    /// The update will arrive via the AdminDashboardUpdated event.
    /// </summary>
    public async Task RequestDashboardStateAsync()
    {
        if (Status != ConnectionStatus.Connected || _hubConnection == null || !_isRegisteredAsAdmin)
        {
            await (ErrorOccurred?.Invoke("Not connected or not registered as admin to request dashboard state.") ?? Task.CompletedTask);
            return;
        }

        try
        {
            await _hubConnection.InvokeAsync("RequestAdminDashboardState");
        }
        catch (Exception ex)
        {
            await (ErrorOccurred?.Invoke($"Error requesting dashboard state: {ex.Message}") ?? Task.CompletedTask);
        }
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            // Unregistering from group is handled by server on disconnect, or you can add an explicit LeaveAdminGroup hub method.
            await _hubConnection.StopAsync();
            // _hubConnection.DisposeAsync() is handled by IAsyncDisposable
        }
        _isRegisteredAsAdmin = false;
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
        _isRegisteredAsAdmin = false;
        GC.SuppressFinalize(this);
    }
}
