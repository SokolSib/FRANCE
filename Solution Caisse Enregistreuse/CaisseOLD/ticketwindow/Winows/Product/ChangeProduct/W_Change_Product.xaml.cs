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
using System.Xml.Linq;

namespace ticketwindow.Winows.Product.ChangeProduct
{
    /// <summary>
    /// Логика взаимодействия для W_Change_Product.xaml
    /// </summary>
    public partial class W_Change_Product : Window
    {
        private string listError = null;
        private Class.ClassProducts.product product { get; set; }
        public W_Change_Product(object product)
        {
            InitializeComponent();

            this.product = (Class.ClassProducts.product)product;

          
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

            xBalance.IsChecked = p.balance;
        //    xBalance_Click(xBalance, null);
           /* xContenance.Text = p.contenance.ToString();
            xUnit_contenance.Text = p.uniteContenance.ToString();
            xTare.Text = p.tare.ToString();*/
        }
        private string validTextBox(object sender)
        {
            listError = null;

            if (sender is TextBox)
            {
                TextBox tb = ((TextBox)sender);

                switch (tb.Name)
                {
                    case "xCodeBar":
                        try
                        {
                            XElement x = (Class.ClassProducts.findCodeBar(xCodeBar.Text));
                            if (x != null)
                                if ((x.Element("CodeBare").Value != product.CodeBare))
                                    listError = ("Ce Nom de produit existe déjà");
                        }
                        catch
                        {
                            listError = ("Code-barres EAN incorrect");
                        }
                        break;
                    
                    case "xName":
                        try
                        {
                            XElement x = (Class.ClassProducts.findName(tb.Text));

                            if (x != null)
                                if ((x.Element("Name").Value != product.Name) )
                                    listError = ("Ce Nom de produit existe déjà");
                        }
                        catch
                        {
                            listError = ("Ce Nom de produit existe déjà");
                        }
                        break;
                    
                    case "xPrice":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text.Replace(".", ","));
                        }
                        catch
                        {
                            listError = ("Le prix incorrect");
                        }
                        break;
                  
                   
                  
                 

                }
                tb.Foreground = (listError != null) ?
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)) :
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                ((Label)this.FindName("l" + tb.Name)).Foreground = tb.Foreground;
            }

            if (sender is ComboBox)
            {
                ComboBox cb = ((ComboBox)sender);

                listError = cb.SelectedItem == null ? "Vérifier tous les domaines" : null;

                cb.Foreground = (listError != null) ?
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)) :
                    new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                ((Label)this.FindName("l" + cb.Name)).Foreground = cb.Foreground;
            }

           

            return listError;
        }
        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            validTextBox(sender);
        }
        private void xBalance_Click(object sender, RoutedEventArgs e)
        {
            Visibility v = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
            /*xContenance.Visibility = v;
            xUnit_contenance.Visibility = v;
            xTare.Visibility = v;*/

        }
        private bool IsValidTextBox()
        {
            string s = null;

            s += validTextBox(xCodeBar);
            s += validTextBox(xName);
            s += validTextBox(xPrice);
            s += validTextBox(xQTY);
            

         /*   if (xBalance.IsChecked ?? true)
            {
                s += validTextBox(xUnit_contenance);
                s += validTextBox(xContenance);
                s += validTextBox(xTare);
            }*/

            s += validTextBox(xTVA);
            s += validTextBox(xGroup);
            s += validTextBox(xSub_group);
           
            return s == "" ? true : false;
        }
        private Class.ClassProducts.product FormToVar()
        {
            Class.ClassProducts.product p = new Class.ClassProducts.product();
            p.balance = xBalance.IsChecked ?? false;
            p.Desc = xDetails.Text;
            p.CodeBare = xCodeBar.Text;
            p.Name = xName.Text;
            p.price = decimal.Parse( xPrice.Text.Replace(".",",") );
            p.qty = decimal.Parse(xQTY.Text.Replace(".", ","));
            p.grp = int.Parse(xGroup.SelectedValue.ToString());
            p.sgrp = int.Parse(xSub_group.SelectedValue.ToString());
            p.cusumerIdSubGroup = p.sgrp;
            p.tare = int.Parse(xTare.Text == "" ? "0" : xTare.Text);
            p.contenance = decimal.Parse(xContenance.Text == "" ? "0" : xContenance.Text.Replace(".", ","));
            p.uniteContenance = int.Parse(xUnit_contenance.Text == "" ? "0" : xUnit_contenance.Text);
            p.tva = (xTVA.SelectedValue == null) ? -1 : int.Parse(xTVA.SelectedValue.ToString());
          
            p.CustumerId = product.CustumerId;
            p.cusumerIdRealStock = product.cusumerIdRealStock;
            p.ProductsWeb_CustomerId = product.ProductsWeb_CustomerId;

            return p;
        }
        private void modifElm()
        {
            Class.ClassProducts.modif(FormToVar());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
            {
                modifElm();
                this.Close();
            }
            else
            {
              ///  new Class.ClassFunctuon().showMessageSB(listError);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (product != null) loadFromWindow(this.product);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
