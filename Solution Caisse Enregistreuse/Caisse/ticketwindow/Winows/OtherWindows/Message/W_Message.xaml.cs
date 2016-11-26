using System.Windows;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Message
{
    /// <summary>
    /// Логика взаимодействия для W_Message.xaml
    /// </summary>
    public partial class WMessage : Window
    {
        public WMessage(string okToolTipText, string mes, string okText)
        {
            InitializeComponent();

            if (okToolTipText != null)
                ButtonOk.ToolTip = okToolTipText;
            else
                ButtonOk.Visibility = Visibility.Hidden;

            Message.Text = mes;
            if (!string.IsNullOrEmpty(okText)) ButtonOk.Content = okText;
        }

        private void BokClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            FunctionsService.Click(sender);
        }

        private void BcanClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
