using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CalculatorApp
{
    public partial class Calculator : Form
    {
        private string lastResult = ""; // store last result
        private bool justEvaluated = false; // check if last action was "="

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {

        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "0";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "6";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "9";
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtDisplay.Clear();
            lastResult = "";
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += ".";
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "00";
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            AddOperator("+");
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            AddOperator("-");
        }

        private void btnMultiple_Click(object sender, EventArgs e)
        {
            AddOperator("*");
        }

        private void btnDevide_Click(object sender, EventArgs e)
        {
            AddOperator("/");
        }
        private void AddNumber(string num)
        {
            if (justEvaluated)
            {
                txtDisplay.Clear();
                justEvaluated = false;
            }

            string[] lines = txtDisplay.Text.Split('\n');
            string expression = lines[0]; // always first row is expression

            expression += num;
            txtDisplay.Text = expression;
            EvaluateExpression();
        }

        private void AddOperator(string op)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
                string[] lines = txtDisplay.Text.Split('\n');
                string expression = lines[0];

                char lastChar = expression[expression.Length - 1];
                if (char.IsDigit(lastChar))
                {
                    expression += op;
                }
                else if ("+-*/".Contains(lastChar)) // replace last operator
                {
                    expression = expression.Remove(expression.Length - 1) + op;
                }

                txtDisplay.Text = expression;
            }
        }

        // ---------------- Equal Button ----------------
        private void btnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                string expression = txtDisplay.Text.Split('\n')[0]; // only first row
                var result = new DataTable().Compute(expression, null);

                txtDisplay.Text = result.ToString(); // keep only result
                lastResult = result.ToString();
                justEvaluated = true;
            }
            catch
            {
                MessageBox.Show("Invalid Expression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------- Evaluate Expression ----------------
        private void EvaluateExpression()
        {
            try
            {
                string expression = txtDisplay.Text.Split('\n')[0]; // keep only expression

                if (string.IsNullOrEmpty(expression)) return;

                // Do not calculate if ends with operator
                if (expression.EndsWith("+") || expression.EndsWith("-") ||
                    expression.EndsWith("*") || expression.EndsWith("/"))
                {
                    return;
                }

                // Must contain at least one operator to evaluate
                if (expression.Contains("+") || expression.Contains("-") ||
                    expression.Contains("*") || expression.Contains("/"))
                {
                    var result = new DataTable().Compute(expression, null);

                    txtDisplay.Text = expression + "\r\n" + result.ToString();
                }
            }
            catch
            {
                // ignore errors
            }
        }

        // ---------------- Handle Text Changed ----------------
        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {
            // prevent recursive changes (we handle updates in AddNumber/AddOperator)
        }

        private void btnPercentage_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = txtDisplay.Text.Split('\n');
                string expression = lines[0]; // always work on expression

                if (string.IsNullOrEmpty(expression))
                    return;

                // Find the last number in the expression
                int i = expression.Length - 1;
                while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    i--;
                }

                string lastNumber = expression.Substring(i + 1);
                if (string.IsNullOrEmpty(lastNumber)) return;

                double num = Convert.ToDouble(lastNumber);
                double percentValue = num / 100.0;

                // Replace the last number with its percentage
                expression = expression.Substring(0, i + 1) + percentValue.ToString();

                txtDisplay.Text = expression;
                EvaluateExpression();
            }
            catch
            {
                MessageBox.Show("Invalid percentage operation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
