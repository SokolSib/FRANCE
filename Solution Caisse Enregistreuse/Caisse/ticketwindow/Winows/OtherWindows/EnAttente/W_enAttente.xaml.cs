using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.EnAttente
{
    /// <summary>
    ///     Логика взаимодействия для W_enAttente.xaml
    /// </summary>
    public partial class WEnAttente : Window
    {
        private SyncPlusProductType _x;

        public WEnAttente()
        {
            InitializeComponent();
            if (RepositorySyncPlus.SyncPluses.Count == 0) RepositorySyncPlus.Sync();

            if (Global.Config.IsUseServer)
                xcasse.DataContext = RepositorySyncPlus.SyncPluses;
            else
                xcasse.DataContext = RepositoryCheck.DocumentEnAttenete.GetXElements("checks", "check");
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = (SyncPlus)xcasse.SelectedItem;

            xcheck.DataContext = RepositorySyncPlusProduct.GetByIdSyncPlus(s.CustomerId);

            CollectionViewSource.GetDefaultView(xcheck.ItemsSource).Refresh();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_x != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof (MainWindow))
                    {
                        CheckService.BufAddXElm(_x.Check.Element("check"));
                        var dg = (window as MainWindow).GridProducts;

                        dg.DataContext = RepositoryCheck.DocumentProductCheck.Element("check");

                        CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();

                        Close();

                        CheckService.SaveEnAttenete(_x.CustomerId);
                    }
                }
            }
        }

        private void XcheckSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = (SyncPlusProductType)xcheck.SelectedItem;
            _x = s;
            listDetails.DataContext = s.Check.GetXElements("check", "product");
            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}