using System.Windows;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Setting
{
    /// <summary>
    /// Логика взаимодействия для W_open_ticket.xaml
    /// </summary>
    public partial class WOpenTicket : Window
    {
        public WOpenTicket(string mes)
        {
            InitializeComponent();
            status.Content += mes;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            RepositoryOpenTicketWindow.Open();
            RepositoryHistoryChangeProduct.ToArh();
            Close();
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
            Close();
        }
    }
}
