using System.Windows;
using System.Windows.Controls;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Setting
{
    /// <summary>
    ///     Логика взаимодействия для W_Close_TicketWindow.xaml
    /// </summary>
    public partial class WCloseTicketWindow : Window
    {
        public WCloseTicketWindow(string str)
        {
            InitializeComponent();
            errorlist.Text = TextService.GetCloseWindowText() + str;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
            {
                bs.Click += ButtonClick;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}