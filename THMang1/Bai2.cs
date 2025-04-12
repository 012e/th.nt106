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

namespace THMang1
{
    public partial class Bai2 : Form
    {
        public Bai2()
        {
            InitializeComponent();
        }

        private Socket listenerSocket;
        private Thread listenThread;
        private bool isListening = false;

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!isListening)
            {
                listenThread = new Thread(StartListening);
                listenThread.IsBackground = true;
                listenThread.Start();
                isListening = true;
                Log("Đang lắng nghe trên cổng 8080...");
                btnListen.Enabled = false;
            }
        }

        private void StartListening()
        {
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 8080);
                listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenerSocket.Bind(localEndPoint);
                listenerSocket.Listen(10);

                while (isListening)
                {
                    Socket clientSocket = listenerSocket.Accept();
                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log("Lỗi: " + ex.Message);
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = clientSocket.Receive(buffer)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Invoke(new Action(() =>
                    {
                        Log("Nhận từ Telnet: " + message);
                    }));
                }
            }
            catch (SocketException se)
            {
                Invoke(new Action(() =>
                {
                    Log("Client ngắt kết nối: " + se.Message);
                }));
            }
            finally
            {
                clientSocket.Close();
            }
        }

        private void Log(string message)
        {
            txtMess.AppendText(message + Environment.NewLine);
        }

        private void TelnetListenerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isListening = false;
            try
            {
                listenerSocket?.Close();
            }
            catch { }
        }
    }
}
