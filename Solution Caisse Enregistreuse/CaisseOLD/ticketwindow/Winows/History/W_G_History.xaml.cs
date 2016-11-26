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
    /// Логика взаимодействия для W_G_History.xaml
    /// </summary>
    public partial class W_G_History : Window
    {
        public W_G_History()
        {
            InitializeComponent();
            list.DataContext = Class.ClassSync.ClassCloseTicket.CloseTicketG.sel(Guid.Empty);
        }

        private void list_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicket.CloseTicketG s = list.SelectedItem as Class.ClassSync.ClassCloseTicket.CloseTicketG;

            if (s != null)
                Class.ClassSync.ClassCloseTicket.CloseTicketG.prnt(s.CustumerId);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
