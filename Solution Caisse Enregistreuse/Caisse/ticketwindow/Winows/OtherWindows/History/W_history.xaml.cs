using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;
using TicketWindow.Class;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.History
{
    public class MyConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            switch (parameter.ToString())
            {
                case "Date":
                    return DateTime.Parse(o.ToString()).ToString("hh:mm", culture);
                case "Money":
                    return (decimal.Parse(((string) o).Replace('.', ','))).ToString("C", culture);
                default:
                    return o;
            }
        }

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    ///     Логика взаимодействия для W_history.xaml
    /// </summary>
    public partial class WHistory : Window
    {
        public WHistory()
        {
            InitializeComponent();
            try
            {
                RepositoryCheck.GetDucument();
                TableChecks.DataContext = RepositoryCheck.Document.GetXElements("checks", "check").Reverse();
            }
            catch
            {
                FunctionsService.ShowMessageTime("Пусто");
            }
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var checkElement = (XElement)TableChecks.SelectedItem;

            var elements = RepositoryCheck.Document.GetXElements("checks", "check").FirstOrDefault(
                        l => l.GetXAttributeValue("barcodeCheck") == checkElement.GetXAttributeValue("barcodeCheck"));

            TableProducts.DataContext = elements.GetXElements("product");
            CollectionViewSource.GetDefaultView(TableProducts.ItemsSource).Refresh();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var checkElement = TableChecks.SelectedItem as XElement;

            if (checkElement != null)
                new ClassPrintCheck(new XDocument(checkElement), true);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}