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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bai1 f = new Bai1();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bai2 f = new Bai2();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bai3 f = new Bai3();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bai4 f = new Bai4();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bai5 f = new Bai5();
            f.Show();
        }
    }
}
