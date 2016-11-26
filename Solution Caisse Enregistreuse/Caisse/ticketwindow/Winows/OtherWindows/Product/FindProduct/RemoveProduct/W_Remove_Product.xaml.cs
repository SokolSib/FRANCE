using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.Product.FindProduct.RemoveProduct
{
    /// <summary>
    /// Логика взаимодействия для W_Remove_product.xaml
    /// </summary>
    public partial class WRemoveProduct : Window
    {
        private ProductType Product { get; set; }

        public WRemoveProduct(object product)
        {
            InitializeComponent();
            Product = (ProductType)product;
            if (product != null) LoadFromWindow(Product);
        }

        private void XBalanceClick(object sender, RoutedEventArgs e)
        {
            var v = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
            xContenance.Visibility = v;
            xUnit_contenance.Visibility = v;
            xTare.Visibility = v;
        }

        private void LoadFromWindow(ProductType p)
        {
            xCodeBar.Text = p.CodeBare;
            xName.Text = p.Name;
            xPrice.Text = p.Price.ToString();
            xTVA.SelectedValue = p.Tva;
            xQTY.Text = p.Qty.ToString();
            xDetails.Text = p.Desc;
            xGroup.SelectedValue = p.Grp;
            xSub_group.SelectedValue = p.Sgrp;
            xBalance.IsChecked = !p.Balance;
            XBalanceClick(xBalance, null);
            xContenance.Text = p.Contenance.ToString();
            xUnit_contenance.Text = p.UniteContenance.ToString();
            xTare.Text = p.Tare.ToString();
        }

        private static WGridProduct GetParents(Object element, int parentLevel)
        {
            WGridProduct returnValue = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WGridProduct))
                    returnValue = (window as WGridProduct);
            }
            return returnValue;
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            var dg = (GetParents(this, 0)).DataGrid;
            RepositoryProduct.Delete(Product);
            CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
