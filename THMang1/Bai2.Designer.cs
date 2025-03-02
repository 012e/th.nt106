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
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.outputMax = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.inputSo2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.inputSo1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.inputSo3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.outputMin = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(537, 132);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 52);
            this.button3.TabIndex = 17;
            this.button3.Text = "Thoát";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(374, 129);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 52);
            this.button2.TabIndex = 16;
            this.button2.Text = "Xóa";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 52);
            this.button1.TabIndex = 15;
            this.button1.Text = "Tìm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Số Lớn Nhất";
            // 
            // outputMax
            // 
            this.outputMax.Location = new System.Drawing.Point(180, 301);
            this.outputMax.Name = "outputMax";
            this.outputMax.ReadOnly = true;
            this.outputMax.Size = new System.Drawing.Size(123, 22);
            this.outputMax.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Số thứ 2";
            // 
            // inputSo2
            // 
            this.inputSo2.Location = new System.Drawing.Point(417, 36);
            this.inputSo2.Name = "inputSo2";
            this.inputSo2.Size = new System.Drawing.Size(123, 22);
            this.inputSo2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Số thứ 1";
            // 
            // inputSo1
            // 
            this.inputSo1.Location = new System.Drawing.Point(127, 36);
            this.inputSo1.Name = "inputSo1";
            this.inputSo1.Size = new System.Drawing.Size(123, 22);
            this.inputSo1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(598, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Số thứ 3";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // inputSo3
            // 
            this.inputSo3.Location = new System.Drawing.Point(700, 33);
            this.inputSo3.Name = "inputSo3";
            this.inputSo3.Size = new System.Drawing.Size(123, 22);
            this.inputSo3.TabIndex = 18;
            this.inputSo3.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(534, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 16);
            this.label5.TabIndex = 21;
            this.label5.Text = "Số Bé Nhất";
            // 
            // outputMin
            // 
            this.outputMin.Location = new System.Drawing.Point(636, 298);
            this.outputMin.Name = "outputMin";
            this.outputMin.ReadOnly = true;
            this.outputMin.Size = new System.Drawing.Size(123, 22);
            this.outputMin.TabIndex = 20;
            // 
            // Bai2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.outputMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputSo3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.outputMax);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputSo2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputSo1);
            this.Name = "Bai2";
            this.Text = "Bai2";
            this.Load += new System.EventHandler(this.Bai2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox outputMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox inputSo2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputSo1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox inputSo3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox outputMin;
    }
}