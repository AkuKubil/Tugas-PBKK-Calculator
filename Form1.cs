using System.Globalization;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        double firstNumber = 0;
        double secondNumber = 0;
        double result = 0;
        string operation = "";

        // true setelah tombol "=" ditekan, supaya angka berikutnya memulai input baru
        bool startNewNumber = false;

        public Form1()
        {
            InitializeComponent();
        }

        // Dipakai bersama oleh tombol 0-9
        private void NumberButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            if (txtDisplay.Text == "0" || startNewNumber)
            {
                txtDisplay.Text = button.Text;
                startNewNumber = false;
            }
            else
            {
                txtDisplay.Text += button.Text;
            }
        }

        // Dipakai bersama oleh tombol + - x /
        private void OperatorButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;

            // Display kosong (operator ditekan dua kali): cukup ganti operatornya
            if (string.IsNullOrEmpty(txtDisplay.Text))
            {
                if (operation != "") operation = button.Text;
                return;
            }

            if (!double.TryParse(txtDisplay.Text, NumberStyles.Float,
                                 CultureInfo.InvariantCulture, out double value))
                return;

            firstNumber = value;
            operation = button.Text;
            startNewNumber = false;

            txtDisplay.Clear();
        }

        private void btnEquals_Click(object? sender, EventArgs e)
        {
            // Belum ada operator yang dipilih, tidak ada yang dihitung
            if (operation == "") return;

            try
            {
                if (!double.TryParse(txtDisplay.Text, NumberStyles.Float,
                                     CultureInfo.InvariantCulture, out secondNumber))
                    throw new FormatException("Masukkan angka kedua terlebih dahulu.");

                switch (operation)
                {
                    case "+": result = firstNumber + secondNumber; break;
                    case "−": result = firstNumber - secondNumber; break;
                    case "×": result = firstNumber * secondNumber; break;
                    case "÷":
                        if (secondNumber == 0)
                            throw new DivideByZeroException("Tidak bisa membagi dengan nol.");
                        result = firstNumber / secondNumber;
                        break;
                }

                txtDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
                operation = "";
                startNewNumber = true;
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetCalculator();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            ResetCalculator();
        }

        private void btnDecimal_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDisplay.Text) || startNewNumber)
            {
                txtDisplay.Text = "0.";
                startNewNumber = false;
            }
            else if (!txtDisplay.Text.Contains('.'))
            {
                txtDisplay.Text += ".";
            }
        }

        private void ResetCalculator()
        {
            firstNumber = 0;
            secondNumber = 0;
            result = 0;
            operation = "";
            startNewNumber = false;
            txtDisplay.Text = "0";
        }
    }
}
