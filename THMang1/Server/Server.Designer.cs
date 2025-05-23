namespace THMang1.Server
{
    partial class Server
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gbConnectionStatus = new GroupBox();
            lblConnectionStatus = new Label();
            btnConnect = new Button();
            gbGameInfo = new GroupBox();
            lblGameStatus = new Label();
            lblTotalRounds = new Label();
            lblCurrentRound = new Label();
            lblMaxRange = new Label();
            lblMinRange = new Label();
            lblSecretNumber = new Label();
            gbPlayerInfo = new GroupBox();
            lblPlayerCount = new Label();
            lbConnectedPlayers = new ListBox();
            gbGameHistory = new GroupBox();
            dgvGameHistory = new DataGridView();
            ColRound = new DataGridViewTextBoxColumn();
            ColSecretNumber = new DataGridViewTextBoxColumn();
            ColRange = new DataGridViewTextBoxColumn();
            ColWinner = new DataGridViewTextBoxColumn();
            ColStartTime = new DataGridViewTextBoxColumn();
            ColEndTime = new DataGridViewTextBoxColumn();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            btnRefresh = new Button();
            timerRefresh = new System.Windows.Forms.Timer(components);
            gbConnectionStatus.SuspendLayout();
            gbGameInfo.SuspendLayout();
            gbPlayerInfo.SuspendLayout();
            gbGameHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGameHistory).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // gbConnectionStatus
            // 
            gbConnectionStatus.Controls.Add(lblConnectionStatus);
            gbConnectionStatus.Controls.Add(btnConnect);
            gbConnectionStatus.Location = new Point(12, 12);
            gbConnectionStatus.Name = "gbConnectionStatus";
            gbConnectionStatus.Size = new Size(385, 72);
            gbConnectionStatus.TabIndex = 0;
            gbConnectionStatus.TabStop = false;
            gbConnectionStatus.Text = "Connection Status";
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Location = new Point(6, 32);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(99, 20);
            lblConnectionStatus.TabIndex = 1;
            lblConnectionStatus.Text = "Disconnected";
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(278, 28);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // gbGameInfo
            // 
            gbGameInfo.Controls.Add(lblGameStatus);
            gbGameInfo.Controls.Add(lblTotalRounds);
            gbGameInfo.Controls.Add(lblCurrentRound);
            gbGameInfo.Controls.Add(lblMaxRange);
            gbGameInfo.Controls.Add(lblMinRange);
            gbGameInfo.Controls.Add(lblSecretNumber);
            gbGameInfo.Location = new Point(12, 90);
            gbGameInfo.Name = "gbGameInfo";
            gbGameInfo.Size = new Size(385, 202);
            gbGameInfo.TabIndex = 1;
            gbGameInfo.TabStop = false;
            gbGameInfo.Text = "Game Information";
            // 
            // lblGameStatus
            // 
            lblGameStatus.AutoSize = true;
            lblGameStatus.Location = new Point(6, 167);
            lblGameStatus.Name = "lblGameStatus";
            lblGameStatus.Size = new Size(126, 20);
            lblGameStatus.TabIndex = 5;
            lblGameStatus.Text = "Game Status: N/A";
            // 
            // lblTotalRounds
            // 
            lblTotalRounds.AutoSize = true;
            lblTotalRounds.Location = new Point(6, 137);
            lblTotalRounds.Name = "lblTotalRounds";
            lblTotalRounds.Size = new Size(129, 20);
            lblTotalRounds.TabIndex = 4;
            lblTotalRounds.Text = "Total Rounds: N/A";
            // 
            // lblCurrentRound
            // 
            lblCurrentRound.AutoSize = true;
            lblCurrentRound.Location = new Point(6, 107);
            lblCurrentRound.Name = "lblCurrentRound";
            lblCurrentRound.Size = new Size(138, 20);
            lblCurrentRound.TabIndex = 3;
            lblCurrentRound.Text = "Current Round: N/A";
            // 
            // lblMaxRange
            // 
            lblMaxRange.AutoSize = true;
            lblMaxRange.Location = new Point(6, 77);
            lblMaxRange.Name = "lblMaxRange";
            lblMaxRange.Size = new Size(117, 20);
            lblMaxRange.TabIndex = 2;
            lblMaxRange.Text = "Max Range: N/A";
            // 
            // lblMinRange
            // 
            lblMinRange.AutoSize = true;
            lblMinRange.Location = new Point(6, 47);
            lblMinRange.Name = "lblMinRange";
            lblMinRange.Size = new Size(114, 20);
            lblMinRange.TabIndex = 1;
            lblMinRange.Text = "Min Range: N/A";
            // 
            // lblSecretNumber
            // 
            lblSecretNumber.AutoSize = true;
            lblSecretNumber.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSecretNumber.Location = new Point(6, 23);
            lblSecretNumber.Name = "lblSecretNumber";
            lblSecretNumber.Size = new Size(153, 20);
            lblSecretNumber.TabIndex = 0;
            lblSecretNumber.Text = "Secret Number: N/A";
            // 
            // gbPlayerInfo
            // 
            gbPlayerInfo.Controls.Add(lblPlayerCount);
            gbPlayerInfo.Controls.Add(lbConnectedPlayers);
            gbPlayerInfo.Location = new Point(12, 298);
            gbPlayerInfo.Name = "gbPlayerInfo";
            gbPlayerInfo.Size = new Size(385, 221);
            gbPlayerInfo.TabIndex = 2;
            gbPlayerInfo.TabStop = false;
            gbPlayerInfo.Text = "Player Information";
            // 
            // lblPlayerCount
            // 
            lblPlayerCount.AutoSize = true;
            lblPlayerCount.Location = new Point(6, 23);
            lblPlayerCount.Name = "lblPlayerCount";
            lblPlayerCount.Size = new Size(145, 20);
            lblPlayerCount.TabIndex = 1;
            lblPlayerCount.Text = "Connected Players: 0";
            // 
            // lbConnectedPlayers
            // 
            lbConnectedPlayers.FormattingEnabled = true;
            lbConnectedPlayers.Location = new Point(6, 46);
            lbConnectedPlayers.Name = "lbConnectedPlayers";
            lbConnectedPlayers.Size = new Size(373, 164);
            lbConnectedPlayers.TabIndex = 0;
            // 
            // gbGameHistory
            // 
            gbGameHistory.Controls.Add(dgvGameHistory);
            gbGameHistory.Location = new Point(403, 12);
            gbGameHistory.Name = "gbGameHistory";
            gbGameHistory.Size = new Size(849, 280);
            gbGameHistory.TabIndex = 3;
            gbGameHistory.TabStop = false;
            gbGameHistory.Text = "Game History";
            // 
            // dgvGameHistory
            // 
            dgvGameHistory.AllowUserToAddRows = false;
            dgvGameHistory.AllowUserToDeleteRows = false;
            dgvGameHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGameHistory.Columns.AddRange(new DataGridViewColumn[] { ColRound, ColSecretNumber, ColRange, ColWinner, ColStartTime, ColEndTime });
            dgvGameHistory.Location = new Point(6, 26);
            dgvGameHistory.Name = "dgvGameHistory";
            dgvGameHistory.ReadOnly = true;
            dgvGameHistory.RowHeadersWidth = 51;
            dgvGameHistory.Size = new Size(837, 248);
            dgvGameHistory.TabIndex = 0;
            dgvGameHistory.SelectionChanged += dgvGameHistory_SelectionChanged;
            // 
            // ColRound
            // 
            ColRound.HeaderText = "Round";
            ColRound.MinimumWidth = 6;
            ColRound.Name = "ColRound";
            ColRound.ReadOnly = true;
            ColRound.Width = 80;
            // 
            // ColSecretNumber
            // 
            ColSecretNumber.HeaderText = "Secret Number";
            ColSecretNumber.MinimumWidth = 6;
            ColSecretNumber.Name = "ColSecretNumber";
            ColSecretNumber.ReadOnly = true;
            ColSecretNumber.Width = 125;
            // 
            // ColRange
            // 
            ColRange.HeaderText = "Range";
            ColRange.MinimumWidth = 6;
            ColRange.Name = "ColRange";
            ColRange.ReadOnly = true;
            ColRange.Width = 125;
            // 
            // ColWinner
            // 
            ColWinner.HeaderText = "Winner";
            ColWinner.MinimumWidth = 6;
            ColWinner.Name = "ColWinner";
            ColWinner.ReadOnly = true;
            ColWinner.Width = 150;
            // 
            // ColStartTime
            // 
            ColStartTime.HeaderText = "Start Time";
            ColStartTime.MinimumWidth = 6;
            ColStartTime.Name = "ColStartTime";
            ColStartTime.ReadOnly = true;
            ColStartTime.Width = 150;
            // 
            // ColEndTime
            // 
            ColEndTime.HeaderText = "End Time";
            ColEndTime.MinimumWidth = 6;
            ColEndTime.Name = "ColEndTime";
            ColEndTime.ReadOnly = true;
            ColEndTime.Width = 150;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 527);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1264, 26);
            statusStrip.TabIndex = 5;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(50, 20);
            toolStripStatusLabel.Text = "Ready";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(290, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(94, 29);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // timerRefresh
            // 
            timerRefresh.Interval = 5000;
            timerRefresh.Tick += timerRefresh_Tick;
            // 
            // Server
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 553);
            Controls.Add(btnRefresh);
            Controls.Add(statusStrip);
            Controls.Add(gbGameHistory);
            Controls.Add(gbPlayerInfo);
            Controls.Add(gbGameInfo);
            Controls.Add(gbConnectionStatus);
            Name = "Server";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Game Server Admin Interface";
            FormClosing += Server_FormClosing;
            Load += Server_Load;
            gbConnectionStatus.ResumeLayout(false);
            gbConnectionStatus.PerformLayout();
            gbGameInfo.ResumeLayout(false);
            gbGameInfo.PerformLayout();
            gbPlayerInfo.ResumeLayout(false);
            gbPlayerInfo.PerformLayout();
            gbGameHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvGameHistory).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox gbConnectionStatus;
        private Label lblConnectionStatus;
        private Button btnConnect;
        private GroupBox gbGameInfo;
        private Label lblGameStatus;
        private Label lblTotalRounds;
        private Label lblCurrentRound;
        private Label lblMaxRange;
        private Label lblMinRange;
        private Label lblSecretNumber;
        private GroupBox gbPlayerInfo;
        private Label lblPlayerCount;
        private ListBox lbConnectedPlayers;
        private GroupBox gbGameHistory;
        private DataGridView dgvGameHistory;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Button btnRefresh;
        private System.Windows.Forms.Timer timerRefresh;
        private DataGridViewTextBoxColumn ColRound;
        private DataGridViewTextBoxColumn ColSecretNumber;
        private DataGridViewTextBoxColumn ColRange;
        private DataGridViewTextBoxColumn ColWinner;
        private DataGridViewTextBoxColumn ColStartTime;
        private DataGridViewTextBoxColumn ColEndTime;
    }
}