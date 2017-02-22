using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ticketwindow.Winows.Product.RemoveProduct
{
    /// <summary>
    /// Логика взаимодействия для W_Remove_product.xaml
    /// </summary>
    public partial class W_Remove_product : Window
    {
        private Class.ClassProducts.product product { get; set; }
        public W_Remove_product(object product)
        {
            InitializeComponent();
            this.product = (Class.ClassProducts.product)product;

            if (product != null) loadFromWindow(this.product);
        }
        private void xBalance_Click(object sender, RoutedEventArgs e)
        {
            Visibility v = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
            xContenance.Visibility = v;
            xUnit_contenance.Visibility = v;
            xTare.Visibility = v;
        }
        private void loadFromWindow(Class.ClassProducts.product p)
        {
            xCodeBar.Text = p.CodeBare;
            xName.Text = p.Name;
            xPrice.Text = p.price.ToString();
            xTVA.SelectedValue = p.tva;
            xQTY.Text = p.qty.ToString();
            xDetails.Text = p.Desc;


            xGroup.SelectedValue = p.grp;
            xSub_group.SelectedValue = p.sgrp;

            xBalance.IsChecked = !p.balance;
            xBalance_Click(xBalance, null);
            xContenance.Text = p.contenance.ToString();
            xUnit_contenance.Text = p.uniteContenance.ToString();
            xTare.Text = p.tare.ToString();
        }
        private W_Grid_Product GetParents(Object element, int parentLevel)
        {
            W_Grid_Product returnValue = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(W_Grid_Product))
                {
                    returnValue = (window as W_Grid_Product);
                }
            }
            return returnValue;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = (GetParents(this, 0)).dataGrid1;
            
            Class.ClassProducts.remove(product);

            CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
