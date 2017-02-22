using System.Windows;

namespace TicketWindow.Winows.OtherWindows.Discount
{
    /// <summary>
    /// Логика взаимодействия для W_discount.xaml
    /// </summary>
    public partial class WDiscount : Window
    {
        public WDiscount()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
