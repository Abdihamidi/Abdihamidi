using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Test
{
    public partial class Calculator : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _result;
        public string Result
        {
            get => _result;
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged(nameof(Result));
                }
            }
        }

        private string leftOperand;
        private string rightOperand;
        private string currentOp;
        private string digits = "0987654321";
        private string listOfOps = "+-*÷";

        public Calculator()
        {
            InitializeComponent();
            Result = "0";
            BindingContext = this;
            leftOperand = rightOperand = currentOp = null;
        }

        protected void Calculate(object sender, EventArgs e)
        {
            var btnPressed = (Button)sender;
            string cvalue = btnPressed.Text;

            if (cvalue == "%")
            {
                Result = (double.Parse(Result) / 100).ToString();
                StoreIt();
                return;
            }

            if (digits.Contains(cvalue))
            {
                if (Result == "0" && cvalue != "0")
                {
                    Result = cvalue;
                    StoreIt();
                    return;
                }
                else
                {
                    if (Result == "0" && cvalue == "0")
                    {
                        return;
                    }
                    else
                    {
                        Result += cvalue;
                        StoreIt();
                    }
                }
            }

            if (cvalue == ".")
            {
                if (!Result.Contains("."))
                {
                    Result += ".";
                    StoreIt();
                    return;
                }
                else return;
            }

            if (cvalue == "+/-")
            {
                if (!string.IsNullOrEmpty(Result) && Result != "0")
                {
                    if (Result.StartsWith("-"))
                        Result = Result.Substring(1);
                    else
                        Result = "-" + Result;
                    StoreIt();
                }
                return;
            }

            if (cvalue == "AC")
            {
                Result = "0";
                leftOperand = rightOperand = currentOp = null;
                return;
            }

            if (listOfOps.Contains(cvalue))
            {
                if (!string.IsNullOrEmpty(Result))
                {
                    leftOperand = Result;
                    currentOp = cvalue;
                    Result = "0";
                }
                return;
            }

            if (cvalue == "=")
            {
                if (!string.IsNullOrEmpty(leftOperand) && !string.IsNullOrEmpty(currentOp))
                {
                    double left = double.Parse(leftOperand);
                    double right = string.IsNullOrEmpty(Result) ? double.Parse(leftOperand) : double.Parse(Result);
                    double result = 0;

                    switch (currentOp)
                    {
                        case "+":
                            result = left + right;
                            break;
                        case "-":
                            result = left - right;
                            break;
                        case "*":
                            result = left * right;
                            break;
                        case "÷":
                            if (right != 0)
                                result = left / right;
                            else
                            {
                                // Handle division by zero error
                                // For example, you can display an error message
                                Result = "Error: Division by zero";
                                return;
                            }
                            break;
                    }

                    Result = result.ToString();
                    leftOperand = result.ToString();
                    rightOperand = currentOp = null;
                }
                return;
            }
        }

        private void StoreIt()
        {
            if (currentOp == null)
            {
                leftOperand = Result;
            }
            else
            {
                rightOperand = Result;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}