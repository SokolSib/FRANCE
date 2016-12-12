using System.Windows;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Divers
{
    /// <summary>
    /// Логика взаимодействия для W_ModifierPrix.xaml
    /// </summary>
    public partial class WModifierPrix : Window
    {
        public ProductType Product { get; }

        public decimal Price => !string.IsNullOrEmpty(XValue.Text) ? decimal.Parse(XValue.Text) : 0;

        public WModifierPrix(ProductType product)
        {
            InitializeComponent();
            Product = product;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            XValue.Text = $"{Product.Price}";
            XNameProduct.Text = Product.Name;
            NumPad.TextBox = XValue;
            NumPad.BEnter = XEnter;
        }

        private void XEnterClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Product.Price == 0.0m)
            {
                var window = ClassEtcFun.FindWindow("MainWindow_") as MainWindow;
                if (window != null)
                {
                    var dataGrid = window.GridProducts;
                    var selected = (XElement)dataGrid.SelectedItem;
                    if (selected != null)
                        CheckService.DelProductCheck(selected.GetXElementValue("ii").ToInt());
                }
            }
            Close();
        }
    }
}
