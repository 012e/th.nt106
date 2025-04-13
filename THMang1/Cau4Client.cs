using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class Cau4Client : Form
    {
        private TcpClient client = new TcpClient();
        private NetworkStream stream;

        public Cau4Client()
        {
            InitializeComponent();
            userIdTxt.Text = "Default";
            button1.Enabled = false;
        }

        private async Task SendMessage(ChatMessage message)
        {
            if (stream == null)
            {
                return;
            }
            if (message.id == null || message.id.Trim() == "" )
            {
                AppendMessage("Username can't be empty");
                return;
            }
            string json = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            if (data.Length >= 1024)
            {
                AppendMessage("Can't send, message too long");
                return;
            }
            try
            {
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch {
                AppendMessage("Failed to send to server");
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            var msg = new ChatMessage
            {
                id = userIdTxt.Text,
                message = messageTxt.Text,
            };

            await SendMessage(msg);
        }

        private void AppendMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendMessage), message);
            }
            else
            {
                try
                {
                    textBox1.AppendText(message + Environment.NewLine);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        private async Task ReceiveLoop()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (client.Connected)
                {
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    }
                    catch
                    {
                        break;
                    }

                    if (bytesRead == 0) break;

                    string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    try
                    {
                        var msg = JsonSerializer.Deserialize<ChatMessage>(json);
                        if (msg != null && msg.message != null)
                        {
                            AppendMessage($"[{msg.id}] {msg.message}");
                        }
                    }
                    catch
                    {
                        AppendMessage("Received malformed message.");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendMessage("Connection error: " + ex.Message);
            }
        }



        private async void Cau4Client_Load(object sender, EventArgs e)
        {
            try
            {
                await client.ConnectAsync(IPAddress.Loopback, 8080);
                stream = client.GetStream();
                AppendMessage("Connected to server");
                button1.Enabled = true;

                _ = Task.Run(ReceiveLoop); // Start listening for messages
            }
            catch
            {
                MessageBox.Show("Failed to connect to server");
                Close();
            }
        }

        private async Task SendDisconnect()
        {
            ChatMessage chatMessage = new ChatMessage
            {
                id = userIdTxt.Text,
                message = null,
            };
            await SendMessage(chatMessage);
        }


        private async void Cau4Client_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            await SendDisconnect();
            client.Close();
            client.Dispose();
        }

    }
}
