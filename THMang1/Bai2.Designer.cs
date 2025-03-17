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
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileNameTxt = new System.Windows.Forms.TextBox();
            this.urlTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lineTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.wordTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.charTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(62, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 105);
            this.button1.TabIndex = 4;
            this.button1.Text = "Đọc file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(302, 35);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(479, 415);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tên file";
            // 
            // fileNameTxt
            // 
            this.fileNameTxt.Location = new System.Drawing.Point(122, 181);
            this.fileNameTxt.Name = "fileNameTxt";
            this.fileNameTxt.ReadOnly = true;
            this.fileNameTxt.Size = new System.Drawing.Size(100, 22);
            this.fileNameTxt.TabIndex = 6;
            // 
            // urlTxt
            // 
            this.urlTxt.Location = new System.Drawing.Point(122, 224);
            this.urlTxt.Name = "urlTxt";
            this.urlTxt.ReadOnly = true;
            this.urlTxt.Size = new System.Drawing.Size(100, 22);
            this.urlTxt.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 230);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "URL";
            // 
            // lineTxt
            // 
            this.lineTxt.Location = new System.Drawing.Point(122, 261);
            this.lineTxt.Name = "lineTxt";
            this.lineTxt.ReadOnly = true;
            this.lineTxt.Size = new System.Drawing.Size(100, 22);
            this.lineTxt.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Số dòng";
            // 
            // wordTxt
            // 
            this.wordTxt.Location = new System.Drawing.Point(122, 304);
            this.wordTxt.Name = "wordTxt";
            this.wordTxt.ReadOnly = true;
            this.wordTxt.Size = new System.Drawing.Size(100, 22);
            this.wordTxt.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Số từ";
            // 
            // charTxt
            // 
            this.charTxt.Location = new System.Drawing.Point(122, 346);
            this.charTxt.Name = "charTxt";
            this.charTxt.ReadOnly = true;
            this.charTxt.Size = new System.Drawing.Size(100, 22);
            this.charTxt.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Số ký tự";
            // 
            // Bai2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 450);
            this.Controls.Add(this.charTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.wordTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lineTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.urlTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fileNameTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Bai2";
            this.Text = "Bai2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileNameTxt;
        private System.Windows.Forms.TextBox urlTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox lineTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox wordTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox charTxt;
        private System.Windows.Forms.Label label5;
    }
}