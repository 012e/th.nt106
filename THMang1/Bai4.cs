using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using THMang1;

namespace THMang1
{
    public partial class Bai4 : Form
    {
        public Bai4()
        {
            InitializeComponent();
        }

        public List<HocVien> danhSachHocVien = new List<HocVien>();

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (IsValideInput())
            {
                try
                {
                    string mssv = txtMSSV.Text;
                    string hoten = txtName.Text;
                    string dienthoai = txtPhone.Text;
                    float diemToan = float.Parse(txtMath.Text);
                    float diemVan = float.Parse(txtLiterature.Text);

                    HocVien hv = new HocVien(mssv, hoten, dienthoai, diemToan, diemVan);
                    danhSachHocVien.Add(hv);

                    MessageBox.Show("Thêm sinh viên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi nhập dữ liệu: " + ex.Message);
                }
            }
        }

        private void BtnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream("input.txt", FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, danhSachHocVien);
                }
                MessageBox.Show("Ghi file thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi file: " + ex.Message);
            }          
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            try
            {
                List<HocVien> dsHocVien;
                using (FileStream fs = new FileStream("input.txt", FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    dsHocVien = (List<HocVien>)bf.Deserialize(fs);
                }

                using (StreamWriter sw = new StreamWriter("output.txt"))
                {
                    foreach (var hv in dsHocVien)
                    {
                        sw.WriteLine($"{hv.MSSV}");
                        sw.WriteLine($"{hv.HoTen}");
                        sw.WriteLine($"{hv.DienThoai}");
                        sw.WriteLine($"{hv.DiemToan}");
                        sw.WriteLine($"{hv.DiemVan}");
                        sw.WriteLine($"{hv.DTB}");
                    }
                }

                rtxtScreen.Text = File.ReadAllText("output.txt");

                MessageBox.Show("Đọc file và ghi kết quả thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đọc file: " + ex.Message);
            }
        }

        private bool IsValideInput()
        {
            if (string.IsNullOrEmpty(txtMSSV.Text))
            {
                MessageBox.Show("Mã số sinh viên không được để trống!");
                txtMSSV.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Họ tên không được để trống!");
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không được để trống!");
                txtPhone.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtMath.Text))
            {
                MessageBox.Show("Điểm toán không được để trống!");
                txtMath.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtLiterature.Text))
            {
                MessageBox.Show("Điểm văn không được để trống!");
                txtLiterature.Focus();
                return false;
            }

            float math = float.Parse(txtMath.Text);
            float literature = float.Parse(txtLiterature.Text);

            if (math < 0 || math > 10)
            {
                MessageBox.Show("Điểm toán phải nằm trong khoảng 0-10!");
                txtMath.Focus();
                return false;
            }

            if (literature < 0 || literature > 10)
            {
                MessageBox.Show("Điểm văn phải nằm trong khoảng 0-10!");
                txtLiterature.Focus();
                return false;
            }
            return true;
        }
    }
}
