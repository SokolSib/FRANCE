using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Product
{
    /// <summary>
    /// Логика взаимодействия для W_Grid_Product.xaml
    /// </summary>
    public partial class WGridProduct : Window
    {
        public WGridProduct()
        {
            InitializeComponent();
            DataGrid.ItemsSource = RepositoryProduct.Products;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender, (ProductType) DataGrid.SelectedItem);
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
