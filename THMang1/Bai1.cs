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

namespace THMang1
{
    public partial class Bai1 : Form
    {
        public Bai1()
        {
            InitializeComponent();
        }

        private string CurrentContent = null;

        private void button1_Click(object sender, EventArgs e)
        {
            var clientForm = new UdpClientForm();
            clientForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var serverForm = new UdpServerForm();
            serverForm.Show();
        }
    }
}
