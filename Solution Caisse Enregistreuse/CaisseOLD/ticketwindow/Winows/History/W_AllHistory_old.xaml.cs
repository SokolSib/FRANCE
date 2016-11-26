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
    public partial class W_AllHistory_old : Window
    {


        public W_AllHistory_old()
        {
            InitializeComponent();

            list.DataContext = Class.ClassSync.ClassCloseTicketTmp.CloseTicket.sel().Where(l=>l.EstablishmentCustomerId == Class.ClassGlobalVar.IdEstablishment);
        }

        private void list_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicketTmp.CloseTicket s = list.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.CloseTicket;

           
            listDetails.DataContext = Class.ClassSync.ClassCloseTicketTmp.ChecksTicket.sel(s.CustumerId);

            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void list_SelectedCellsChanged_(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicketTmp.ChecksTicket s = listDetails.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.ChecksTicket;


            if (s != null)
            {
                var el = Class.ClassSync.ClassCloseTicketTmp.PayProducts.sel(s.CustumerId);

                listDetailsProducts.DataContext = el;

                CollectionViewSource.GetDefaultView(listDetailsProducts.ItemsSource).Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicket.ChecksTicket s = listDetails.SelectedItem as Class.ClassSync.ClassCloseTicket.ChecksTicket;

            if (s != null)
                 new Class.ClassPrintCheck(Class.ClassCheck.DbToXelemet(s), true);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
