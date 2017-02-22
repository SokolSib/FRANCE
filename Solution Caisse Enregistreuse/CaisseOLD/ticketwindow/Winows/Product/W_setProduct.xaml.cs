
using System;
using System.Collections.Generic;
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

namespace ticketwindow.Winows.Product
{
    /// <summary>
    /// Логика взаимодействия для W_setProduct.xaml
    /// </summary>
    public partial class W_setProduct : Window
    {

        public int x, y;
 
        public W_setProduct()
        {
            InitializeComponent();

            xDescription.ItemsSource = Class.ClassProducts.listProducts;
            xDescription.ItemFilter += (searchText, obj) =>
                    (searchText.Length > 3) && (obj as Class.ClassProducts.product).Name.ToUpper().IndexOf(searchText.ToUpper()) != -1;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            statusMes.Content = ("Choisissez une seule produit dans la liste :");
            string val = xDescription.Text;
            if (val != "")
            {

                List<Class.ClassProducts.product> lp = Class.ClassProducts.getProduct(val.ToUpper());

                list.DataContext = lp;

                CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();

            }
        }

        private void go (Class.ClassProducts.product p)
        {
            MainWindow w_product = this.Owner as MainWindow;

            Class.ClassGridProduct.elm el = new Class.ClassGridProduct.elm();

            el.background = new SolidColorBrush(xColor.SelectedColor);

            el.Description = xDescription.Text;

            el.customerId = p.CustumerId;

            el.font = new SolidColorBrush(xFontColor.SelectedColor); ;

            el.x = Convert.ToByte(x);

            el.y = Convert.ToByte(y);

            Button v = ((Button)w_product.FindName("_b_" + x + "x" + y));

            if (v != null)
            {
                ((TextBlock)v.Content).Text = el.Description;

                v.ToolTip = "Products id=[" + p.CustumerId.ToString() + "]"; 
                v.Background = new SolidColorBrush(xFontColor.SelectedColor);
                v.Foreground = new SolidColorBrush(xColor.SelectedColor);
                new Class.ClassGridProduct().save(el, w_product.I, w_product.J);

                this.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Class.ClassProducts.product>  lp = Class.ClassProducts.getProduct(xDescription.Text.ToUpper());

            switch (lp.Count)
            {
                case 0: statusMes.Content = (" Ce produit n'est pas trouvé "); _expander.IsExpanded = true; break;

                case 1: statusMes.Content = (""); go(lp[0]);  break; 

                default :
                    Class.ClassProducts.product pe = list.SelectedItem as Class.ClassProducts.product;
                    if (pe == null)
                        _expander.IsExpanded = true;
                 

                    else

                        go(pe);
                    
                    break;
                
            }

       
                
         
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /*    W_product w_product = this.Owner as W_product;

                Class.ClassGridProduct.elm el = new Class.ClassGridProduct.elm();

                el.background = new SolidColorBrush(xColor.SelectedColor);


                el.Description = "";

                el.customerId = Guid.Empty;

                //   el.fontColor = new SolidColorBrush(xFontColor.SelectedColor); ;

                el.x = Convert.ToByte(x);

                el.y = Convert.ToByte(y);

                Button v = ((Button)w_product.FindName("b_" + x + "x" + y));

                if (v != null)
                {
                    ((TextBlock)v.Content).Text = el.Description;

                    v.ToolTip = "";
                    v.ClearValue(Button.BackgroundProperty);// new SolidColorBrush(Color.FromArgb(0,255,0,0));

                //    v.Foreground = new SolidColorBrush(xFontColor.SelectedColor);
                    new Class.ClassGridProduct().save(el, w_product.I, w_product.J);

                    this.Close();
                    */

            MainWindow w_product = this.Owner as MainWindow;

            Class.ClassGridProduct.elm el = new Class.ClassGridProduct.elm();



            el.customerId = Guid.Empty;

            el.Description = "";

            el.x = Convert.ToByte(x);




            el.y = Convert.ToByte(y);

            Button v = ((Button)w_product.FindName("_b_" + x + "x" + y));

            if (v != null)
            {
                ((TextBlock)v.Content).Text = el.Description;

                v.ToolTip = "";

                v.ClearValue(Button.BackgroundProperty);

             //   v.Background = new SolidColorBrush(xColor.SelectedColor);
             //   v.Foreground = new SolidColorBrush(xFontColor.SelectedColor);
                new Class.ClassGridProduct().save(el, w_product.I, w_product.J);

                this.Close();
            }
        
        }


    }
}
