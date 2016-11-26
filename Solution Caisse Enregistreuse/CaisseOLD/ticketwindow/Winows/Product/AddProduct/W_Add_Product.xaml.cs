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
using ticketwindow.Class;


namespace ticketwindow.Winows.Product.AddProduct
{
    /// <summary>
    /// Логика взаимодействия для W_AddProduct.xaml
    /// </summary>
    public partial class W_AddProduct : Window
    {

        public W_AddProduct()
        {
            Window w = ClassETC_fun.findWindow("NameW_Grid_Product");

            if (w == null)
            {
                w = new W_Grid_Product();
                w.Show();

            }
            InitializeComponent();
            /*xContenance.Visibility = Visibility.Hidden;
            xUnit_contenance.Visibility = Visibility.Hidden;
            xTare.Visibility = Visibility.Hidden;*/
          
        }
        private string validTextBox(object sender)
        {
            string listError = null;

            if (sender is TextBox)
            {
                TextBox tb = ((TextBox)sender);

                switch (tb.Name)
                {
                    case "xCodeBar":
                        try
                        {
                            XElement x = (Class.ClassProducts.findCodeBar(tb.Text));
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

                        XElement n = (Class.ClassProducts.findName(tb.Text));
                        if (tb.Text.Length < 2)
                            
                            try
                            {
                                if ((n.Element("Name").Value == tb.Text))
                                    listError = ("Nom");
                            }
                            catch
                            {
                                listError = ("Ce Nom de produit existe déjà");
                            }
                        break;
                    
                    case "xPrice":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text.Replace(".",","));
                        }
                        catch
                        {
                            listError = ("Le prix incorrect");
                        }
                        break;
                    
                    case "xQTY":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("QTY incorrect");
                        }
                        break;
                    
                    case "xUnit_contenance":
                        try
                        {
                            int d = int.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xUnit_contenance is not correct");
                        }
                        break;
                    
                    case "xContenance":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xContenance is not correct");
                        }
                        break;
                    case "xTare":
                        try
                        {
                            int d = int.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xTare is not correct");
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

                listError = cb.SelectedItem == null ? "the Cb not correct" : null;

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
           /* xContenance.Visibility = v;
            xUnit_contenance.Visibility = v;
            xTare.Visibility = v;*/

        }
        private bool IsValidTextBox()
        {
            string s = null;

            s += validTextBox(xCodeBar);
            s += validTextBox(xName);
            s += validTextBox(xPrice);
           // s += validTextBox(xQTY);

            if (xBalance.IsChecked ?? true)
            {
               /* s += validTextBox(xUnit_contenance);
                s += validTextBox(xContenance);
                s += validTextBox(xTare);*/
            }
            s += validTextBox(xTVA);
            s += validTextBox(xGroup);
            s += validTextBox(xSub_group);
            return s == "" ? true : false;
        }
        private Class.ClassProducts.product FormToVar()
        {
            Class.ClassProducts.product p = new Class.ClassProducts.product();
            p.balance = xBalance.IsChecked ?? false;
            //p.Desc = xDetails.Text;
            p.CodeBare = xCodeBar.Text;
            p.Name = xName.Text;
            p.price = decimal.Parse(xPrice.Text.Replace(".", ","));
           // p.qty = decimal.Parse(xQTY.Text);
            p.grp = int.Parse(xGroup.SelectedValue.ToString());
            p.sgrp = int.Parse(xSub_group.SelectedValue.ToString());
            p.cusumerIdSubGroup = p.sgrp;
            p.tare = 0;
            p.tva = (xTVA.SelectedValue == null) ? -1 : int.Parse(xTVA.SelectedValue.ToString());
            p.uniteContenance = 0;
            p.CustumerId =  Guid.NewGuid();
            p.ProductsWeb_CustomerId = Guid.NewGuid();
            p.cusumerIdRealStock = Guid.NewGuid();
            p.Desc = "";
            p.qty = 0;
            return p;
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
        private void AddElm()
        {
             Class.ClassProducts.add( new List<Class.ClassProducts.product>() { FormToVar()});
             
             DataGrid dg = (GetParents(this, 0)).dataGrid1;

             this.Close();

             CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
                   AddElm();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
