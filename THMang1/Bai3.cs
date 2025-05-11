using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Whut;


namespace THMang1
{


    public partial class Bai3 : Form
    {
        private BindingList<FileAttachment> _files = new BindingList<FileAttachment>();

        public Bai3()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (_files.Count > 0 && e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                _files.RemoveAt(e.RowIndex);
            }
        }


        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void ResetForm()
        {
            bodyTxt.Text = "";
            subjectTxt.Text = "";
            _files.Clear();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string from = fromTxt.Text;
            string to = toTxt.Text;
            string password = passwordTxt.Text;

            string subject = subjectTxt.Text;
            string body = bodyTxt.Text;

            if (!IsValidEmail(from))
            {
                MessageBox.Show($"Invalid from address {from}");
                return;
            }

            if (!IsValidEmail(to))
            {
                MessageBox.Show($"Invalid to address {to}");
                return;
            }

            try
            {
                button1.Enabled = false;
                await EmailSender.SendGmailEmailAsync(from, password, to, subject, body,
                    attachmentPaths: _files.Select(r => r.FilePath).ToList());
                MessageBox.Show("Sent successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send mail: {ex.Message}");
                return;
            }
            finally
            {
                button1.Enabled = true;
            }
            ResetForm();
        }


        private string SelectFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a file";
                openFileDialog.Filter = "All files (*.*)|*.*"; // Change filter as needed
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }

            return null;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string newFile = SelectFilePath();
            if (newFile != null)
            {
                _files.Add(new FileAttachment { FilePath = newFile });
            }

        }

        private void bodyTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void Bai3_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;

            var fileColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FilePath",
                HeaderText = "File",
                Name = "FilePath"
            };
            var deleteButton = new DataGridViewButtonColumn
            {
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Name = "Delete"
            };
            dataGridView1.Columns.Add(fileColumn);
            dataGridView1.Columns.Add(deleteButton);
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView1.DataSource = _files;

        }
    }

    public class FileAttachment
    {
        public string FilePath { get; set; }
    }
}
