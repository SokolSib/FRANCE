using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Product.AddProduct
{
    /// <summary>
    ///     Логика взаимодействия для W_AddProduct.xaml
    /// </summary>
    public partial class WAddProduct : Window
    {
        public WAddProduct()
        {
            var windowProducts = ClassEtcFun.FindWindow("NameWGridProduct");

            if (windowProducts == null)
            {
                windowProducts = new WGridProduct();
                windowProducts.Show();
            }
            InitializeComponent();
            TvaBox.ItemsSource = RepositoryTva.Tvases;
            GroupBox.ItemsSource = RepositoryGroupProduct.GroupProducts;
        }

        private string ValidTextBox(object sender)
        {
            string listError = null;

            var box = sender as TextBox;
            if (box != null)
            {
                var tb = box;
                var name = tb.Name;

                switch (name)
                {
                    case "xCodeBar":
                        try
                        {
                            var x = (RepositoryProduct.GetXElementByBarcode(tb.Text));
                            if ((x != null) || (tb.Text.Length < 0))
                                if ((x.Element("CodeBare").Value == tb.Text))
                                    listError = ("Code-barres EAN existe déjà");
                        }
                        catch
                        {
                            listError = ("Code-barres EAN incorrect");
                        }
                        break;
                    case "xName":
                        var n = (RepositoryProduct.GetXElementByElementName("Name", tb.Text.Trim().ToUpper()));
                        if (tb.Text.Length < 2)
                            try
                            {
                                if ((n.GetXElementValue("Name") == tb.Text))
                                    listError = ("Nom");
                            }
                            catch
                            {
                                listError = ("Ce Nom de produit existe déjà");
                            }
                        break;
                    case "xPrice":
                    case "xQTY":
                    case "xContenance":
                        decimal decimalValue;
                        if (!decimal.TryParse(tb.Text.Replace(".", ","), out decimalValue))
                            listError = (string.Format("{0} incorrect", name));
                        break;
                    case "xUnit_contenance":
                    case "xTare":
                        int intValue;
                        if (!int.TryParse(tb.Text, out intValue))
                            listError = (string.Format("{0} incorrect", name));
                        break;
                }

                tb.Foreground = (listError != null)
                    ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0))
                    : new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                ((Label) FindName("l" + tb.Name)).Foreground = tb.Foreground;
            }

            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var cb = comboBox;

                listError = cb.SelectedItem == null ? "the Cb not correct" : null;

                cb.Foreground = (listError != null)
                    ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0))
                    : new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                ((Label) FindName("l" + cb.Name)).Foreground = cb.Foreground;
            }
            return listError;
        }

        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            ValidTextBox(sender);
        }

        private void BalanceClick(object sender, RoutedEventArgs e)
        {
            var v = (xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool IsValidTextBox()
        {
            string s = null;

            s += ValidTextBox(xCodeBar);
            s += ValidTextBox(xName);
            s += ValidTextBox(xPrice);
            s += ValidTextBox(TvaBox);
            s += ValidTextBox(GroupBox);
            s += ValidTextBox(SubgroupBox);
            return s == "";
        }

        private ProductType FormToVar()
        {
            string tvaId = null;
            if (TvaBox.SelectedValue != null)
                tvaId = TvaBox.SelectedValue.ToString();

            var tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == tvaId.ToInt());

            var p = new ProductType(Guid.NewGuid(), xName.Text, xCodeBar.Text, string.Empty, 0, xBalance.IsChecked ?? false,
                0, 0, 0, DateTime.Now, tva != null ? tva.CustomerId : Guid.Empty, Guid.NewGuid(), SubgroupBox.SelectedValue.ToString().ToInt())
                    {
                        Price = xPrice.Text.ToDecimal(),
                        CusumerIdRealStock = Guid.NewGuid()
                    };

            return p;
        }

        private static WGridProduct GetParents()
        {
            WGridProduct returnValue = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof (WGridProduct))
                    returnValue = (window as WGridProduct);
            }
            return returnValue;
        }

        private void AddElm()
        {
            RepositoryProduct.Add(FormToVar());
            var dg = (GetParents()).DataGrid;
            Close();
            CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
                AddElm();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}