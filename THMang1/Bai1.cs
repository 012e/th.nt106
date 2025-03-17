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
                                CurrentContent = fileContent;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentContent == null)
            {
                MessageBox.Show("No file selected yet, exiting!");
                return;
            }
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
                    saveFileDialog.Title = "Save a file";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        if (string.IsNullOrWhiteSpace(filePath))
                            throw new Exception("Invalid file path.");

                        try
                        {
                            using (StreamWriter writer = new StreamWriter(filePath))
                            {
                                writer.Write(CurrentContent.ToUpper());
                            }
                            MessageBox.Show("File successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("You do not have permission to write to this file.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
