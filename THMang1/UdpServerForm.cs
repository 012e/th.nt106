using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class UdpServerForm : Form
    {
        private UdpClient udpServer;
        private Thread receiveThread;
        private bool running = false;

        public UdpServerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string portText = txtPort.Text.Trim();

            if (string.IsNullOrEmpty(portText))
            {
                MessageBox.Show("Vui lòng nhập Port để lắng nghe.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(portText, out int port))
            {
                MessageBox.Show("Port không hợp lệ. Vui lòng nhập số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try 
            {
                udpServer = new UdpClient(port);
                running = true;
                receiveThread = new Thread(ReceiveData);
                receiveThread.Start();
                Log("Server đã khởi động...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi động server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveData()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            while (running)
            {
                try
                {
                    byte[] bytes = udpServer.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(bytes);
                    Invoke(new Action(() =>
                    {
                        Log($"{remoteEP.Address}: {message}");
                    }));
                }
                catch (Exception ex)
                {
                    Log("Lỗi: " + ex.Message);
                }
            }
        }

        private void Log(string message)
        {
            txtMess.AppendText(message + Environment.NewLine);
        }

        private void UdpServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            udpServer?.Close();
            receiveThread?.Join();
        }
    }
}
