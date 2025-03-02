using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THMang1
{
    public partial class Bai4 : Form
    {
        public Bai4()
        {
            InitializeComponent();
        }

        public static string BinaryToDecimal(string binary)
        {
            return Convert.ToInt32(binary, 2).ToString();
        }

        public static string BinaryToHex(string binary)
        {
            return Convert.ToInt32(binary, 2).ToString("X");
        }

        public static string DecimalToBinary(string decimalNumber)
        {
            return Convert.ToString(int.Parse(decimalNumber), 2);
        }

        public static string DecimalToHex(string decimalNumber)
        {
            return int.Parse(decimalNumber).ToString("X");
        }

        public static string HexToBinary(string hex)
        {
            return Convert.ToString(Convert.ToInt32(hex, 16), 2);
        }

        public static string HexToDecimal(string hex)
        {
            return Convert.ToInt32(hex, 16).ToString();
        }
        public static bool IsValidBinary(string binary)
        {
            return Regex.IsMatch(binary, "^[01]+$");
        }

        public static bool IsValidDecimal(string decimalNumber)
        {
            return Regex.IsMatch(decimalNumber, "^[0-9]+$");
        }

        public static bool IsValidHex(string hex)
        {
            return Regex.IsMatch(hex, "^[0-9A-Fa-f]+$");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private bool IsValidBase(string s)
        {
            return String.Equals(s, "Binary", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(s, "Decimal", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(s, "Hexadecimal", StringComparison.OrdinalIgnoreCase);
        }

        private int GetBase(string s)
        {
            if (String.Equals(s, "Binary", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            else if (String.Equals(s, "Decimal", StringComparison.OrdinalIgnoreCase))
            {
                return 10;
            }
            else if (String.Equals(s, "Hexadecimal", StringComparison.OrdinalIgnoreCase))
            {
                return 16;
            }

            throw new Exception("Invalid base");
        }

        private string ConvertToBase(string s, int baseFrom, int baseTo)
        {
            // Convert from source base to decimal
            int decimalValue = Convert.ToInt32(s, baseFrom);

            // Convert from decimal to target base
            return Convert.ToString(decimalValue, baseTo).ToUpper();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fromFormat = comboBoxFrom.SelectedItem?.ToString().ToLower();
            var toFormat = comboBoxTo.SelectedItem?.ToString().ToLower();

            if (fromFormat is null || toFormat is null)
            {
                MessageBox.Show("Hệ cơ số không phù hợp.");
                txtOutput.Text = "";
                return;
            }

            if (!IsValidBase(fromFormat) || !IsValidBase(toFormat))
            {
                MessageBox.Show("Hệ cơ số không phù hợp.");
                txtOutput.Text = "";
                return;
            }
            int fromBase = GetBase(fromFormat);
            int toBase = GetBase(toFormat);

            try
            {
                string result = ConvertToBase(txtInput.Text, fromBase, toBase);
                txtOutput.Text = result;
            }
            catch (Exception)
            {
                MessageBox.Show("Input không hợp lệ");
                txtOutput.Text = "";
                return;
            }

        }
    }
}
