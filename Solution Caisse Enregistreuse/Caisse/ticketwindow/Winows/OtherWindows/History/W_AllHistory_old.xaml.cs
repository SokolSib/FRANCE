using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.History
{
    /// <summary>
    /// Логика взаимодействия для W_AllHistory.xaml
    /// </summary>
    public partial class WAllHistoryOld : Window
    {
        public WAllHistoryOld()
        {
            InitializeComponent();

            RepositoryCloseTicketG.Sync();
            list.DataContext = RepositoryCloseTicketTmp.CloseTicketTmps.Where(l => l.EstablishmentCustomerId == Global.Config.IdEstablishment);
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = list.SelectedItem as CloseTicketTmp;
            listDetails.DataContext = RepositoryCheckTicketTmp.GetByCloseTicketId(s.CustomerId);
            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void list_SelectedCellsChanged_(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = listDetails.SelectedItem as CheckTicketTmp;

            if (s != null)
            {
                var el = RepositoryPayProductTmp.GetByCheckTicketId(s.CustomerId);
                listDetailsProducts.DataContext = el;
                CollectionViewSource.GetDefaultView(listDetailsProducts.ItemsSource).Refresh();
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var check = listDetails.SelectedItem as CheckTicket;

            if (check != null)
                new Class.ClassPrintCheck(new XDocument(CheckTicket.ToCheckXElement(check)), true);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
