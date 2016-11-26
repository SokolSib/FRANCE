using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Product.FindProduct
{
    /// <summary>
    /// Логика взаимодействия для W_Find_Product.xaml
    /// </summary>
    public partial class WFindProduct : Window
    {
        public WFindProduct()
        {
            InitializeComponent();

        }

        private string ValidTextBox(object sender)
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
            ValidTextBox(sender);
        }

        private void XBalanceClick(object sender, RoutedEventArgs e)
        {
            Visibility v = ((xBalance.IsChecked ?? false) ? Visibility.Collapsed : Visibility.Visible);
            spContenance.Visibility = v;
            spUnit_contenance.Visibility = v;
            spTare.Visibility = v;
            tgbTare.Visibility = v;
            tgbUnitContenance.Visibility = v;
            tgbContenance.Visibility = v;

        }
        
        private void ModifElm()
        {
            DataGrid dg = (Owner as WGridProduct).DataGrid;
            dg.ItemsSource = CassieService.ProductsFilter(this);
            CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
            Close();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ModifElm();
        }

        private void TgbClick(object sender, RoutedEventArgs e)
        {
            var b = ((ToggleButton)sender);
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

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
