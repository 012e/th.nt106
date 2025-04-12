using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class UdpClientForm : Form
    {
        public UdpClientForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string portText = txtPort.Text.Trim();
            string message = txtMess.Text.Trim();

            // Kiểm tra input rỗng
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(portText) || string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ IP, Port và Thông điệp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng port
            if (!int.TryParse(portText, out int port))
            {
                MessageBox.Show("Port không hợp lệ. Vui lòng nhập số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                UdpClient client = new UdpClient();
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                client.Send(bytes, bytes.Length, ip, port);
                Log($"\nĐã gửi: {message}");
            }
            catch (Exception ex)
            {
                Log("Lỗi: " + ex.Message);
            }
        }

        private void Log(string message)
        {
            txtMess.AppendText(message + Environment.NewLine);
        }
    }
}
