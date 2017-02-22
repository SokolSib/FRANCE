using System.Windows;
using System.Windows.Controls;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Keyboard
{
    /// <summary>
    /// Логика взаимодействия для W_NumPadComplet.xaml
    /// </summary>
    public partial class WNumPadComplet : UserControl
    {
        public WNumPadComplet()
        {
            InitializeComponent();
        }
        
        public TextBox Box { get; set; }
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
                    if (b.ToolTip.ToString() == "WNumPadComplet")
                    {
                        string getValue = b.Name.Remove(0, 1);
                        if (Clr)
                            Box.Text = "";
                        switch (getValue)
                        {
                            case "Sup": Box.Text = ""; break;
                            case "Entree": FunctionsService.Click(BEnter); break;
                            case "Point": Box.Text += ","; break;
                                
                            default:
                                int f;
                                if (int.TryParse(getValue, out f))
                                    Box.Text += getValue; break;
                        }

                        Clr = false;
                    }
                }
            }
        }

        private void UserControlLoaded1(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;

        }
    }
}
