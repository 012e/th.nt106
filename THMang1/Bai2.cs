using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Bai2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Double.TryParse(inputSo1.Text, out double num1))
            {
                MessageBox.Show("Số thứ nhất không phải là số, hãy nhập lại.");
                return;
            }

            if (!Double.TryParse(inputSo2.Text, out double num2))
            {
                MessageBox.Show("Số thứ hai không phải là số, hãy nhập lại.");
                return;
            }

            if (!Double.TryParse(inputSo3.Text, out double num3))
            {
                MessageBox.Show("Số thứ ba không phải là số, hãy nhập lại.");
                return;
            }

            double min = Math.Min(num1, Math.Min(num2, num3));
            double max = Math.Max(num1, Math.Max(num2, num3));

            outputMax.Text = max.ToString();
            outputMin.Text = min.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            inputSo1.Text = "";
            inputSo2.Text = "";
            inputSo3.Text = "";
            outputMax.Text = "";
            outputMin.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
