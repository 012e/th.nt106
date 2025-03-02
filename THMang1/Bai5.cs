using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        }

        private void DisplayScores(List<double> scores)
        {
            tableLayoutPanel1.Controls.Clear();
            int row = 0;
            int col = 0;
            for (int i = 0; i < scores.Count; ++i)
            {
                double score = scores[i];
                string s = $"Môn {i + 1}: {score.ToString("F2")}đ";
                Label lbl = new Label { Text = s, AutoSize = true };
                tableLayoutPanel1.Controls.Add(lbl, col, row);

                col++;
                if (col >= tableLayoutPanel1.ColumnCount)  // Move to next column when row limit is reached
                {
                    row++;
                    col = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var scores = ScoreList.FromString(input.Text);
                DisplayScores(scores.Scores);

                DisplayScoreDetails(scores);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ResetData();
            }
        }

        private void ResetData()
        {
            tableLayoutPanel1.Controls.Clear();
            txtTB.Text = "";
            txtMax.Text = "";
            txtPass.Text = "";
            txtGrade.Text = "";
            txtMin.Text = "";
            txtFailed.Text = "";
        }

        private void DisplayScoreDetails(ScoreList scores)
        {
            txtTB.Text = scores.GetAverage().ToString("F2");
            txtMax.Text = scores.GetMax().ToString("F2") + "đ";
            txtPass.Text = scores.GetPassedCount().ToString();
            txtGrade.Text = scores.GetClassification().ToString();
            txtMin.Text = scores.GetMin().ToString("F2") + "đ";
            txtFailed.Text = scores.GetFailedCount().ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
