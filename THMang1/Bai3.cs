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
            InitializeCustomComponent();
        }

        private string inputFilePath;
        private string outputFilePath;

        private void InitializeCustomComponent()
        {
            this.btnSelectInputFile = new System.Windows.Forms.Button();
            this.btnSelectOutputFile = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectInputFile
            // 
            this.btnSelectInputFile.Location = new System.Drawing.Point(12, 12);
            this.btnSelectInputFile.Name = "btnSelectInputFile";
            this.btnSelectInputFile.Size = new System.Drawing.Size(150, 30);
            this.btnSelectInputFile.TabIndex = 0;
            this.btnSelectInputFile.Text = "Select Input File";
            this.btnSelectInputFile.UseVisualStyleBackColor = true;
            this.btnSelectInputFile.Click += new System.EventHandler(this.btnSelectInputFile_Click);
            // 
            // btnSelectOutputFile
            // 
            this.btnSelectOutputFile.Location = new System.Drawing.Point(12, 60);
            this.btnSelectOutputFile.Name = "btnSelectOutputFile";
            this.btnSelectOutputFile.Size = new System.Drawing.Size(150, 30);
            this.btnSelectOutputFile.TabIndex = 1;
            this.btnSelectOutputFile.Text = "Select Output File";
            this.btnSelectOutputFile.UseVisualStyleBackColor = true;
            this.btnSelectOutputFile.Click += new System.EventHandler(this.btnSelectOutputFile_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(12, 108);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(150, 30);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Enabled = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Location = new System.Drawing.Point(168, 21);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(95, 13);
            this.lblInputFile.TabIndex = 3;
            this.lblInputFile.Text = "No file selected";
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(168, 69);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(95, 13);
            this.lblOutputFile.TabIndex = 4;
            this.lblOutputFile.Text = "No file selected";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(12, 156);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(476, 182);
            this.txtStatus.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblOutputFile);
            this.Controls.Add(this.lblInputFile);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnSelectOutputFile);
            this.Controls.Add(this.btnSelectInputFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Math Expression Calculator";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnSelectInputFile;
        private System.Windows.Forms.Button btnSelectOutputFile;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.TextBox txtStatus;

        private void btnSelectInputFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        inputFilePath = openFileDialog.FileName;
                        lblInputFile.Text = Path.GetFileName(inputFilePath);
                        LogStatus($"Input file selected: {inputFilePath}");
                        UpdateProcessButtonState();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting input file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogStatus($"Error: {ex.Message}");
            }
        }

        private void btnSelectOutputFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.OverwritePrompt = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        outputFilePath = saveFileDialog.FileName;
                        lblOutputFile.Text = Path.GetFileName(outputFilePath);
                        LogStatus($"Output file selected: {outputFilePath}");
                        UpdateProcessButtonState();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting output file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogStatus($"Error: {ex.Message}");
            }
        }

        private void UpdateProcessButtonState()
        {
            btnProcess.Enabled = !string.IsNullOrEmpty(inputFilePath) && !string.IsNullOrEmpty(outputFilePath);
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputFilePath))
                {
                    throw new InvalidOperationException("Input file not selected");
                }

                if (string.IsNullOrEmpty(outputFilePath))
                {
                    throw new InvalidOperationException("Output file not selected");
                }

                // Check if input file exists
                if (!File.Exists(inputFilePath))
                {
                    throw new FileNotFoundException("Input file does not exist", inputFilePath);
                }

                // Try to open the output file to make sure we can write to it
                using (FileStream fs = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Just testing access
                }

                ProcessFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogStatus($"Error: {ex.Message}");
            }
        }

        private void ProcessFile()
        {
            LogStatus("Processing started...");
            int lineCount = 0;
            int successCount = 0;
            int errorCount = 0;

            try
            {
                // Read all lines from input file
                string[] inputLines = File.ReadAllLines(inputFilePath);

                if (inputLines.Length == 0)
                {
                    LogStatus("Warning: Input file is empty");
                }

                // Create a list to store the results
                System.Collections.Generic.List<string> outputLines = new System.Collections.Generic.List<string>();

                // Process each line
                foreach (string line in inputLines)
                {
                    lineCount++;
                    string trimmedLine = line.Trim();

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(trimmedLine))
                    {
                        LogStatus($"Line {lineCount}: Skipping empty line");
                        outputLines.Add(string.Empty); // Preserve the empty line in output
                        continue;
                    }

                    try
                    {
                        string result = ProcessExpression(trimmedLine, lineCount);
                        outputLines.Add(result);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        string errorMessage = $"{trimmedLine} = ERROR: {ex.Message}";
                        outputLines.Add(errorMessage);
                        LogStatus($"Line {lineCount}: {errorMessage}");
                    }
                }

                // Write to output file
                File.WriteAllLines(outputFilePath, outputLines);
                LogStatus($"Processing completed. Success: {successCount}, Errors: {errorCount}, Total: {lineCount}");
                MessageBox.Show($"Processing completed successfully!\nExpressions processed: {successCount}\nErrors: {errorCount}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogStatus($"Critical error during processing: {ex.Message}");
                throw;
            }
        }

        private string ProcessExpression(string expression, int lineNumber)
        {
            // Define regex pattern to match "number operator number"
            string pattern = @"^\s*([+-]?\d+(\.\d+)?)\s*([+\-*/])\s*([+-]?\d+(\.\d+)?)\s*$";
            Match match = Regex.Match(expression, pattern);

            if (!match.Success)
            {
                throw new FormatException("Invalid expression format. Expected: number operator number");
            }

            // Extract operands and operator
            decimal leftOperand, rightOperand;
            try
            {
                leftOperand = decimal.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new FormatException($"Invalid left operand: {match.Groups[1].Value}");
            }

            try
            {
                rightOperand = decimal.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new FormatException($"Invalid right operand: {match.Groups[4].Value}");
            }

            string op = match.Groups[3].Value;
            decimal result;

            // Perform calculation
            switch (op)
            {
                case "+":
                    result = leftOperand + rightOperand;
                    break;
                case "-":
                    result = leftOperand - rightOperand;
                    break;
                case "*":
                    result = leftOperand * rightOperand;
                    break;
                case "/":
                    if (rightOperand == 0)
                    {
                        throw new DivideByZeroException("Division by zero is not allowed");
                    }
                    result = leftOperand / rightOperand;
                    break;
                default:
                    throw new ArgumentException($"Unsupported operator: {op}");
            }

            // Format the result
            return $"{expression} = {result}";
        }

        private void LogStatus(string message)
        {
            txtStatus.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            txtStatus.ScrollToCaret();
            Application.DoEvents(); // Refresh UI
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleUnexpectedException(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleUnexpectedException(e.ExceptionObject as Exception);
        }

        private static void HandleUnexpectedException(Exception ex)
        {
            string errorMessage = "An unexpected error occurred:\n\n";
            if (ex != null)
            {
                errorMessage += ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;
            }
            else
            {
                errorMessage += "Unknown error";
            }

            try
            {
                // Log to file
                string logPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "CalculatorApp",
                    "error.log");

                // Create directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

                File.AppendAllText(
                    logPath,
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {errorMessage}{Environment.NewLine}{Environment.NewLine}");
            }
            catch
            {
                // Cannot log to file, just show the message
            }

            MessageBox.Show(errorMessage, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
