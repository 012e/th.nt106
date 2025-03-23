namespace THMang1
{
    partial class Bai4
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMSSV = new System.Windows.Forms.TextBox();
            this.txtLiterature = new System.Windows.Forms.TextBox();
            this.txtMath = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.BtnWrite = new System.Windows.Forms.Button();
            this.BtnRead = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.rtxtScreen = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã số sinh viên";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Điểm Văn";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Điểm Toán";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(206, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Họ và tên";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(385, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Số điện thoại";
            // 
            // txtMSSV
            // 
            this.txtMSSV.Location = new System.Drawing.Point(34, 50);
            this.txtMSSV.Name = "txtMSSV";
            this.txtMSSV.Size = new System.Drawing.Size(100, 22);
            this.txtMSSV.TabIndex = 1;
            // 
            // txtLiterature
            // 
            this.txtLiterature.Location = new System.Drawing.Point(209, 173);
            this.txtLiterature.Name = "txtLiterature";
            this.txtLiterature.Size = new System.Drawing.Size(100, 22);
            this.txtLiterature.TabIndex = 5;
            // 
            // txtMath
            // 
            this.txtMath.Location = new System.Drawing.Point(34, 173);
            this.txtMath.Name = "txtMath";
            this.txtMath.Size = new System.Drawing.Size(100, 22);
            this.txtMath.TabIndex = 4;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(388, 50);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 22);
            this.txtPhone.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(209, 50);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 22);
            this.txtName.TabIndex = 2;
            // 
            // BtnWrite
            // 
            this.BtnWrite.Location = new System.Drawing.Point(112, 352);
            this.BtnWrite.Name = "BtnWrite";
            this.BtnWrite.Size = new System.Drawing.Size(108, 46);
            this.BtnWrite.TabIndex = 8;
            this.BtnWrite.Text = "Ghi File";
            this.BtnWrite.UseVisualStyleBackColor = true;
            this.BtnWrite.Click += new System.EventHandler(this.BtnWrite_Click);
            // 
            // BtnRead
            // 
            this.BtnRead.Location = new System.Drawing.Point(309, 352);
            this.BtnRead.Name = "BtnRead";
            this.BtnRead.Size = new System.Drawing.Size(108, 46);
            this.BtnRead.TabIndex = 9;
            this.BtnRead.Text = "Đọc File";
            this.BtnRead.UseVisualStyleBackColor = true;
            this.BtnRead.Click += new System.EventHandler(this.BtnRead_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(388, 161);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(108, 46);
            this.BtnAdd.TabIndex = 7;
            this.BtnAdd.Text = "Thêm sinh viên";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // rtxtScreen
            // 
            this.rtxtScreen.Location = new System.Drawing.Point(533, 31);
            this.rtxtScreen.Name = "rtxtScreen";
            this.rtxtScreen.Size = new System.Drawing.Size(315, 367);
            this.rtxtScreen.TabIndex = 10;
            this.rtxtScreen.Text = "";
            // 
            // Bai4
            // 
            this.ClientSize = new System.Drawing.Size(891, 472);
            this.Controls.Add(this.rtxtScreen);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.BtnRead);
            this.Controls.Add(this.BtnWrite);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtMath);
            this.Controls.Add(this.txtLiterature);
            this.Controls.Add(this.txtMSSV);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Bai4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMSSV;
        private System.Windows.Forms.TextBox txtLiterature;
        private System.Windows.Forms.TextBox txtMath;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button BtnWrite;
        private System.Windows.Forms.Button BtnRead;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.RichTextBox rtxtScreen;
    }
}