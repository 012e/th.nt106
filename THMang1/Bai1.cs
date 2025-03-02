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
    public partial class Bai1 : Form
    {
        public Bai1()
        {
            InitializeComponent();
        }

        private void ResetResult()
        {
            textBox3.Text = "";
        }

        private void SetResult(int n)
        {
            textBox3.Text = n.ToString();
        }


        private void Bai1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Int32.TryParse(textBox1.Text, out int num1))
            {
                MessageBox.Show("Số thứ nhất không phải là số nguyên, hãy nhập lại.");
                return;
            }

            if (!Int32.TryParse(textBox2.Text, out int num2))
            {
                MessageBox.Show("Số thứ hai không phải là số nguyên, hãy nhập lại.");
                return;
            }

            SetResult(num1 + num2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetResult();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
