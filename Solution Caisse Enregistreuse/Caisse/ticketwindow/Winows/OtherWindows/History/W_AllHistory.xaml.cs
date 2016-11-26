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
            list.DataContext = RepositoryCloseTicketG.Get(Guid.Empty).Where(l => l.EstablishmentCustomerId == Global.Config.IdEstablishment);
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = list.SelectedItem as CloseTicketG;
            listDetails.DataContext = RepositoryCloseTicket.GetByCloseTicketGId(s.CustomerId);
            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void ListSelectedCellsChanged1(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = listDetails.SelectedItem as CloseTicket;
            listDetails_.DataContext = RepositoryCheckTicket.GetByCloseTicketId(s.CustomerId);
            CollectionViewSource.GetDefaultView(listDetails_.ItemsSource).Refresh();
        }

        private void ListSelectedCellsChanged2(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = listDetails_.SelectedItem as CheckTicket;

            if (s != null)
            {
                var el = RepositoryPayProduct.GetByCheckTicketId(s.CustomerId);
                listDetailsProducts.DataContext = el;
                CollectionViewSource.GetDefaultView(listDetailsProducts.ItemsSource).Refresh();
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var check = listDetails_.SelectedItem as CheckTicket;

            if (check != null)
                new ClassPrintCheck(new XDocument(CheckTicket.ToCheckXElement(check)), true);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}