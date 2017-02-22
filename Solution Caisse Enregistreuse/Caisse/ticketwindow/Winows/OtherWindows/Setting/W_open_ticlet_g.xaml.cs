using System.Windows;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Setting
{
    /// <summary>
    /// Логика взаимодействия для W_open_ticlet_g.xaml
    /// </summary>
    public partial class WOpenTicletG : Window
    {
        public WOpenTicletG(string mes)
        {
            InitializeComponent();
            status.Content += mes;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!CassieService.Open())
                status.Content += RepositoryGeneral.Mess;
            Close();
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
