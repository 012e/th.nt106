using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace THMang1
{
    public partial class Bai5 : Form
    {
        public Bai5()
        {
            InitializeComponent();
            SetupLsView();
        }

        private void SetupLsView()
        {
            LsView.View = View.Details;
            LsView.FullRowSelect = true;
            LsView.Columns.Add("Tên file", 300);
            LsView.Columns.Add("Kích thước", 150);
            LsView.Columns.Add("Đuôi mở rộng", 100);
            LsView.Columns.Add("Ngày tạo", 200);
        }

        private void BtnGetFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnGetFiles_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPath.Text) || !Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Vui lòng chọn thư mục hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LsView.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(txtPath.Text);
            FileInfo[] files = di.GetFiles();

            foreach (FileInfo file in files)
            {
                ListViewItem item = new ListViewItem(file.Name);
                item.SubItems.Add(file.Length.ToString());
                item.SubItems.Add(file.Extension);
                item.SubItems.Add(file.CreationTime.ToString());

                LsView.Items.Add(item);
            }
        }
    }
}
