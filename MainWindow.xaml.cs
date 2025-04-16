using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_1
{
    public partial class MainWindow : Window
    {
        private StringBuilder currentInput = new StringBuilder();
        private StringBuilder expression = new StringBuilder();
        private double? lastResult = null;
        private string lastOperation = null;
        private bool newInput = true;
        private bool errorState = false;

        public MainWindow()
        {
            InitializeComponent();
            ResultTextBlock.Text = "0";
        }

        private void UpdateDisplay()
        {
            if (currentInput.Length > 0)
            {
                ResultTextBlock.Text = currentInput.ToString();
            }
            else
            {
                ResultTextBlock.Text = "0";
            }

            ExpressionTextBlock.Text = expression.ToString();
        }

        private void ClearAll()
        {
            currentInput.Clear();
            expression.Clear();
            lastResult = null;
            lastOperation = null;
            newInput = true;
            errorState = false;
            UpdateDisplay();
        }

        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            var button = (Button)sender;
            string digit = button.Content.ToString();

            if (newInput)
            {
                currentInput.Clear();
                newInput = false;
            }

            currentInput.Append(digit);
            UpdateDisplay();
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            if (newInput)
            {
                currentInput.Clear();
                currentInput.Append("0");
                newInput = false;
            }

            if (!currentInput.ToString().Contains(","))
            {
                currentInput.Append(",");
                UpdateDisplay();
            }
        }

        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState || currentInput.Length == 0) return;

            if (currentInput[0] == '-')
            {
                currentInput.Remove(0, 1);
            }
            else
            {
                currentInput.Insert(0, '-');
            }

            UpdateDisplay();
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            if (currentInput.Length > 0)
            {
                currentInput.Remove(currentInput.Length - 1, 1);
                UpdateDisplay();
            }
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            var button = (Button)sender;
            string operation = button.Content.ToString();

            if (currentInput.Length > 0)
            {
                double currentNumber;
                if (!double.TryParse(currentInput.ToString(), out currentNumber))
                {
                    errorState = true;
                    ResultTextBlock.Text = "Ошибка";
                    return;
                }

                if (lastOperation != null && lastResult != null)
                {
                    CalculateResult();
                }
                else
                {
                    lastResult = currentNumber;
                }

                expression.Append(lastResult.Value.ToString());
                expression.Append(" " + operation + " ");
                lastOperation = operation;
                currentInput.Clear();
                newInput = true;
                UpdateDisplay();
            }
            else if (lastResult != null && lastOperation == null)
            {
                expression.Append(lastResult.Value.ToString());
                expression.Append(" " + operation + " ");
                lastOperation = operation;
                UpdateDisplay();
            }
        }

        private void FunctionButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            var button = (Button)sender;
            string function = button.Content.ToString();

            if (currentInput.Length == 0 && lastResult != null)
            {
                currentInput.Append(lastResult.Value.ToString());
            }

            if (currentInput.Length > 0)
            {
                double inputNumber;
                if (!double.TryParse(currentInput.ToString(), out inputNumber))
                {
                    errorState = true;
                    ResultTextBlock.Text = "Ошибка";
                    return;
                }

                double result = 0;
                bool success = true;

                try
                {
                    switch (function)
                    {
                        case "sin":
                            result = Math.Sin(inputNumber * Math.PI / 180);
                            expression.Append($"sin({inputNumber})");
                            break;
                        case "cos":
                            result = Math.Cos(inputNumber * Math.PI / 180);
                            expression.Append($"cos({inputNumber})");
                            break;
                        case "tg":
                            result = Math.Tan(inputNumber * Math.PI / 180);
                            expression.Append($"tan({inputNumber})");
                            break;
                        case "x²":
                            result = Math.Pow(inputNumber, 2);
                            expression.Append($"({inputNumber})²");
                            break;
                        case "1/x":
                            if (inputNumber == 0) throw new DivideByZeroException();
                            result = 1 / inputNumber;
                            expression.Append($"1/({inputNumber})");
                            break;
                        case "|x|":
                            result = Math.Abs(inputNumber);
                            expression.Append($"|{inputNumber}|");
                            break;
                        case "√x":
                            if (inputNumber < 0) throw new ArgumentException();
                            result = Math.Sqrt(inputNumber);
                            expression.Append($"√({inputNumber})");
                            break;
                        case "n!":
                            if (inputNumber < 0 || inputNumber % 1 != 0 || inputNumber > 20)
                                throw new ArgumentException();
                            result = Factorial((int)inputNumber);
                            expression.Append($"({inputNumber}!)");
                            break;
                        case "xʸ":
                            lastResult = inputNumber;
                            lastOperation = "^";
                            expression.Append($"({inputNumber})^");
                            newInput = true;
                            currentInput.Clear();
                            UpdateDisplay();
                            return;
                        case "10ˣ":
                            result = Math.Pow(10, inputNumber);
                            expression.Append($"10^({inputNumber})");
                            break;
                        case "log":
                            if (inputNumber <= 0) throw new ArgumentException();
                            result = Math.Log10(inputNumber);
                            expression.Append($"log({inputNumber})");
                            break;
                        case "ln":
                            if (inputNumber <= 0) throw new ArgumentException();
                            result = Math.Log(inputNumber);
                            expression.Append($"ln({inputNumber})");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    errorState = true;
                    ResultTextBlock.Text = "Ошибка";
                    return;
                }

                currentInput.Clear();
                currentInput.Append(result.ToString());
                lastResult = result;
                newInput = true;
                UpdateDisplay();
            }
        }

        private void ConstantButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            var button = (Button)sender;
            string constant = button.Content.ToString();

            if (newInput)
            {
                currentInput.Clear();
                newInput = false;
            }

            switch (constant)
            {
                case "π":
                    currentInput.Append(Math.PI.ToString());
                    expression.Append("π");
                    break;
                case "e":
                    currentInput.Append(Math.E.ToString());
                    expression.Append("e");
                    break;
            }

            UpdateDisplay();
        }

        private void ParenthesisButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            var button = (Button)sender;
            string parenthesis = button.Content.ToString();

            expression.Append(parenthesis);
            UpdateDisplay();
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (errorState) return;

            if (lastOperation != null)
            {
                CalculateResult();
            }
        }

        private void CalculateResult()
        {
            if (currentInput.Length > 0)
            {
                double currentNumber;
                if (!double.TryParse(currentInput.ToString(), out currentNumber))
                {
                    errorState = true;
                    ResultTextBlock.Text = "Ошибка";
                    return;
                }

                try
                {
                    switch (lastOperation)
                    {
                        case "+":
                            lastResult += currentNumber;
                            break;
                        case "-":
                            lastResult -= currentNumber;
                            break;
                        case "×":
                            lastResult *= currentNumber;
                            break;
                        case "÷":
                            if (currentNumber == 0) throw new DivideByZeroException();
                            lastResult /= currentNumber;
                            break;
                        case "^":
                            lastResult = Math.Pow(lastResult.Value, currentNumber);
                            break;
                    }

                    expression.Append(currentNumber.ToString() + " = ");
                    currentInput.Clear();
                    currentInput.Append(lastResult.Value.ToString());
                    lastOperation = null;
                    newInput = true;
                    UpdateDisplay();
                }
                catch (Exception ex)
                {
                    errorState = true;
                    ResultTextBlock.Text = "Ошибка";
                }
            }
        }

        private double Factorial(int n)
        {
            if (n == 0) return 1;
            double result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}