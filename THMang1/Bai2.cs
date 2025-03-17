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
    public partial class Bai2 : Form
    {
        public Bai2()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "All Files|*.*";
                    openFileDialog.Title = "Select a file to open";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        if (string.IsNullOrWhiteSpace(filePath))
                            throw new Exception("Invalid file path.");

                        try
                        {
                            using (StreamReader reader = new StreamReader(filePath))
                            {
                                string fileContent = reader.ReadToEnd();
                                if (!fileContent.IsPrintable())
                                {
                                    MessageBox.Show("The file is not a valid UTF-8 file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                richTextBox1.Text = fileContent;
                                fileNameTxt.Text = Path.GetFileName(filePath);
                                urlTxt.Text = Path.GetFullPath(filePath);
                                wordTxt.Text = fileContent.CountWords().ToString();
                                lineTxt.Text = fileContent.CountLines().ToString();
                                charTxt.Text = fileContent.CountTotalCharsExcludeWhitespace().ToString();
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("You do not have permission to access this file.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (IOException ioEx)
                        {
                            MessageBox.Show($"I/O Error: {ioEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
