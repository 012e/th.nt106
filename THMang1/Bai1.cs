using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace THMang1
{
    public partial class Bai1 : Form
    {
        public Bai1()
        {
            InitializeComponent();
        }

        private string CurrentContent = null;

        private void btnSend_Click(object sender, EventArgs e)
        {
            string mailfrom = txtFrom.Text.Trim();
            string mailto = txtTo.Text.Trim();
            string password = txtPassword.Text.Trim();
            string subject = txtSubject.Text.Trim();
            string body = rtbBody.Text;

            // Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(mailfrom) ||
                string.IsNullOrWhiteSpace(mailto) ||
                string.IsNullOrWhiteSpace(subject) ||
                string.IsNullOrWhiteSpace(body))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SmtpClient smtpClient = new SmtpClient("127.0.0.1", 25)) // localhost and port 25
                {
                    smtpClient.EnableSsl = false;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(mailfrom, password);

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(mailfrom);
                        message.To.Add(mailto);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true; // Set IsBodyHtml to true means you can send HTML email.

                        message.BodyEncoding = Encoding.UTF8;
                        message.SubjectEncoding = Encoding.UTF8;

                        smtpClient.Send(message);
                        MessageBox.Show("Gửi email thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể gửi email: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
