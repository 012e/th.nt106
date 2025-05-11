using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace THMang1
{
    public partial class Bai2 : Form
    {
        public Bai2()
        {
            InitializeComponent();
            lsvBody.View = View.Details;
            lsvBody.Columns.Add("Email", 200);
            lsvBody.Columns.Add("From", 100);
            lsvBody.Columns.Add("Thời gian", 100);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var client = new ImapClient())
                {
                    // Kết nối tới MDaemon
                    await client.ConnectAsync("127.0.0.1", 143, SecureSocketOptions.None);

                    // Đăng nhập
                    await client.AuthenticateAsync(email, password);

                    // Mở hộp thư đến
                    var inbox = client.Inbox;
                    await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                    labTotal.Text = $"Total: {inbox.Count}";

                    var today = DateTime.Today;
                    var recentMessages = await inbox.SearchAsync(SearchQuery.DeliveredAfter(today));
                    labRecent.Text = $"Recent: {recentMessages.Count}";

                    lsvBody.Items.Clear();

                    foreach (var index in inbox.Search(SearchQuery.All))
                    {
                        var message = inbox.GetMessage(index);

                        var item = new ListViewItem(message.Subject ?? "(Không tiêu đề)");
                        item.SubItems.Add(message.From.Mailboxes.FirstOrDefault()?.Address ?? "");
                        item.SubItems.Add(message.Date.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
                        lsvBody.Items.Add(item);
                    }

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập hoặc đọc thư: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
    }
}
