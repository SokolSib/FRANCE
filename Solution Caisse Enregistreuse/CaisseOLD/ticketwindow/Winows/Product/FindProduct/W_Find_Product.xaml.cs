using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ticketwindow.Winows.Product.FindProduct
{
    /// <summary>
    /// Логика взаимодействия для W_Find_Product.xaml
    /// </summary>
    public partial class W_Find_Product : Window
    {
        public W_Find_Product()
        {
            InitializeComponent();

        }
        private string validTextBox(object sender)
        {
            string listError = null;

            TextBox tb = ((TextBox)sender);

            if (tb.Visibility == Visibility.Visible)
            {
                switch (tb.Name)
                {
                    case "xCodeBar":
                        try
                        {

                            if (tb.Text.Length < 0)
                                listError = ("the CodeBare is not correct");
                        }
                        catch
                        {
                            listError = ("the CodeBare is not correct");
                        }
                        break;
                    case "xName":
                        //   XElement x = (Class.ClassProducts.findName(tb.Text));
                        //     if (((x != null) && (tb.Text.Length < 2)))

                        //        if (x.Element("name").Value == tb.Text)
                        if (tb.Text.TrimEnd().Length == 0)
                            listError = ("the Name is not correct");
                        break;
                    case "xPricea":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the Price is not correct");
                        }
                        break;
                    case "xPriceb":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the Price is not correct");
                        }
                        break;
                    case "xQTYa":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the QTY is not correct");
                        }
                        break;
                      
                    case "xQTYb":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the QTY is not correct");
                        }
                        break;
                    case "xUnit_contenancea":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xUnit_contenance is not correct");
                        }
                        break;
                    case "xUnit_contenanceb":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xUnit_contenance is not correct");
                        }
                        break;

                    case "xContenancea":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xContenance is not correct");
                        }
                        break;
                    case "xContenanceb":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xContenance is not correct");
                        }
                        break;

                    case "xTarea":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
                        }
                        catch
                        {
                            listError = ("the xTare is not correct");
                        }
                        break;
                    case "xTareb":
                        try
                        {
                            decimal d = decimal.Parse(tb.Text);
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
            return listError;
        }
        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            validTextBox(sender);
        }
        private void xBalance_Click(object sender, RoutedEventArgs e)
        {
            Visibility v = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
            spContenance.Visibility = v;
            spUnit_contenance.Visibility = v;
            spTare.Visibility = v;
            tgbTare.Visibility = v;
            tgbUnitContenance.Visibility = v;
            tgbContenance.Visibility = v;

        }
        private bool IsValidTextBox()
        {
            string s = null;

            s += validTextBox(xCodeBar);
            s += validTextBox(xName);
          //  s += validTextBox(xPricea);
            s += validTextBox(xQTYa);

            if (!xBalance.IsChecked ?? true)
            {
                s += validTextBox(xUnit_contenancea);
                s += validTextBox(xContenancea);
                s += validTextBox(xTarea);
            }

            return s == "" ? true : false;
        }
     
        private void modifElm()
        {
            DataGrid dg= (this.Owner as W_Grid_Product).dataGrid1;

            dg.ItemsSource = Class.ClassProducts.filtr(this);

            CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();

            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   if (IsValidTextBox())
            {
                modifElm();

            }
        }

        private void tgb_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton b = ((ToggleButton)sender);
            switch (b.Name)
            {
                case "tgbCodeBar": xCodeBar.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbName": xName.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbPrice": spPrice.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbTVA": xTVA.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbQTY": spQTY.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbDetails": xDetails.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbGroup": xGroup.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbSubGruop": xSub_group.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbBallance": xBalance.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbContenance": spContenance.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbUnitContenance": spUnit_contenance.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
                case "tgbTare": spTare.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
