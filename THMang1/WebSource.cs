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
    public partial class WebSource : Form
    {
        public WebSource(string htmlContent)
        {
            InitializeComponent();
            richTextBox1.Text = htmlContent;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
