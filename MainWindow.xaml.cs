using System;
using System.Windows;
using System.Windows.Controls;

namespace practos4
{
    public partial class MainWindow : Window
    {
        private double firstNumber;
        private double secondNumber;
        private string currentOperation;
        private bool isOperationClicked;

        public MainWindow()
        {
            InitializeComponent();
            isOperationClicked = false;
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (Display.Text == "0" || isOperationClicked ||
                Display.Text == "Деление на 0!" ||
                Display.Text == "Ошибка!")
            {
                Display.Text = button.Content.ToString();
                isOperationClicked = false;
            }
            else
            {
                Display.Text += button.Content.ToString();
            }
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            firstNumber = double.Parse(Display.Text);
            currentOperation = button.Content.ToString();
            isOperationClicked = true;
        }

        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                secondNumber = double.Parse(Display.Text);
                double result = 0;

                switch (currentOperation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "×":
                        result = firstNumber * secondNumber;
                        break;
                    case "÷":
                        if (secondNumber == 0)
                        {
                            Display.Text = "Деление на 0!";
                            isOperationClicked = true;
                            return;
                        }
                        result = firstNumber / secondNumber;
                        break;
                }

                Display.Text = result.ToString();
                firstNumber = result;
                isOperationClicked = true;
            }
            catch (Exception ex)
            {
                Display.Text = "Ошибка!";
                isOperationClicked = true;
            }
        }
    }
}