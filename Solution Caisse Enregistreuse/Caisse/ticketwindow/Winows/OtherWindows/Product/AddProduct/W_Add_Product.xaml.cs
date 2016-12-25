using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Product.AddProduct
{
    /// <summary>
    ///     Логика взаимодействия для W_AddProduct.xaml
    /// </summary>
    public partial class WAddProduct : Window
    {
        public ProductType Product { get; }

        public WAddProduct(ProductType product = null)
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

            if (product != null)
            {
                Save.Content = Properties.Resources.BtnUpdate;
                Product = product;
                xName.Text = product.Name;
                xCodeBar.Text = product.CodeBare;
                xPrice.Text = $"{product.Price}";
                TvaBox.SelectedItem = product.Tva;
                xBalance.IsChecked = product.Balance;

                var group =
                    RepositoryGroupProduct.GroupProducts.FirstOrDefault(g => g.Id == product.SubGrpProduct.Group.Id);
                GroupBox.SelectedItem = group;

                if (group != null)
                {
                    SubgroupBox.ItemsSource = group.SubGroups;
                    SubgroupBox.SelectedItem = group.SubGroups.FirstOrDefault(s => s.Id == product.SubGrpProduct.Id);
                }
            }

            if (!RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactStockCount))
            {
                lStockCount.Visibility = Visibility.Collapsed;
                xStockCount.Visibility = Visibility.Collapsed;
            }
            else if (product != null)
            {
                var stockReal = RepositoryStockReal.GetByProduct(product);
                xStockCount.Text = $"{stockReal.Qty}";
            }

            BoxErrorText.Text = string.Empty;
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
                            var x = RepositoryProduct.GetXElementByBarcode(tb.Text);
                            if ((x != null) || (tb.Text.Length < 0))
                                if (x != null && (x.GetXElementValue("CodeBare") == tb.Text))
                                    listError = Properties.Resources.LabelEanAlreadyExists;
                        }
                        catch
                        {
                            listError = Properties.Resources.LabelIncorrectEAN;
                        }
                        break;
                    case "xName":
                        if (string.IsNullOrEmpty(tb.Text))
                            listError = $"{((FrameworkElement) sender).ToolTip} {Properties.Resources.LabelIncorrect}";
                        else
                        {
                            var element = RepositoryProduct.GetXElementByElementName("Name", tb.Text.Trim().ToUpper());
                            if (tb.Text.Length < 2)
                                try
                                {
                                    if (element.GetXElementValue("Name") == tb.Text)
                                        listError = "Nom";
                                }
                                catch
                                {
                                    listError = Properties.Resources.LabelNameAlreadyExists;
                                }
                        }
                        break;
                    case "xPrice":
                    case "xQTY":
                    case "xContenance":
                    case "xStockCount":
                        decimal decimalValue;
                        if (!decimal.TryParse(tb.Text.Replace(".", ","), out decimalValue))
                            listError = $"{((FrameworkElement) sender).ToolTip} {Properties.Resources.LabelIncorrect}";
                        break;
                    case "xUnit_contenance":
                    case "xTare":
                        int intValue;
                        if (!int.TryParse(tb.Text, out intValue))
                            listError = $"{((FrameworkElement)sender).ToolTip} {Properties.Resources.LabelIncorrect}";
                        break;
                }

                tb.Foreground = listError != null
                    ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0))
                    : new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                var findName = (Label) FindName("l" + tb.Name);
                if (findName != null)
                    findName.Foreground = tb.Foreground;
            }

            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var cb = comboBox;

                listError = cb.SelectedItem == null ? $"{((FrameworkElement)sender).ToolTip} {Properties.Resources.LabelIncorrect}" : null;

                cb.Foreground = listError != null
                    ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0))
                    : new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                var findName = (Label) FindName("l" + cb.Name);
                if (findName != null)
                    findName.Foreground = cb.Foreground;
            }
            return listError;
        }

        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            BoxErrorText.Text = ValidTextBox(sender);
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
            s += ValidTextBox(xStockCount);
            return s == "";
        }

        private ProductType FormToVar()
        {
            string tvaId = null;
            if (TvaBox.SelectedValue != null)
                tvaId = TvaBox.SelectedValue.ToString();

            var tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == tvaId.ToInt());
            var id = Product?.CustomerId ?? Guid.NewGuid();
            var idStock = Product?.CusumerIdRealStock ?? Guid.NewGuid();
            var desc = Product?.Desc ?? string.Empty;
            var chpCat = Product?.ChpCat ?? 0;
            var contenance = Product?.Contenance ?? 0;
            var uniteContenance = Product?.UniteContenance ?? 0;
            var tare = Product?.Tare ?? 0;
            var productsWebCustomerId = Product?.ProductsWebCustomerId ?? Guid.NewGuid();

            var p = new ProductType(id, xName.Text, xCodeBar.Text, desc, chpCat, xBalance.IsChecked ?? false,
                        contenance, uniteContenance, tare, DateTime.Now, tva?.CustomerId ?? Guid.Empty,
                        productsWebCustomerId, SubgroupBox.SelectedValue.ToString().ToInt())
                    {
                        Price = xPrice.Text.ToDecimal(),
                        CusumerIdRealStock = idStock
                    };

            return p;
        }

        private static WGridProduct GetParents()
        {
            WGridProduct returnValue = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof (WGridProduct))
                    returnValue = window as WGridProduct;
            }
            return returnValue;
        }

        private void AddElm()
        {
            var worker = new BackgroundWorker();
            ProgressHelper.Instance.IsIndeterminate = true;
            ProgressHelper.Instance.Start(1, Properties.Resources.LabelPleaseWaitWhileLoading);
            var product = FormToVar();

            worker.DoWork += (s, e) => { RepositoryProduct.Add(product); };
            worker.RunWorkerCompleted += (s, e) =>
            {
                var dg = GetParents().DataGrid;
                Close();
                CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
            };
            worker.RunWorkerAsync();

            //RepositoryProduct.Add(FormToVar());
            //var dg = GetParents().DataGrid;
            //Close();
            //CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
        }
        
        private void EditElm()
        {
            var worker = new BackgroundWorker();
            ProgressHelper.Instance.IsIndeterminate = true;
            ProgressHelper.Instance.Start(1, Properties.Resources.LabelPleaseWaitWhileLoading);
            var product = FormToVar();
            decimal stockRealCount;
            decimal.TryParse(xStockCount.Text.Replace(".", ","), out stockRealCount);

            worker.DoWork += (s, e) =>
                             {
                                 RepositoryProduct.Update(product);
                                 RepositoryProduct.UpdateProductPrice(product);

                                 if (RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactStockCount))
                                 {
                                     var stockReal = RepositoryStockReal.GetByProduct(product);
                                     RepositoryStockReal.UpdateProductCount(stockRealCount, stockReal.CustomerId);
                                 }
                             };
            worker.RunWorkerCompleted += (s, e) =>
            {
                var dg = GetParents().DataGrid;
                Close();
                dg.ItemsSource = RepositoryProduct.Products;
                CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
            };
            worker.RunWorkerAsync();

            //var product = FormToVar();
            //RepositoryProduct.Update(product);
            //RepositoryProduct.UpdateProductPrice(product);

            //if (RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactStockCount))
            //{
            //    var stockReal = RepositoryStockReal.GetByProduct(product);

            //    decimal stockRealCount;
            //    decimal.TryParse(xStockCount.Text.Replace(".", ","), out stockRealCount);

            //    RepositoryStockReal.UpdateProductCount(stockRealCount, stockReal.CustomerId);
            //}

            //var dg = GetParents().DataGrid;
            //Close();
            //dg.ItemsSource = RepositoryProduct.Products;
            //CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
                if (Product == null) AddElm();
                else EditElm();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}