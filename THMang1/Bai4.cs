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

        private void button2_Click(object sender, EventArgs e)
        {
            Cau4Server w = new Cau4Server();
            w.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cau4Client w = new Cau4Client();
            w.Show();
        }
    }
}
