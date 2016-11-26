using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ticketwindow.Winows.History
{
    /// <summary>
    /// Логика взаимодействия для W_AllHistory.xaml
    /// </summary>
    public partial class W_AllHistory : Window
    {


        public W_AllHistory()
        {
            InitializeComponent();

            list.DataContext = Class.ClassSync.ClassCloseTicket.CloseTicketG.sel(Guid.Empty).Where(l=>l.EstablishmentCustomerId == Class.ClassGlobalVar.IdEstablishment);
        }

        private void list_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicket.CloseTicketG s = list.SelectedItem as Class.ClassSync.ClassCloseTicket.CloseTicketG;
           
            listDetails.DataContext = new Class.ClassSync.ClassCloseTicket.CloseTicket().get(s.CustumerId);

            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void list_SelectedCellsChanged_(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicket.CloseTicket s = listDetails.SelectedItem as Class.ClassSync.ClassCloseTicket.CloseTicket;

            listDetails_.DataContext = new Class.ClassSync.ClassCloseTicket.ChecksTicket().get(s.CustumerId);

            CollectionViewSource.GetDefaultView(listDetails_.ItemsSource).Refresh();
        }



        private void list_SelectedCellsChanged__(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicket.ChecksTicket s = listDetails_.SelectedItem as Class.ClassSync.ClassCloseTicket.ChecksTicket;

            if (s != null)
            {
                var el = new Class.ClassSync.ClassCloseTicket.PayProducts().get(s.CustumerId);

                listDetailsProducts.DataContext = el;

                CollectionViewSource.GetDefaultView(listDetailsProducts.ItemsSource).Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicket.ChecksTicket s = listDetails_.SelectedItem as Class.ClassSync.ClassCloseTicket.ChecksTicket;

            if (s != null)
                 new Class.ClassPrintCheck(Class.ClassCheck.DbToXelemet(s), true);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
