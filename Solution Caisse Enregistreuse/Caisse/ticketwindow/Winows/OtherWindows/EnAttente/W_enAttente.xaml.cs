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
        private SyncPlusProductType _selectedSyncPlusProduct;

        public WEnAttente()
        {
            InitializeComponent();
            if (RepositorySyncPlus.SyncPluses.Count == 0) RepositorySyncPlus.Sync();
            Xcasse.DataContext = RepositorySyncPlus.SyncPluses;
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var syncPlus = (SyncPlus)Xcasse.SelectedItem;
            Xcheck.DataContext = RepositorySyncPlusProduct.GetByIdSyncPlus(syncPlus.CustomerId);
            CollectionViewSource.GetDefaultView(Xcheck.ItemsSource).Refresh();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_selectedSyncPlusProduct != null)
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof (MainWindow))
                    {
                        CheckService.BufAddXElm(_selectedSyncPlusProduct.Check.Element("check"));
                        var dg = ((MainWindow) window).GridProducts;

                        dg.DataContext = RepositoryCheck.DocumentProductCheck.Element("check");

                        CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();

                        Close();

                        CheckService.SaveEnAttenete(_selectedSyncPlusProduct.CustomerId);
                    }
                }
        }

        private void XcheckSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = (SyncPlusProductType)Xcheck.SelectedItem;
            _selectedSyncPlusProduct = s;
            ListDetails.DataContext = s.Check.GetXElements("check", "product");
            CollectionViewSource.GetDefaultView(ListDetails.ItemsSource).Refresh();
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}