namespace THMang1
{
    partial class Bai5
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
            this.LsView = new System.Windows.Forms.ListView();
            this.BtnGetFiles = new System.Windows.Forms.Button();
            this.BtnGetFolder = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LsView
            // 
            this.LsView.HideSelection = false;
            this.LsView.Location = new System.Drawing.Point(12, 12);
            this.LsView.Name = "LsView";
            this.LsView.Size = new System.Drawing.Size(740, 357);
            this.LsView.TabIndex = 0;
            this.LsView.UseCompatibleStateImageBehavior = false;
            // 
            // BtnGetFiles
            // 
            this.BtnGetFiles.Location = new System.Drawing.Point(477, 406);
            this.BtnGetFiles.Name = "BtnGetFiles";
            this.BtnGetFiles.Size = new System.Drawing.Size(117, 38);
            this.BtnGetFiles.TabIndex = 2;
            this.BtnGetFiles.Text = "Xem Files";
            this.BtnGetFiles.UseVisualStyleBackColor = true;
            this.BtnGetFiles.Click += new System.EventHandler(this.BtnGetFiles_Click);
            // 
            // BtnGetFolder
            // 
            this.BtnGetFolder.Location = new System.Drawing.Point(173, 406);
            this.BtnGetFolder.Name = "BtnGetFolder";
            this.BtnGetFolder.Size = new System.Drawing.Size(117, 38);
            this.BtnGetFolder.TabIndex = 1;
            this.BtnGetFolder.Text = "Chọn Folder";
            this.BtnGetFolder.UseVisualStyleBackColor = true;
            this.BtnGetFolder.Click += new System.EventHandler(this.BtnGetFolder_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 375);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(740, 22);
            this.txtPath.TabIndex = 3;
            // 
            // Bai5
            // 
            this.ClientSize = new System.Drawing.Size(764, 480);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.BtnGetFolder);
            this.Controls.Add(this.BtnGetFiles);
            this.Controls.Add(this.LsView);
            this.Name = "Bai5";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView LsView;
        private System.Windows.Forms.Button BtnGetFiles;
        private System.Windows.Forms.Button BtnGetFolder;
        private System.Windows.Forms.TextBox txtPath;
    }
}