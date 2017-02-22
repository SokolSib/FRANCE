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
using System.Xml.Linq;

namespace ticketwindow.Winows.EnAttente
{
    /// <summary>
    /// Логика взаимодействия для W_enAttente.xaml
    /// </summary>
    public partial class W_enAttente : Window
    {
        public W_enAttente()
        {
            InitializeComponent();
            xcasse.DataContext = new Class.ClassSync.En_attenete.SyncPlus().sel(); // Class.ClassCheck.a.Element("checks").Elements("check");
        }

        private Class.ClassSync.En_attenete.SyncPlusProducts x;
        private void list_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.En_attenete.SyncPlus s = (Class.ClassSync.En_attenete.SyncPlus)xcasse.SelectedItem;

            xcheck.DataContext = new Class.ClassSync.En_attenete.SyncPlusProducts().sel(s.customerId) ;

            CollectionViewSource.GetDefaultView(xcheck.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (x != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        Class.ClassCheck.buf_addXElm(x.check.Element("check"));
                        DataGrid dg = (window as MainWindow).dataGrid1;

                        dg.DataContext = Class.ClassCheck.b.Element("check");

                        CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();

                        this.Close();

                      

                        Class.ClassCheck.save_en_attenete(x.customerId);
                    }

                }


            }
        }

        private void xcheck_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Class.ClassSync.En_attenete.SyncPlusProducts s = (Class.ClassSync.En_attenete.SyncPlusProducts)xcheck.SelectedItem;

            x = s;

            listDetails.DataContext = s.check.Element("check").Elements("product");

            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
