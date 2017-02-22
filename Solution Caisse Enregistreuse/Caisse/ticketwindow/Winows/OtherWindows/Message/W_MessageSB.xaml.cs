using System.Windows;
using System.Windows.Input;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Message
{
    /// <summary>
    /// Логика взаимодействия для W_MessageSB.xaml
    /// </summary>
    /// 
    public partial class WMessageSb : Window
    {
        public WMessageSb(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }
        
        private void BcanClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                ClassEtcFun.WmSound(@"Data\Computer_Error.wav");
        }
    }
}
