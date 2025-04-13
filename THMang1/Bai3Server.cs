using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class Cau3Server : Form
    {
        private TcpListener _listener;
        private IPAddress _ipAddress;
        private IPEndPoint _ipEndpoint;

        public Cau3Server()
        {
            InitializeComponent();
            _ipAddress = IPAddress.Parse("127.0.0.1");
            _ipEndpoint = new IPEndPoint(_ipAddress, 8080);
            _listener = new TcpListener(_ipEndpoint);
        }

        private void Cau3Client_Load(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            try
            {
                _listener.Start();
                AppendMessage("Listening on 127.0.0.1:8080...");

                while (true)
                {
                    TcpClient client;
                    try
                    {
                        client = await _listener.AcceptTcpClientAsync();
                    }
                    catch (ObjectDisposedException)
                    {
                        AppendMessage("Listener stopped.");
                        break; // Listener was stopped intentionally
                    }
                    catch (SocketException ex)
                    {
                        AppendMessage("Socket closed: " + ex.Message);
                        break; // Break loop on stop
                    }

                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                AppendMessage("Error: " + ex.Message);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        AppendMessage("Received: " + message);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendMessage("Client error: " + ex.Message);
            }
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
                    richTextBox1.AppendText(message + Environment.NewLine);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        private void Cau3Server_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void Cau3Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _listener.Stop();
            }
            catch { }
        }
    }
}
