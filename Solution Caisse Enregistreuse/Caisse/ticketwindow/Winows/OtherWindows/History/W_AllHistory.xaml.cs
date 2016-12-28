using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using TicketWindow.Class;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.History
{
    /// <summary>
    ///     Логика взаимодействия для W_AllHistory.xaml
    /// </summary>
    public partial class WAllHistory : Window
    {
        public WAllHistory()
        {
            InitializeComponent();
            TableCloseTicketGs.DataContext = RepositoryCloseTicketG.Get(Guid.Empty).Where(l => l.EstablishmentCustomerId == Global.Config.IdEstablishment);
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var closeTicketG = TableCloseTicketGs.SelectedItem as CloseTicketG;

            if (closeTicketG != null)
            {
                TableCloseTicket.DataContext = RepositoryCloseTicket.GetByCloseTicketGId(closeTicketG.CustomerId);
                CollectionViewSource.GetDefaultView(TableCloseTicket.ItemsSource).Refresh();
            }
            else TableCloseTicket.DataContext = null;
        }

        private void ListSelectedCellsChanged1(object sender, SelectedCellsChangedEventArgs e)
        {
            var closeTicket = TableCloseTicket.SelectedItem as CloseTicket;

            if (closeTicket != null)
            {
                TableCheckTickets.DataContext = RepositoryCheckTicket.GetByCloseTicketId(closeTicket.CustomerId);
                CollectionViewSource.GetDefaultView(TableCheckTickets.ItemsSource).Refresh();
            }
            else TableCheckTickets.DataContext = null;
        }

        private void ListSelectedCellsChanged2(object sender, SelectedCellsChangedEventArgs e)
        {
            var checkTicket = TableCheckTickets.SelectedItem as CheckTicket;

            if (checkTicket != null)
            {
                TablePayProducts.DataContext = RepositoryPayProduct.GetByCheckTicketId(checkTicket.CustomerId);
                CollectionViewSource.GetDefaultView(TablePayProducts.ItemsSource).Refresh();
            }
            else TablePayProducts.DataContext = null;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var check = TableCheckTickets.SelectedItem as CheckTicket;

            if (check != null)
                new ClassPrintCheck(new XDocument(CheckTicket.ToCheckXElement(check)), true);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}