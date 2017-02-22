using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ticketwindow.Winows.addProduct
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private bool isModif;
        Class.ClassBufProducts buf;
        public AddProduct()
        {
            InitializeComponent();
            buf = new Class.ClassBufProducts();
            xContenance.Visibility = Visibility.Hidden;
            xUnit_contenance.Visibility = Visibility.Hidden;
            xTare.Visibility = Visibility.Hidden;
            
        }
        private bool IsValidTextBox()
        {
            string s = null;

            s += validTextBox(xCodeBar);
            s += validTextBox(xName);
            s += validTextBox(xPrice);
            s += validTextBox(xQTY);

            if (!xBalance.IsChecked ?? true)
            {
                s += validTextBox(xUnit_contenance);
                s += validTextBox(xContenance);
                s += validTextBox(xTare);
            }

            return s == "" ? true : false;
        }
        private Class.ClassProducts.product FormToVar ()
        {
            Class.ClassProducts.product p = new Class.ClassProducts.product();
            p.balance = !xBalance.IsChecked ?? false;     
            p.CodeBare = xCodeBar.Text;
            p.Name = xName.Text;
            p.price = decimal.Parse(xPrice.Text);
            p.qty = int.Parse(xQTY.Text);

            try
            {
                p.tare = int.Parse(xTare.Text);
            }

            catch
            {
                p.tare = 0;
            }

            p.tva = (xTVA.SelectedValue == null) ? -1 : int.Parse(xTVA.SelectedValue.ToString());

            try
            {
                p.uniteContenance = int.Parse(xUnit_contenance.Text);
            }

            catch
            {
                p.uniteContenance = 0;
            }
            p.CustumerId = Guid.NewGuid();

            return p;
        }
        private void ClearForm()
        {
            xCodeBar.Text = "";
            xName.Text = "";
            xPrice.Text = "";
            xTVA.SelectedValue = 0;
            xQTY.Text = "";
            xDetails.Text = "";
            xGroup.SelectedValue = 1;
            xSub_group.SelectedValue = 1;
            xBalance.IsChecked = true;
            xContenance.Text = "";
            xUnit_contenance.Text = "";
            xTare.Text = "";
            isModif = false;
            Cancel.Content = "Anuller";
            Save.Content = "Ajouter";
        }
        private void AddElm()
        {
            buf.add(FormToVar());

            dataGrid1.DataContext = null;

            dataGrid1.DataContext = buf.x.Element("Product").Elements("rec");

            ClearForm();
        }
        private void Modif()
        { 
       //     Class.ClassProducts.modif(FormToVar(), buf.x, false);

            dataGrid1.DataContext = null;

            dataGrid1.DataContext = buf.x.Element("Product").Elements("rec");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidTextBox())
                if (isModif)
                    Modif();
                else
                    AddElm();
        }
        private string validTextBox(object sender)
        {
            string listError = null;

            TextBox tb = ((TextBox)sender);

            switch (tb.Name)
            {
                case "xCodeBar":
                    try
                    {
                        if ((Class.ClassProducts.findCodeBar(Int64.Parse(tb.Text).ToString()) != null) || (tb.Text.Length < 5))
                            listError = ("the CodeBare is not correct");
                    }
                    catch
                    {
                        listError = ("the CodeBare is not correct");
                    }
                    break;
                case "xName":
                    if ((Class.ClassProducts.findName(tb.Text) != null) || (tb.Text.Length < 2))
                        listError = ("the Name is not correct");
                    break;
                case "xPrice":
                    try
                    {
                        decimal d = decimal.Parse(tb.Text);
                    }
                    catch
                    {
                        listError = ("the Price is not correct");
                    }
                    break;
                case "xQTY":
                    try
                    {
                        decimal d = decimal.Parse(tb.Text);
                    }
                    catch
                    {
                        listError = ("the QTY is not correct");
                    }
                    break;
                case "xUnit_contenance":
                    try
                    {
                        decimal d = decimal.Parse(tb.Text);
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

            return listError;
        }
        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            validTextBox(sender);
        }
        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            if (buf.pb.Count > 0)
            {
                Class.ClassProducts.add(buf.pb);

                this.Close();
            }
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
        /*    xDetails.Text = p.chp_cat_s;
            xGroup.SelectedValue = p.chp_fam;
            xSub_group.SelectedValue = p.chp_ss_fam;*/
            xBalance.IsChecked = !p.balance;
            xBalance_Click(xBalance, null);
            xContenance.Text = p.contenance.ToString();
            xUnit_contenance.Text = p.uniteContenance.ToString();
            xTare.Text = p.tare.ToString();
            isModif = true;
            Cancel.Content = "Ajouter Product";
            Save.Content = "Modifie Product";
        }
        private void dataGrid1_Selected(object sender, RoutedEventArgs e)
        {
            XElement x = (XElement)dataGrid1.SelectedItem;

            if (x != null) loadFromWindow(Class.ClassProducts.transform(x));
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            Close();
        }
    }
}
