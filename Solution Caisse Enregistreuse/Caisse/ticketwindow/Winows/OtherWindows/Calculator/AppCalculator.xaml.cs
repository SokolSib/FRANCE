using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketWindow.Winows.AdditionalClasses;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Winows.OtherWindows.Calculator
{
    /// <summary>
    ///     Interaction logic for AppCalculator.xaml
    /// </summary>
    public partial class AppCalculator : Window
    {
        private static CalculatorBox _displayBox;
        private static CalculatorBox _paperBox;
        private static PaperTrail _paper;
        private string _lastVal;
        private string _memVal;
        private Operation _lastOper;

        public AppCalculator()
        {
            InitializeComponent();
            //sub-class our textBox
            _displayBox = new CalculatorBox();
            Grid.SetRow(_displayBox, 0);
            Grid.SetColumn(_displayBox, 0);
            Grid.SetColumnSpan(_displayBox, 9);
            _displayBox.Height = 30;
            MyGrid.Children.Add(_displayBox);

            //sub-class our paper trail textBox
            _paperBox = new CalculatorBox();
            Grid.SetRow(_paperBox, 1);
            Grid.SetColumn(_paperBox, 0);
            Grid.SetColumnSpan(_paperBox, 3);
            Grid.SetRowSpan(_paperBox, 5);
            _paperBox.IsReadOnly = true;
            _paperBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            _paperBox.Margin = new Thickness(3.0, 1.0, 1.0, 1.0);
            _paperBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            _paper = new PaperTrail();

            MyGrid.Children.Add(_paperBox);
            ProcessKey('0');
            EraseDisplay = true;
        }

        //flag to erase or just add to current display flag
        private bool EraseDisplay { get; set; }

        //Get/Set Memory cell value
        private double Memory
        {
            get
            {
                if (_memVal == string.Empty)
                    return 0.0;
                return Convert.ToDouble(_memVal);
            }
            set { _memVal = value.ToString(); }
        }

        //Lats value entered
        private string LastValue
        {
            get
            {
                if (_lastVal == string.Empty)
                    return "0";
                return _lastVal;
            }
            set { _lastVal = value; }
        }

        //The current Calculator display
        private string Display { get; set; }

        // Sample event handler:  
        private void OnWindowKeyDown(object sender, TextCompositionEventArgs /*System.Windows.Input.KeyEventArgs*/ e)
        {
            var s = e.Text;
            var c = (s.ToCharArray())[0];
            e.Handled = true;

            if ((c >= '0' && c <= '9') || c == ',' || c == '\b') // '\b' is backspace
            {
                ProcessKey(c);
                return;
            }
            switch (c)
            {
                case '+':
                    ProcessOperation("BPlus");
                    break;
                case '-':
                    ProcessOperation("BMinus");
                    break;
                case '*':
                    ProcessOperation("BMultiply");
                    break;
                case '/':
                    ProcessOperation("BDevide");
                    break;
                case '%':
                    ProcessOperation("BPercent");
                    break;
                case '=':
                    ProcessOperation("BEqual");
                    break;
            }
        }

        private void DigitBtnClick(object sender, RoutedEventArgs e)
        {
            var s = ((Button) sender).Content.ToString();

            var ids = s.ToCharArray();
            ProcessKey(ids[0]);
        }

        private void ProcessKey(char c)
        {
            if (EraseDisplay)
            {
                Display = string.Empty;
                EraseDisplay = false;
            }
            AddToDisplay(c);
        }

        private void ProcessOperation(string s)
        {
            switch (s)
            {
                case "BPM":
                    _lastOper = Operation.Negate;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    EraseDisplay = true;
                    _lastOper = Operation.None;
                    break;
                case "BDevide":

                    if (EraseDisplay) //stil wait for a digit...
                    {
                        //stil wait for a digit...
                        _lastOper = Operation.Devide;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Devide;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BMultiply":
                    if (EraseDisplay) //stil wait for a digit...
                    {
                        //stil wait for a digit...
                        _lastOper = Operation.Multiply;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Multiply;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BMinus":
                    if (EraseDisplay) //stil wait for a digit...
                    {
                        //stil wait for a digit...
                        _lastOper = Operation.Subtract;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Subtract;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BPlus":
                    if (EraseDisplay)
                    {
                        //stil wait for a digit...
                        _lastOper = Operation.Add;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Add;
                    LastValue = Display;
                    EraseDisplay = true;
                    break;
                case "BEqual":
                    if (EraseDisplay) //stil wait for a digit...
                        break;
                    CalcResults();
                    EraseDisplay = true;
                    _lastOper = Operation.None;
                    LastValue = Display;
                    //val = Display;
                    break;
                case "BSqrt":
                    _lastOper = Operation.Sqrt;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    EraseDisplay = true;
                    _lastOper = Operation.None;
                    break;
                case "BPercent":
                    if (EraseDisplay) //stil wait for a digit...
                    {
                        //stil wait for a digit...
                        _lastOper = Operation.Percent;
                        break;
                    }
                    CalcResults();
                    _lastOper = Operation.Percent;
                    LastValue = Display;
                    EraseDisplay = true;
                    //LastOper = Operation.None;
                    break;
                case "BOneOver":
                    _lastOper = Operation.OneX;
                    LastValue = Display;
                    CalcResults();
                    LastValue = Display;
                    EraseDisplay = true;
                    _lastOper = Operation.None;
                    break;
                case "BC": //clear All
                    _lastOper = Operation.None;
                    Display = LastValue = string.Empty;
                    _paper.Clear();
                    UpdateDisplay();
                    break;
                case "BCE": //clear entry
                    _lastOper = Operation.None;
                    Display = LastValue;
                    UpdateDisplay();
                    break;
                case "BMemClear":
                    Memory = 0.0F;
                    DisplayMemory();
                    break;
                case "BMemSave":
                    Memory = Convert.ToDouble(Display);
                    DisplayMemory();
                    EraseDisplay = true;
                    break;
                case "BMemRecall":
                    Display = /*val =*/ Memory.ToString();
                    UpdateDisplay();
                    EraseDisplay = false;
                    break;
                case "BMemPlus":
                    var d = Memory + Convert.ToDouble(Display);
                    Memory = d;
                    DisplayMemory();
                    EraseDisplay = true;
                    break;
            }
        }

        private void OperBtnClick(object sender, RoutedEventArgs e)
        {
            ProcessOperation(((Button) sender).Name);
        }

        private double Calc(Operation lastOper)
        {
            var d = 0.0;

            try
            {
                switch (lastOper)
                {
                    case Operation.Devide:
                        _paper.AddArguments(LastValue + " / " + Display);
                        d = (Convert.ToDouble(LastValue)/Convert.ToDouble(Display));
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.Add:
                        _paper.AddArguments(LastValue + " + " + Display);
                        d = Convert.ToDouble(LastValue) + Convert.ToDouble(Display);
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.Multiply:
                        _paper.AddArguments(LastValue + " * " + Display);
                        d = Convert.ToDouble(LastValue)*Convert.ToDouble(Display);
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.Percent:
                        //Note: this is different (but make more sense) then Windows calculator
                        _paper.AddArguments(LastValue + " % " + Display);
                        d = (Convert.ToDouble(LastValue)*Convert.ToDouble(Display))/100.0F;
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.Subtract:
                        _paper.AddArguments(LastValue + " - " + Display);
                        d = Convert.ToDouble(LastValue) - Convert.ToDouble(Display);
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.Sqrt:
                        _paper.AddArguments("Sqrt( " + LastValue + " )");
                        d = Math.Sqrt(Convert.ToDouble(LastValue));
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.OneX:
                        _paper.AddArguments("1 / " + LastValue);
                        d = 1.0F/Convert.ToDouble(LastValue);
                        CheckResult(d);
                        _paper.AddResult(d.ToString());
                        break;
                    case Operation.Negate:
                        d = Convert.ToDouble(LastValue)*(-1.0F);
                        break;
                }
            }
            catch
            {
                d = 0;
                var parent = (Window) MyPanel.Parent;
                _paper.AddResult("Error");
                MessageBox.Show(parent, "Operation cannot be perfomed", parent.Title);
            }

            return d;
        }

        private static void CheckResult(double d)
        {
            if (double.IsNegativeInfinity(d) || double.IsPositiveInfinity(d) || double.IsNaN(d))
                throw new System.Exception("Illegal value");
        }

        private void DisplayMemory()
        {
            if (_memVal != string.Empty)
                BMemBox.Text = "Memory: " + _memVal;
            else
                BMemBox.Text = "Memory: [empty]";
        }

        private void CalcResults()
        {
            if (_lastOper == Operation.None)
                return;

            var d = Calc(_lastOper);
            Display = d.ToString();

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            _displayBox.Text = Display == string.Empty ? "0" : Display;
        }

        private void AddToDisplay(char c)
        {
            if (c == ',')
            {
                if (Display.IndexOf(',', 0) >= 0) //already exists
                    return;
                Display = Display + c;
            }
            else
            {
                if (c >= '0' && c <= '9')
                {
                    Display = Display + c;
                }
                else if (c == '\b') //backspace ?
                {
                    if (Display.Length <= 1)
                        Display = string.Empty;
                    else
                    {
                        var i = Display.Length;
                        Display = Display.Remove(i - 1, 1); //remove last char 
                    }
                }
            }

            UpdateDisplay();
        }

        private void OnMenuExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private enum Operation
        {
            None,
            Devide,
            Multiply,
            Subtract,
            Add,
            Percent,
            Sqrt,
            OneX,
            Negate
        }

        private class PaperTrail
        {
            private string _args;

            public void AddArguments(string a)
            {
                _args = a;
            }

            public void AddResult(string r)
            {
                _paperBox.Text += _args + " = " + r + "\n";
            }

            public void Clear()
            {
                _paperBox.Text = string.Empty;
                _args = string.Empty;
            }
        }
    }
}