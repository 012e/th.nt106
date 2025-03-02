using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
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

        public static bool IsValidNumber(string number)
        {
            foreach (char c in number)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var input = txtInput.Text;
            if (!Double.TryParse(input, out double number) || !IsValidNumber(input))
            {
                MessageBox.Show("Input không hợp lệ");
                txtOutput.Text = "";
                return;
            }

            var readNumber = NumberToVietnameseConverter.ConvertToVietnameseText(number);

            TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
            txtOutput.Text = textInfo.ToTitleCase(readNumber);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtOutput.Text = "";
            txtInput.Text = "";

        }
    }
}
