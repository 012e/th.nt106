using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class Bai3 : Form
    {
        public Bai3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Vui lòng nhập URL.");
                return;
            }

            string filePath = txtSavePath.Text.Trim();

            // Mở SaveFileDialog
            if (string.IsNullOrEmpty(filePath))
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
                    saveFileDialog.Title = "Chọn nơi lưu file HTML";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filePath = saveFileDialog.FileName;
                        txtSavePath.Text = filePath;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                // Thêm ".html" nếu chưa có
                if (!filePath.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                {
                    filePath += ".html";
                    txtSavePath.Text = filePath;
                }
            }


            // Tải và hiển thị nội dung HTML trong RichTextBox
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    Stream response = client.OpenRead(url);
                    client.DownloadFile(url, filePath);
                }

                string htmlContent = File.ReadAllText(filePath, Encoding.UTF8);
                rtxtContent.Text = htmlContent;

                MessageBox.Show("Tải và hiển thị nội dung HTML thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

    }
}
