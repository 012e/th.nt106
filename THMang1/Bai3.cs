using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Bai3Client bai3Client = new Bai3Client();
            bai3Client.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cau3Server cau3Server = new Cau3Server();
            cau3Server.Show();
        }
    }
}
