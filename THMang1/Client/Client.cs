namespace THMang1.Client
{
    public partial class Client : Form
    {
        private GameClientSDK _gameClient;
        private const string ServerUrl = "http://localhost:8080/gameHub";

        private System.Windows.Forms.Timer _guessCooldownTimer;
        private int _countdownSeconds = 3;
        private const int GUESS_COOLDOWN_TOTAL_SECONDS = 3;

        private List<string> _serverMessageHistoryForFile = new List<string>();
        private HashSet<int> _autoGuessedNumbersInRound = new HashSet<int>();

        private int _currentMinRange;
        private int _currentMaxRange;
        private bool _isRoundCurrentlyActive = false;
        private bool _canCurrentlySubmitGuess = false; // Combines cooldown and round active

        private Random _autoPlayRandom = new Random();

        public Client() // Changed constructor name from MainForm to Client
        {
            InitializeComponent(); // This is called by the designer-generated code
            InitializeGameClient();
            InitializeTimers();
            UpdateGuessingAvailability(); // Initial state
        }

        private void InitializeGameClient()
        {
            _gameClient = new GameClientSDK(ServerUrl);
            SubscribeToSdkEvents();
        }

        private void InitializeTimers()
        {
            _guessCooldownTimer = new System.Windows.Forms.Timer();
            _guessCooldownTimer.Interval = 1000; // 1 second
            _guessCooldownTimer.Tick += GuessCooldownTimer_Tick;
        }

        #region SDK Event Subscription and Handlers

        private void SubscribeToSdkEvents()
        {
            _gameClient.JoinedSuccess += OnJoinedSuccess;
            _gameClient.JoinFailed += OnJoinFailed;
            _gameClient.NewRoundStarted += OnNewRoundStarted;
            _gameClient.GuessFeedbackReceived += OnGuessFeedbackReceived;
            _gameClient.RoundOver += OnRoundOver;
            _gameClient.PlayerJoinedGame += OnPlayerJoinedGame;
            _gameClient.PlayerLeftGame += OnPlayerLeftGame;
            _gameClient.PlayerGuessed += OnPlayerGuessed; // From SDK
            _gameClient.GameFinished += OnGameFinished;
            _gameClient.ErrorReceived += OnErrorReceived;
            _gameClient.ConnectionClosed += OnConnectionClosed;
            _gameClient.Reconnecting += OnReconnecting;
            _gameClient.Reconnected += OnReconnected;
        }

        // --- SDK Event Handlers ---
        // IMPORTANT: UI updates must be marshaled to the UI thread using Invoke.

        private Task OnJoinedSuccess(string playerName, int minRange, int maxRange, bool isRoundActive, int currentRoundNum)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = $"Status: Connected as {playerName}";
                AddMessageToLog($"Successfully joined as {playerName}.");
                txtPlayerName.Enabled = false;
                btnConnect.Text = "Connected";
                btnConnect.Enabled = false;

                _isRoundCurrentlyActive = isRoundActive;
                if (isRoundActive)
                {
                    _currentMinRange = minRange;
                    _currentMaxRange = maxRange;
                    lblRangeInfo.Text = $"Range: {minRange} < X < {maxRange}";
                    lblCurrentRound.Text = $"Round: {currentRoundNum}";
                    AddMessageToLog($"Current round {currentRoundNum} is active. Guess between {minRange} and {maxRange}.");
                    _autoGuessedNumbersInRound.Clear();
                }
                else
                {
                    lblRangeInfo.Text = "Range: Waiting for round to start...";
                    lblCurrentRound.Text = $"Round: {currentRoundNum} (or waiting)";
                    if (currentRoundNum == 0) AddMessageToLog("Waiting for the first round to start.");
                    else AddMessageToLog("Waiting for the next round to start.");
                }
                ResetCountdown(); // Allow first guess if round is active
                UpdateGuessingAvailability();
                MaybeTriggerAutoPlay();
            });
            return Task.CompletedTask;
        }

        private Task OnJoinFailed(string reason)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = "Status: Join Failed";
                AddMessageToLog($"Failed to join: {reason}");
                MessageBox.Show($"Failed to join: {reason}", "Join Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPlayerName.Enabled = true;
                btnConnect.Text = "Connect";
                btnConnect.Enabled = true;
            });
            return Task.CompletedTask;
        }

        private Task OnNewRoundStarted(int min, int max, int roundNum)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _isRoundCurrentlyActive = true;
                _currentMinRange = min;
                _currentMaxRange = max;
                lblRangeInfo.Text = $"Range: {min} < X < {max}";
                lblCurrentRound.Text = $"Round: {roundNum}";
                AddMessageToLog($"--- New Round {roundNum} Started ---");
                AddMessageToLog($"Guess between {min} and {max}.");
                _autoGuessedNumbersInRound.Clear();
                txtGuess.Clear();
                ResetCountdown();
                UpdateGuessingAvailability();
                MaybeTriggerAutoPlay();
            });
            return Task.CompletedTask;
        }

        private Task OnGuessFeedbackReceived(string feedback, bool isCorrect, bool isRoundOver, string? winnerName)
        {
            this.Invoke((MethodInvoker)delegate
            {
                AddMessageToLog($"Server: {feedback}");
                if (!isCorrect) // Only start cooldown if guess was processed but incorrect or correct but not by us
                {
                    StartGuessCooldown(); // Start cooldown for next guess
                }

                if (isCorrect && winnerName == txtPlayerName.Text) // We won
                {
                    // RoundOver event will handle detailed win message for us.
                    // No need to do much here other than potentially disabling controls
                    _isRoundCurrentlyActive = false; // Our guess ended the round
                }
                else if (isRoundOver) // Round ended, possibly by someone else or game ended
                {
                    _isRoundCurrentlyActive = false;
                }
                UpdateGuessingAvailability();
            });
            return Task.CompletedTask;
        }

        private Task OnRoundOver(string winnerName, int secretNumber, int roundNumberOfWinner)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _isRoundCurrentlyActive = false;
                AddMessageToLog($"--- Round {roundNumberOfWinner} Over ---");
                AddMessageToLog($"Winner: {winnerName}! The number was {secretNumber}.");
                if (winnerName == txtPlayerName.Text)
                {
                    lblStatus.Text = "Status: You won this round!";
                }
                UpdateGuessingAvailability();
                // Next round will be triggered by server if game is not over
            });
            return Task.CompletedTask;
        }

        private Task OnPlayerJoinedGame(string playerName, int playerCount)
        {
            this.Invoke((MethodInvoker)delegate
            {
                AddMessageToLog($"{playerName} has joined the game. Total players: {playerCount}.");
            });
            return Task.CompletedTask;
        }

        private Task OnPlayerLeftGame(string playerName, int playerCount)
        {
            this.Invoke((MethodInvoker)delegate
            {
                AddMessageToLog($"{playerName} has left the game. Total players: {playerCount}.");
            });
            return Task.CompletedTask;
        }

        private Task OnPlayerGuessed(string playerName, int guessedNumber, string feedback)
        {
            this.Invoke((MethodInvoker)delegate
            {
                // Optional: Display other players' guesses if desired, server sends this
                // AddMessageToLog($"{playerName} guessed {guessedNumber}. ({feedback})");
            });
            return Task.CompletedTask;
        }

        private Task OnGameFinished(string? overallWinnerName, string historyUrl, string historyDetails)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _isRoundCurrentlyActive = false;
                UpdateGuessingAvailability();
                AddMessageToLog("--- GAME FINISHED ---");
                if (!string.IsNullOrEmpty(overallWinnerName))
                {
                    AddMessageToLog($"Overall Winner: {overallWinnerName}!");
                    lblStatus.Text = $"Status: Game Over! Winner: {overallWinnerName}";
                }
                else
                {
                    AddMessageToLog("Game finished. No overall winner determined or draw.");
                    lblStatus.Text = "Status: Game Over!";
                }
                AddMessageToLog($"Server history URL (if implemented by server): {historyUrl}");

                SaveHistoryToFile(historyDetails); // Requirement: Client saves server-provided history details

                MessageBox.Show($"Game Over! Overall Winner: {overallWinnerName ?? "N/A"}\nHistory saved to history.txt (server provided details).\nServer URL: {historyUrl}", "Game Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Requirement: Client automatically closes
                this.Close();
            });
            return Task.CompletedTask;
        }

        private Task OnErrorReceived(string errorMessage)
        {
            this.Invoke((MethodInvoker)delegate
            {
                AddMessageToLog($"SERVER ERROR: {errorMessage}");
            });
            return Task.CompletedTask;
        }

        private Task OnConnectionClosed(Exception? ex)
        {
            this.Invoke((MethodInvoker)delegate
            {
                _isRoundCurrentlyActive = false;
                lblStatus.Text = "Status: Disconnected";
                AddMessageToLog($"Connection closed. {(ex != null ? ex.Message : "")}");
                txtPlayerName.Enabled = true;
                btnConnect.Text = "Connect";
                btnConnect.Enabled = true;
                UpdateGuessingAvailability();
            });
            return Task.CompletedTask;
        }

        private Task OnReconnecting()
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = "Status: Reconnecting...";
                AddMessageToLog("Attempting to reconnect to the server...");
            });
            return Task.CompletedTask;
        }

        private Task OnReconnected(string? newConnectionId)
        {
            this.Invoke((MethodInvoker)delegate
            {
                // SDK's ConnectAsync logic (via re-invoking JoinGame) will handle re-joining.
                // The JoinedSuccess event should fire if successful.
                lblStatus.Text = "Status: Reconnected! Verifying player...";
                AddMessageToLog($"Reconnected to the server with new ID: {newConnectionId}. Attempting to re-join game...");
                // If SDK re-join fails, OnJoinFailed or OnError will notify.
            });
            return Task.CompletedTask;
        }

        #endregion

        #region UI Event Handlers (Buttons, Timers, Checkbox)

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            string playerName = txtPlayerName.Text.Trim();
            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter your player name.", "Name Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            lblStatus.Text = "Status: Connecting...";
            AddMessageToLog($"Attempting to connect as {playerName}...");
            txtPlayerName.Enabled = false;
            btnConnect.Enabled = false;

            bool success = await _gameClient.ConnectAsync(playerName);
            // If connection infrastructure fails (before JoinGame is even called),
            // OnConnectionClosed might fire. If JoinGame is called and fails, OnJoinFailed fires.
            // No explicit error handling here as SDK events cover it.
            if (!success && _gameClient.Status == ConnectionStatus.Failed)
            {
                // This case might be if _gameClient.ConnectAsync itself throws an exception before SignalR connection events fire.
                lblStatus.Text = "Status: Connection Failed";
                MessageBox.Show("Could not establish connection to the server. Check URL and server status.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPlayerName.Enabled = true;
                btnConnect.Enabled = true;
                btnConnect.Text = "Connect";
            }
        }

        private async void btnSubmitGuess_Click(object sender, EventArgs e)
        {
            if (!_canCurrentlySubmitGuess) return;

            if (!int.TryParse(txtGuess.Text, out int guessedNumber))
            {
                MessageBox.Show("Please enter a valid number for your guess.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (guessedNumber <= _currentMinRange || guessedNumber >= _currentMaxRange)
            {
                MessageBox.Show($"Your guess must be between {_currentMinRange + 1} and {_currentMaxRange - 1}.", "Out of Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddMessageToLog($"You guessed: {guessedNumber}");
            if (chkAutoPlay.Checked)
            {
                _autoGuessedNumbersInRound.Add(guessedNumber); // Record auto-guess
            }

            // StartGuessCooldown(); // Moved to OnGuessFeedbackReceived for incorrect guesses
            // This allows server to validate first, then client starts cooldown.
            // However, for UX, disabling button immediately is good.
            UpdateGuessingAvailability(isSubmitting: true); // Temporarily disable submit button

            await _gameClient.SubmitGuessAsync(guessedNumber);
            txtGuess.Clear();
            // Don't re-enable submit button here; wait for cooldown or next round.
        }

        private void GuessCooldownTimer_Tick(object? sender, EventArgs e)
        {
            _countdownSeconds--;
            lblCountdown.Text = $"Next guess in: {_countdownSeconds}s";

            if (_countdownSeconds <= 0)
            {
                ResetCountdown();
                UpdateGuessingAvailability();
                MaybeTriggerAutoPlay(); // Attempt auto-play if conditions are met
            }
        }

        private void chkAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoPlay.Checked)
            {
                AddMessageToLog("Auto-Play enabled.");
                MaybeTriggerAutoPlay();
            }
            else
            {
                AddMessageToLog("Auto-Play disabled.");
            }
        }

        private async void Client_FormClosing(object sender, FormClosingEventArgs e) // Changed from MainForm_FormClosing
        {
            if (_gameClient != null)
            {
                // Give a moment for any pending messages if the connection is active.
                // This is optional and might not be necessary for all scenarios.
                // if (_gameClient.Status == ConnectionStatus.Connected) await Task.Delay(200);
                await _gameClient.DisposeAsync();
            }
            _guessCooldownTimer?.Stop();
            _guessCooldownTimer?.Dispose();
        }

        #endregion

        #region Helper Methods

        private void AddMessageToLog(string message)
        {
            if (lstMessages.InvokeRequired)
            {
                lstMessages.Invoke((MethodInvoker)delegate { ActualAddMessage(message); });
            }
            else
            {
                ActualAddMessage(message);
            }
        }

        private void ActualAddMessage(string message)
        {
            string timedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
            lstMessages.Items.Add(timedMessage);
            _serverMessageHistoryForFile.Add(timedMessage); // Add to history for client-side full log
            if (lstMessages.Items.Count > 0)
            {
                lstMessages.SelectedIndex = lstMessages.Items.Count - 1; // Auto-scroll
            }
        }

        private void UpdateGuessingAvailability(bool isSubmitting = false)
        {
            // Called after round start, round end, guess submission, cooldown end.
            bool canGuessNow = _isRoundCurrentlyActive && _countdownSeconds <= 0;
            _canCurrentlySubmitGuess = canGuessNow;

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    btnSubmitGuess.Enabled = _canCurrentlySubmitGuess && !isSubmitting;
                    txtGuess.Enabled = _isRoundCurrentlyActive; // Allow typing even during cooldown
                    chkAutoPlay.Enabled = _isRoundCurrentlyActive || (_gameClient.Status == ConnectionStatus.Connected && !_isRoundCurrentlyActive); // Can toggle even if waiting for round
                });
            }
            else
            {
                btnSubmitGuess.Enabled = _canCurrentlySubmitGuess && !isSubmitting;
                txtGuess.Enabled = _isRoundCurrentlyActive;
                chkAutoPlay.Enabled = _isRoundCurrentlyActive || (_gameClient.Status == ConnectionStatus.Connected && !_isRoundCurrentlyActive);
            }
        }

        private void StartGuessCooldown()
        {
            _countdownSeconds = GUESS_COOLDOWN_TOTAL_SECONDS;
            lblCountdown.Text = $"Next guess in: {_countdownSeconds}s";
            _guessCooldownTimer.Start();
            UpdateGuessingAvailability(); // Disables submit button
        }

        private void ResetCountdown()
        {
            _guessCooldownTimer.Stop();
            _countdownSeconds = 0;
            lblCountdown.Text = $"Next guess in: 0s";
            UpdateGuessingAvailability(); // Enables submit button if round active
        }

        private void MaybeTriggerAutoPlay()
        {
            if (chkAutoPlay.Checked && _canCurrentlySubmitGuess && _isRoundCurrentlyActive)
            {
                // Attempt to make an auto-guess
                int guessAttempts = 0;
                int maxAttempts = 10; // Prevent infinite loop if range is small and all numbers guessed
                bool guessMade = false;

                while (!guessMade && guessAttempts < maxAttempts && (_currentMaxRange - _currentMinRange - 1) > _autoGuessedNumbersInRound.Count)
                {
                    int autoGuess = _autoPlayRandom.Next(_currentMinRange + 1, _currentMaxRange);
                    if (!_autoGuessedNumbersInRound.Contains(autoGuess))
                    {
                        txtGuess.Text = autoGuess.ToString();
                        AddMessageToLog($"Auto-playing guess: {autoGuess}");
                        // Simulate button click to use existing submission logic, including cooldown
                        btnSubmitGuess_Click(this, EventArgs.Empty);
                        guessMade = true;
                    }
                    guessAttempts++;
                }
                if (!guessMade && guessAttempts >= maxAttempts)
                {
                    AddMessageToLog("Auto-Play: Could not find a unique number to guess after several attempts or all numbers in range tried.");
                }
            }
        }

        private void SaveHistoryToFile(string serverProvidedHistoryDetails)
        {
            try
            {
                // The requirement is: "phía clients sẽ lưu toàn bộ lịch sử thông báo của server thành file history.txt"
                // This can be interpreted as saving the _serverMessageHistoryForFile which is a log of what was displayed.
                // OR "server gửi toàn bộ lịch sử trò chơi lên website ... sau đó cả clients và server tự động đóng ứng dụng."
                // "Khi trò chơi kết thúc, phía clients sẽ lưu toàn bộ lịch sử thông báo của server thành file history.txt"
                // The `GameFinished` event from the SDK now provides `historyDetails` which is the server's formatted history.
                // Let's save THAT as `history.txt` as per the stronger interpretation of the requirement.

                File.WriteAllText("history.txt", serverProvidedHistoryDetails);
                AddMessageToLog("Server game history saved to history.txt");
            }
            catch (Exception ex)
            {
                AddMessageToLog($"Error saving history: {ex.Message}");
                MessageBox.Show($"Error saving history.txt: {ex.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Designer Generated Code (Example - you'll have your own)
        // In a real project, this is in MainForm.Designer.cs
        // For brevity, I'll outline it here.
        // You would drag these controls onto your form in the VS Designer.

        private System.Windows.Forms.Label labelPlayerName;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label labelGameInfo;
        private System.Windows.Forms.Label lblRangeInfo;
        private System.Windows.Forms.Label lblCurrentRound;
        private System.Windows.Forms.Label labelGuess;
        private System.Windows.Forms.TextBox txtGuess;
        private System.Windows.Forms.Button btnSubmitGuess;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.CheckBox chkAutoPlay;
        private System.Windows.Forms.Label labelGameLog;
        private System.Windows.Forms.ListBox lstMessages;
        // private System.Windows.Forms.Label lblScore; // If you add scoring

        private void InitializeComponent()
        {
            this.labelPlayerName = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.labelGameInfo = new System.Windows.Forms.Label();
            this.lblRangeInfo = new System.Windows.Forms.Label();
            this.lblCurrentRound = new System.Windows.Forms.Label();
            this.labelGuess = new System.Windows.Forms.Label();
            this.txtGuess = new System.Windows.Forms.TextBox();
            this.btnSubmitGuess = new System.Windows.Forms.Button();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.chkAutoPlay = new System.Windows.Forms.CheckBox();
            this.labelGameLog = new System.Windows.Forms.Label();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // labelPlayerName
            // 
            this.labelPlayerName.AutoSize = true;
            this.labelPlayerName.Location = new System.Drawing.Point(12, 15);
            this.labelPlayerName.Name = "labelPlayerName";
            this.labelPlayerName.Size = new System.Drawing.Size(70, 13);
            this.labelPlayerName.TabIndex = 0;
            this.labelPlayerName.Text = "Player Name:";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Location = new System.Drawing.Point(88, 12);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(140, 20);
            this.txtPlayerName.TabIndex = 1;
            this.txtPlayerName.Text = "Player" + new Random().Next(1, 100); // Default name
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(234, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(315, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(105, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Disconnected";
            // 
            // labelGameInfo
            // 
            this.labelGameInfo.AutoSize = true;
            this.labelGameInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGameInfo.Location = new System.Drawing.Point(12, 45);
            this.labelGameInfo.Name = "labelGameInfo";
            this.labelGameInfo.Size = new System.Drawing.Size(68, 13);
            this.labelGameInfo.TabIndex = 4;
            this.labelGameInfo.Text = "Game Info:";
            // 
            // lblRangeInfo
            // 
            this.lblRangeInfo.AutoSize = true;
            this.lblRangeInfo.Location = new System.Drawing.Point(12, 65);
            this.lblRangeInfo.Name = "lblRangeInfo";
            this.lblRangeInfo.Size = new System.Drawing.Size(125, 13);
            this.lblRangeInfo.TabIndex = 5;
            this.lblRangeInfo.Text = "Range: Waiting for game...";
            // 
            // lblCurrentRound
            // 
            this.lblCurrentRound.AutoSize = true;
            this.lblCurrentRound.Location = new System.Drawing.Point(200, 65);
            this.lblCurrentRound.Name = "lblCurrentRound";
            this.lblCurrentRound.Size = new System.Drawing.Size(53, 13);
            this.lblCurrentRound.TabIndex = 6;
            this.lblCurrentRound.Text = "Round: -";
            // 
            // labelGuess
            // 
            this.labelGuess.AutoSize = true;
            this.labelGuess.Location = new System.Drawing.Point(12, 95);
            this.labelGuess.Name = "labelGuess";
            this.labelGuess.Size = new System.Drawing.Size(68, 13);
            this.labelGuess.TabIndex = 7;
            this.labelGuess.Text = "Your Guess:";
            // 
            // txtGuess
            // 
            this.txtGuess.Enabled = false;
            this.txtGuess.Location = new System.Drawing.Point(88, 92);
            this.txtGuess.Name = "txtGuess";
            this.txtGuess.Size = new System.Drawing.Size(75, 20);
            this.txtGuess.TabIndex = 8;
            // 
            // btnSubmitGuess
            // 
            this.btnSubmitGuess.Enabled = false;
            this.btnSubmitGuess.Location = new System.Drawing.Point(169, 90);
            this.btnSubmitGuess.Name = "btnSubmitGuess";
            this.btnSubmitGuess.Size = new System.Drawing.Size(90, 23);
            this.btnSubmitGuess.TabIndex = 9;
            this.btnSubmitGuess.Text = "Submit Guess";
            this.btnSubmitGuess.UseVisualStyleBackColor = true;
            this.btnSubmitGuess.Click += new System.EventHandler(this.btnSubmitGuess_Click);
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Location = new System.Drawing.Point(265, 95);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(88, 13);
            this.lblCountdown.TabIndex = 10;
            this.lblCountdown.Text = "Next guess in: 0s";
            // 
            // chkAutoPlay
            // 
            this.chkAutoPlay.AutoSize = true;
            this.chkAutoPlay.Enabled = false;
            this.chkAutoPlay.Location = new System.Drawing.Point(15, 125);
            this.chkAutoPlay.Name = "chkAutoPlay";
            this.chkAutoPlay.Size = new System.Drawing.Size(75, 17);
            this.chkAutoPlay.TabIndex = 11;
            this.chkAutoPlay.Text = "Auto-Play";
            this.chkAutoPlay.UseVisualStyleBackColor = true;
            this.chkAutoPlay.CheckedChanged += new System.EventHandler(this.chkAutoPlay_CheckedChanged);

            // 
            // labelGameLog
            // 
            this.labelGameLog.AutoSize = true;
            this.labelGameLog.Location = new System.Drawing.Point(12, 155);
            this.labelGameLog.Name = "labelGameLog";
            this.labelGameLog.Size = new System.Drawing.Size(60, 13);
            this.labelGameLog.TabIndex = 12;
            this.labelGameLog.Text = "Game Log:";
            // 
            // lstMessages
            // 
            this.lstMessages.FormattingEnabled = true;
            this.lstMessages.Location = new System.Drawing.Point(15, 175);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(480, 199);
            this.lstMessages.TabIndex = 13;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 390); // Adjust as needed
            this.Controls.Add(this.lstMessages);
            this.Controls.Add(this.labelGameLog);
            this.Controls.Add(this.chkAutoPlay);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.btnSubmitGuess);
            this.Controls.Add(this.txtGuess);
            this.Controls.Add(this.labelGuess);
            this.Controls.Add(this.lblCurrentRound);
            this.Controls.Add(this.lblRangeInfo);
            this.Controls.Add(this.labelGameInfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.labelPlayerName);
            this.Name = "MainForm";
            this.Text = "Game Đoán Số Client";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}
