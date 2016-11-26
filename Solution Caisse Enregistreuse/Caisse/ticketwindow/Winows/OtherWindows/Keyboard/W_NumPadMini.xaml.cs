using System.Windows;
using System.Windows.Controls;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

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
        }
        
        public TextBox TextBox { get; set; }
        public Button BEnter { get; set; }
        public bool Clr = true;

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var b = button;

                if (b.ToolTip != null)
                {
                    if (b.ToolTip.ToString() == "WNumPadMini")
                    {
                        string getValue = b.Name.Remove(0, 1);
                      
                        switch (getValue)
                        {
                            case "Sup": TextBox.Text = ""; break;
                            case "Entree": FunctionsService.Click(BEnter); break;
                            case "Point": TextBox.Text += ","; break;
                            default:
                                if (Clr)
                                    TextBox.Text = "";
                                int f;
                                if (int.TryParse(getValue, out f))
                                    TextBox.Text += getValue; break;
                        }

                        Clr = false;
                    }
                }
            }
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;

        }
    }
}
