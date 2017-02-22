using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.DetailsProducts
{
    /// <summary>
    ///     Логика взаимодействия для W_DetailsProducts.xaml
    /// </summary>
    public partial class WDetailsProducts : Window
    {
        public WDetailsProducts()
        {
            InitializeComponent();
            SetDataSet();
        }

        public void SetDataSet()
        {
            var mainWindow = (MainWindow) Application.Current.MainWindow;
            DataContext = mainWindow.CheckOriginalDocument != null ? mainWindow.CheckOriginalDocument.Root : mainWindow.GridProducts.DataContext;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            TbBareCode.Focus();
        }

        private void TbBareCodeKeyUp(object sender, KeyEventArgs e)
        {
            var box = sender as TextBox;

            if (e.Key == Key.Enter && box != null)
            {
                CheckService.DelProductCheck(box.Text);
                ProductsGrid.DataContext = RepositoryCheck.DocumentProductCheck.Element("check");
                CollectionViewSource.GetDefaultView(ProductsGrid.ItemsSource).Refresh();
                ProductsGrid.SelectedIndex = ProductsGrid.Items.Count - 1;
                box.Text = string.Empty;
            }
        }

        private void FinishClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}