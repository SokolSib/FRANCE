using System.Windows;
using System.Windows.Controls;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;
using TicketWindow.Winows.OtherWindows.Payment;

namespace TicketWindow.Winows.OtherWindows.Keyboard
{
    /// <summary>
    /// Логика взаимодействия для W_NumPad.xaml
    /// </summary>
    public partial class WNumPadMini : UserControl
    {
        public WNumPadMini()
        {
            InitializeComponent();
            Clr = true;
        }
        
        public TextBox TextBox { get; set; }
        public Button BEnter { get; set; }

        public bool Clr { get; set; }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.ToolTip != null && button.ToolTip.ToString() == "WNumPadMini")
            {
                string getValue = button.Name.Remove(0, 1);

                switch (getValue)
                {
                    case "Sup":
                        TextBox.Text = "";
                        break;
                    case "Entree":
                        var window = Tag as WPayEtc;
                        if (window != null)
                        {
                            decimal money;
                            if (!string.IsNullOrEmpty(window.tbS.Text) &&
                                decimal.TryParse(window.tbS.Text.Trim(), out money) &&
                                FunctionsService.PayWithValidation(sender, money, window.MaxMoney, window.PayType))
                                FunctionsService.Click(BEnter);
                        }
                        else
                            FunctionsService.Click(BEnter);
                        break;
                    case "Point":
                        TextBox.Text += ",";
                        break;
                    default:
                        if (Clr)
                            TextBox.Text = "";
                        int f;
                        if (int.TryParse(getValue, out f))
                            TextBox.Text += getValue;
                        break;
                }

                Clr = false;
            }
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;

        }
    }
}
