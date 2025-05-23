using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THMang1.Models;
using THMang1.Server.Services;

namespace THMang1.Server
{
    public partial class Server : Form
    {
        private readonly string _hubUrl = "http://localhost:8080/gameHub";
        private AdminClientSDK _adminClient;
        private bool _isConnected = false;
        private int _selectedRoundIndex = -1;
        private AdminDashboardData _latestData;

        public Server()
        {
            InitializeComponent();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            // Initialize the AdminClientSDK
            _adminClient = new AdminClientSDK(_hubUrl);
            
            // Register event handlers for SDK events
            _adminClient.AdminDashboardUpdated += HandleDashboardUpdate;
            _adminClient.ErrorOccurred += HandleError;
            _adminClient.ConnectionClosed += HandleConnectionClosed;
            _adminClient.Reconnecting += HandleReconnecting;
            _adminClient.Reconnected += HandleReconnected;

            // Set initial UI state
            UpdateUIConnectionStatus(ConnectionStatus.Disconnected);
            btnRefresh.Enabled = false;
            // Start the backend server
            
            // Set up UI update timer
            timerRefresh.Enabled = false;
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up resources
            DisconnectAsync().ConfigureAwait(false);
        }

        private async Task DisconnectAsync()
        {
            timerRefresh.Enabled = false;
            if (_adminClient != null)
            {
                await _adminClient.DisconnectAsync();
                _isConnected = false;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                await DisconnectAsync();
                btnConnect.Text = "Connect";
                UpdateUIConnectionStatus(ConnectionStatus.Disconnected);
                btnRefresh.Enabled = false;
            }
            else
            {
                UpdateUIConnectionStatus(ConnectionStatus.Connecting);
                btnConnect.Enabled = false;
                
                try
                {
                    bool success = await _adminClient.ConnectAndRegisterAdminAsync();
                    if (success)
                    {
                        // Connection process started, but we'll wait for the AdminDashboardUpdated event
                        // to confirm successful registration
                        btnConnect.Text = "Disconnect";
                        btnConnect.Enabled = true;
                        _isConnected = true;
                        btnRefresh.Enabled = true;
                        timerRefresh.Enabled = true;
                    }
                    else
                    {
                        UpdateUIConnectionStatus(ConnectionStatus.Failed);
                        btnConnect.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    UpdateUIConnectionStatus(ConnectionStatus.Failed);
                    btnConnect.Enabled = true;
                    MessageBox.Show($"Connection error: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                toolStripStatusLabel.Text = "Refreshing data...";
                await _adminClient.RequestDashboardStateAsync();
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // Auto-refresh data every interval
            if (_isConnected)
            {
                _adminClient.RequestDashboardStateAsync().ConfigureAwait(false);
            }
        }

        private void dgvGameHistory_SelectionChanged(object sender, EventArgs e)
        {
            // Show guesses for selected round
            if (dgvGameHistory.SelectedRows.Count > 0)
            {
                int rowIndex = dgvGameHistory.SelectedRows[0].Index;
                if (_latestData != null && rowIndex >= 0 && rowIndex < _latestData.FullGameHistory.Count)
                {
                    _selectedRoundIndex = rowIndex;
                    DisplayGuessesForSelectedRound();
                }
            }
        }

        #region Event Handlers for AdminClientSDK

        private async Task HandleDashboardUpdate(AdminDashboardData data)
        {
            if (InvokeRequired)
            {
                await Invoke(async () => await HandleDashboardUpdate(data));
                return;
            }

            _latestData = data;
            
            // Update UI with the data
            UpdateConnectionStatusUI();
            UpdateGameInfo(data);
            UpdatePlayerInfo(data);
            UpdateGameHistory(data);

            if (_selectedRoundIndex >= 0)
            {
                DisplayGuessesForSelectedRound();
            }
            else if (data.FullGameHistory.Count > 0 && data.IsRoundActive)
            {
                // If no round is selected but there's an active round, show the current round's guesses
                int currentRoundIndex = data.FullGameHistory.FindIndex(r => r.RoundNumber == data.CurrentRoundNumber);
                if (currentRoundIndex >= 0)
                {
                    _selectedRoundIndex = currentRoundIndex;
                    DisplayGuessesForSelectedRound();
                }
            }

            toolStripStatusLabel.Text = "Data updated successfully";
        }

        private async Task HandleError(string errorMessage)
        {
            if (InvokeRequired)
            {
                await Invoke(async () => await HandleError(errorMessage));
                return;
            }

            toolStripStatusLabel.Text = $"Error: {errorMessage}";
        }

        private async Task HandleConnectionClosed(Exception? ex)
        {
            if (InvokeRequired)
            {
                await Invoke(async () => await HandleConnectionClosed(ex));
                return;
            }

            _isConnected = false;
            UpdateUIConnectionStatus(ConnectionStatus.Disconnected);
            btnConnect.Text = "Connect";
            btnConnect.Enabled = true;
            btnRefresh.Enabled = false;
            timerRefresh.Enabled = false;

            if (ex != null)
            {
                toolStripStatusLabel.Text = $"Connection closed due to error: {ex.Message}";
            }
            else
            {
                toolStripStatusLabel.Text = "Connection closed";
            }
        }

        private async Task HandleReconnecting()
        {
            if (InvokeRequired)
            {
                await Invoke(async () => await HandleReconnecting());
                return;
            }

            UpdateUIConnectionStatus(ConnectionStatus.Reconnecting);
            toolStripStatusLabel.Text = "Attempting to reconnect...";
        }

        private async Task HandleReconnected(string? connectionId)
        {
            if (InvokeRequired)
            {
                await Invoke(async () => await HandleReconnected(connectionId));
                return;
            }

            UpdateUIConnectionStatus(ConnectionStatus.Connected);
            toolStripStatusLabel.Text = "Reconnected successfully";
        }

        #endregion

        #region UI Update Methods

        private void UpdateConnectionStatusUI()
        {
            // Update the connection status based on the current state
            UpdateUIConnectionStatus(_adminClient.Status);
        }
        
        private void UpdateUIConnectionStatus(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Disconnected:
                    lblConnectionStatus.Text = "Disconnected";
                    lblConnectionStatus.ForeColor = Color.Red;
                    break;
                case ConnectionStatus.Connecting:
                    lblConnectionStatus.Text = "Connecting...";
                    lblConnectionStatus.ForeColor = Color.Orange;
                    break;
                case ConnectionStatus.Connected:
                    lblConnectionStatus.Text = "Connected";
                    lblConnectionStatus.ForeColor = Color.Green;
                    break;
                case ConnectionStatus.Reconnecting:
                    lblConnectionStatus.Text = "Reconnecting...";
                    lblConnectionStatus.ForeColor = Color.Orange;
                    break;
                case ConnectionStatus.Failed:
                    lblConnectionStatus.Text = "Connection Failed";
                    lblConnectionStatus.ForeColor = Color.Red;
                    break;
            }
        }

        private void UpdateGameInfo(AdminDashboardData data)
        {
            // Update secret number
            lblSecretNumber.Text = data.CurrentTargetNumber.HasValue 
                ? $"Secret Number: {data.CurrentTargetNumber}" 
                : "Secret Number: Hidden";

            // Update range info
            lblMinRange.Text = $"Min Range: {data.CurrentMinRange}";
            lblMaxRange.Text = $"Max Range: {data.CurrentMaxRange}";
            
            // Update round info
            lblCurrentRound.Text = $"Current Round: {data.CurrentRoundNumber}";
            lblTotalRounds.Text = $"Total Rounds: {data.TotalRoundsToPlay}";
            
            // Update game status
            string gameStatus;
            if (data.IsGameFinished)
            {
                gameStatus = $"Game Finished - Winner: {data.OverallWinner ?? "None"}";
            }
            else if (data.IsRoundActive)
            {
                gameStatus = "Round Active";
            }
            else
            {
                gameStatus = "Round Not Started";
            }
            lblGameStatus.Text = $"Game Status: {gameStatus}";
        }

        private void UpdatePlayerInfo(AdminDashboardData data)
        {
            lblPlayerCount.Text = $"Connected Players: {data.PlayerCount}";
            
            // Update player list
            lbConnectedPlayers.Items.Clear();
            foreach (var playerName in data.ConnectedPlayerNames)
            {
                lbConnectedPlayers.Items.Add(playerName);
            }
        }

        private void UpdateGameHistory(AdminDashboardData data)
        {
            dgvGameHistory.Rows.Clear();
            
            foreach (var round in data.FullGameHistory)
            {
                var row = new object[]
                {
                    round.RoundNumber,
                    round.SecretNumber,
                    $"{round.MinRange} - {round.MaxRange}",
                    round.WinnerName ?? "No Winner",
                    round.StartTime.ToString("HH:mm:ss"),
                    round.EndTime.HasValue ? round.EndTime.Value.ToString("HH:mm:ss") : "-"
                };
                dgvGameHistory.Rows.Add(row);
            }

            // Select current round in the history if available
            if (data.IsRoundActive && data.CurrentRoundNumber > 0)
            {
                for (int i = 0; i < dgvGameHistory.Rows.Count; i++)
                {
                    if ((int)dgvGameHistory.Rows[i].Cells[0].Value == data.CurrentRoundNumber)
                    {
                        dgvGameHistory.Rows[i].Selected = true;
                        break;
                    }
                }
            }
        }

        private void DisplayGuessesForSelectedRound()
        {
            if (_latestData == null || _selectedRoundIndex < 0 || _selectedRoundIndex >= _latestData.FullGameHistory.Count)
                return;

            GameRound selectedRound = _latestData.FullGameHistory[_selectedRoundIndex];
            gbCurrentRoundGuesses.Text = $"Guesses for Round {selectedRound.RoundNumber}";
            
            dgvGuesses.Rows.Clear();
            
            foreach (var guess in selectedRound.Guesses)
            {
                var row = new object[]
                {
                    guess.PlayerName,
                    guess.GuessedNumber,
                    guess.Timestamp.ToString("HH:mm:ss"),
                    guess.IsCorrect ? "Yes" : "No",
                    guess.Feedback
                };
                dgvGuesses.Rows.Add(row);
            }
        }

        #endregion
    }
}
