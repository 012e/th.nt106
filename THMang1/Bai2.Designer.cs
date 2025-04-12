namespace THMang1
{
    partial class Bai2
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
            this.btnListen = new System.Windows.Forms.Button();
            this.txtMess = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(435, 26);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(106, 41);
            this.btnListen.TabIndex = 4;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // txtMess
            // 
            this.txtMess.Location = new System.Drawing.Point(62, 82);
            this.txtMess.Name = "txtMess";
            this.txtMess.ReadOnly = true;
            this.txtMess.Size = new System.Drawing.Size(479, 478);
            this.txtMess.TabIndex = 3;
            this.txtMess.Text = "";
            // 
            // Bai2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 610);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.txtMess);
            this.Name = "Bai2";
            this.Text = "Bai2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.RichTextBox txtMess;
    }
}