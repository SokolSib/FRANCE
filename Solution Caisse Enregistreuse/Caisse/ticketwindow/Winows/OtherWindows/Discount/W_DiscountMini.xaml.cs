using System.Windows;
using System.Windows.Controls;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Discount
{
    /// <summary>
    /// Логика взаимодействия для W_DiscountMini.xaml
    /// </summary>
    public partial class WDiscountMini : Window
    {
        public WDiscountMini()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
