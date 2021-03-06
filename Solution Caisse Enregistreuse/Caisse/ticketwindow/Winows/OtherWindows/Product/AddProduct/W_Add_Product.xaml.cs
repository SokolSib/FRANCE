﻿using System;
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
using TicketWindow.Global;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Product.AddProduct
{
    /// <summary>
    ///     Логика взаимодействия для W_AddProduct.xaml
    /// </summary>
    public partial class WAddProduct : Window
    {
        private readonly BackgroundWorker _workerAdd = new BackgroundWorker();
        private readonly BackgroundWorker _workerEdit = new BackgroundWorker();
        private decimal _stockRealCount;

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

            _workerAdd.RunWorkerCompleted += WorkerCompleted;
            _workerEdit.RunWorkerCompleted += WorkerCompleted;
            _workerAdd.DoWork += WorkerAddDoWork;
            _workerEdit.DoWork += WorkerEditDoWork;
        }

        public ProductType Product { get; }

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
                                if ((x != null) && (x.GetXElementValue("CodeBare") == tb.Text))
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
                            listError = $"{((FrameworkElement) sender).ToolTip} {Properties.Resources.LabelIncorrect}";
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

                listError = cb.SelectedItem == null
                    ? $"{((FrameworkElement) sender).ToolTip} {Properties.Resources.LabelIncorrect}"
                    : null;

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
            if (RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactStockCount))
                p.Qty = xStockCount.Text.ToDecimal();
            else if (Product != null)
                p.Qty = Product.Qty;

            return p;
        }

        private static WGridProduct GetParents()
        {
            WGridProduct returnValue = null;
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(WGridProduct))
                    returnValue = window as WGridProduct;
            return returnValue;
        }

        private void AddElm()
        {
            ProgressHelper.Instance.IsIndeterminate = true;
            ProgressHelper.Instance.Start(1, Properties.Resources.LabelPleaseWaitWhileLoading);
            var product = FormToVar();
            decimal.TryParse(xStockCount.Text.Replace(".", ","), out _stockRealCount);

            _workerAdd.RunWorkerAsync(product);
        }
        
        private void EditElm()
        {
            ProgressHelper.Instance.IsIndeterminate = true;
            ProgressHelper.Instance.Start(1, Properties.Resources.LabelPleaseWaitWhileLoading);
            var product = FormToVar();
            decimal.TryParse(xStockCount.Text.Replace(".", ","), out _stockRealCount);

            _workerEdit.RunWorkerAsync(product);
        }

        private void WorkerEditDoWork(object sender, DoWorkEventArgs e)
        {
            var product = (ProductType) e.Argument;
            RepositoryProduct.Update(product);
            RepositoryProduct.UpdateProductPrice(product);

            if (RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactStockCount))
            {
                var stockReal = RepositoryStockReal.GetByProduct(product);
                RepositoryStockReal.UpdateProductCount(_stockRealCount, stockReal.CustomerId);
            }
        }

        private void WorkerAddDoWork(object sender, DoWorkEventArgs e)
        {
            var product = (ProductType) e.Argument;
            RepositoryProduct.Add(product);

            if (RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactStockCount))
            {
                var stockReal = RepositoryStockReal.GetByProduct(product);
                RepositoryStockReal.AddAsNull(product.CustomerId, Config.IdEstablishment);
                RepositoryStockReal.UpdateProductCount(_stockRealCount, stockReal.CustomerId);
            }
        }
        
        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var dg = GetParents().DataGrid;
            Close();
            dg.ItemsSource = RepositoryProduct.Products;
            CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
            ProgressHelper.Instance.Stop();
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