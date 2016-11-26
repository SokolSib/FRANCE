using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.Winows.OtherWindows.Product.ChangeProduct
{
    /// <summary>
    /// Логика взаимодействия для W_Change_Product.xaml
    /// </summary>
    public partial class WChangeProduct : Window
    {
        private string _listError = null;
        private ProductType Product { get; set; }

        public WChangeProduct(object product)
        {
            InitializeComponent();
            Product = (ProductType)product;
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

            xBalance.IsChecked = p.Balance;
        }

        private string ValidTextBox(object sender)
        {
            _listError = null;

            var box = sender as TextBox;
            if (box != null)
            {
                var tb = box;

                switch (tb.Name)
                {
                    case "xCodeBar":
                        try
                        {
                            XElement x = (RepositoryProduct.GetXElementByBarcode(xCodeBar.Text));
                            if (x != null)
                                if ((x.Element("CodeBare").Value != Product.CodeBare))
                                    _listError = ("Ce Nom de produit existe déjà");
                        }
                        catch
                        {
                            _listError = ("Code-barres EAN incorrect");
                        }
                        break;
                    
                    case "xName":
                        try
                        {
                            XElement x = (RepositoryProduct.GetXElementByElementName("Name", tb.Text.Trim().ToUpper()));

                            if (x != null)
                                if ((x.Element("Name").Value != Product.Name) )
                                    _listError = ("Ce Nom de produit existe déjà");
                        }
                        catch
                        {
                            _listError = ("Ce Nom de produit existe déjà");
                        }
                        break;
                    
                    case "xPrice":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text.Replace(".", ","));
                        }
                        catch
                        {
                            _listError = ("Le prix incorrect");
                        }
                        break;
                }

                tb.Foreground = (_listError != null) ?
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)) :
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                ((Label)FindName("l" + tb.Name)).Foreground = tb.Foreground;
            }

            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var cb = comboBox;

                _listError = cb.SelectedItem == null ? "Vérifier tous les domaines" : null;

                cb.Foreground = (_listError != null) ?
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)) :
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                ((Label)FindName("l" + cb.Name)).Foreground = cb.Foreground;
            }

           

            return _listError;
        }

        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            ValidTextBox(sender);
        }

        private void XBalanceClick(object sender, RoutedEventArgs e)
        {
            var v = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
        }

        private bool IsValidTextBox()
        {
            string s = null;

            s += ValidTextBox(xCodeBar);
            s += ValidTextBox(xName);
            s += ValidTextBox(xPrice);
            s += ValidTextBox(xQTY);
            s += ValidTextBox(xTVA);
            s += ValidTextBox(xGroup);
            s += ValidTextBox(xSub_group);
           
            return s == "";
        }

        private ProductType FormToVar()
        {
            string tvaId = null;
            if (xTVA.SelectedValue != null)
                tvaId = xTVA.SelectedValue.ToString();
            
            var tva = RepositoryTva.Tvases.FirstOrDefault(t => t.Id == tvaId.ToInt());
            var sgrp = int.Parse(xSub_group.SelectedValue.ToString());

            var p = new ProductType(Product.CustomerId, xName.Text, xCodeBar.Text, xDetails.Text, 0,
                xBalance.IsChecked ?? false,
                decimal.Parse(xContenance.Text == "" ? "0" : xContenance.Text.Replace(".", ",")),
                int.Parse(xUnit_contenance.Text == "" ? "0" : xUnit_contenance.Text), int.Parse(xTare.Text == "" ? "0" : xTare.Text),
                DateTime.Now,tva!=null? tva.CustomerId:Guid.Empty, Product.ProductsWebCustomerId, sgrp)
                    {
                        Price = decimal.Parse(xPrice.Text.Replace(".", ",")),
                        Qty = decimal.Parse(xQTY.Text.Replace(".", ",")),
                        Grp = int.Parse(xGroup.SelectedValue.ToString()),
                        Sgrp = int.Parse(xSub_group.SelectedValue.ToString())
                    };

            p.CusumerIdSubGroup = p.Sgrp;
            p.TvaId = (xTVA.SelectedValue == null) ? -1 : int.Parse(xTVA.SelectedValue.ToString());
            p.CusumerIdRealStock = Product.CusumerIdRealStock;

            return p;
        }

        private void ModifElm()
        {
            RepositoryProduct.Update(FormToVar());
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
            {
                ModifElm();
                Close();
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Product != null) LoadFromWindow(this.Product);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
