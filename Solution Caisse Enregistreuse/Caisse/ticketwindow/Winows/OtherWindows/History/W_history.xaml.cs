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
        private XElement _elements;

        public WHistory()
        {
            InitializeComponent();
            try
            {
                list.DataContext =
                    RepositoryCheck.Document.GetXElements("checks", "check").Reverse();
            }
            catch
            {
                FunctionsService.ShowMessageTime("Пусто");
            }
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var s = (XElement) list.SelectedItem;

            _elements = RepositoryCheck.Document.GetXElements("checks", "check").FirstOrDefault(l => l.GetXAttributeValue("barcodeCheck") == s.GetXAttributeValue("barcodeCheck"));

            listDetails.DataContext = _elements.GetXElements("product");

            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var s = list.SelectedItem as XElement;

            if (s != null)
            {
                var x = new XDocument(s);
                new ClassPrintCheck(x, true);
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}