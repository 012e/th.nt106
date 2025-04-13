using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class Cau4Server : Form
    {
        private TcpListener _listener;
        private IPAddress _ipAddress;
        private IPEndPoint _ipEndpoint;
        private Dictionary<int, TcpClient> _clients = new Dictionary<int, TcpClient>();

        public Cau4Server()
        {
            InitializeComponent();
            _ipAddress = IPAddress.Parse("127.0.0.1");
            _ipEndpoint = new IPEndPoint(_ipAddress, 8080);
            _listener = new TcpListener(_ipEndpoint);
        }

        private void Cau3Client_Load(object sender, EventArgs e)
        {
        }

        private async Task BroadcastMessageAsync(ChatMessage msg, int senderPort)
        {
            string json = JsonSerializer.Serialize(msg);
            byte[] data = Encoding.UTF8.GetBytes(json);

            foreach (var kvp in _clients.ToList())
            {
                try
                {
                    if (kvp.Value.Connected)
                    {
                        NetworkStream stream = kvp.Value.GetStream();
                        if (stream.CanWrite)
                        {
                            await stream.WriteAsync(data, 0, data.Length);
                        }
                    }
                }
                catch
                {
                    _clients.Remove(kvp.Key);
                }
            }
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
                MessageBox.Show($"Error, ${ex.Message}");
            }
            finally
            {
                _listener.Stop();
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

        private async Task HandleClientAsync(TcpClient client)
        {
            int clientPort = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

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

                    if (bytesRead == 0)
                        break; // Client disconnected

                    string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var msg = JsonSerializer.Deserialize<ChatMessage>(json);

                    if (msg == null || string.IsNullOrWhiteSpace(msg.id))
                        continue;

                    if (msg.message == null)
                    {
                        if (_clients.Remove(clientPort))
                            AppendMessage($"User '{msg.id}' disconnected.");
                        break;
                    }

                    if (!_clients.ContainsKey(clientPort))
                    {
                        _clients[clientPort] = client;
                        AppendMessage($"User '{msg.id}' connected from port {clientPort}.");
                    }

                    AppendMessage($"[{msg.id}] {msg.message}");
                    await BroadcastMessageAsync(msg, clientPort);
                }
            }
            catch (Exception ex)
            {
                AppendMessage($"Client {clientPort} error: {ex.Message}");
            }
            finally
            {
                _clients.Remove(clientPort);
                client.Close();
            }
        }

        private void Cau4Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Stop accepting new connections
                _listener.Stop();
                AppendMessage("Listener stopped.");
            }
            catch (Exception ex)
            {
                AppendMessage($"Error stopping listener: {ex.Message}");
            }
            finally
            {
                // Optionally, you can also close any remaining client connections here
                foreach (var client in _clients.Values)
                {
                    client.Close();
                }

                _listener.Server.Close();  // Close the underlying socket
                _listener.Server.Dispose(); // Dispose of the server to free resources
            }

        }
    }
}
