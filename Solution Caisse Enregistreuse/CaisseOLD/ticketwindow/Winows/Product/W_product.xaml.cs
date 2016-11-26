using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using ticketwindow.Winows.Product;

namespace ticketwindow
{
    /// <summary>
    /// Логика взаимодействия для W_product.xaml
    /// </summary>
    public partial class W_product : Window
    {

        public int I, J;

        public W_product(int I, int J)
        {
            InitializeComponent();

            this.J = J;

            this.I = I;

            for (int i = 0; i < Class.ClassGridProduct.grid.GetLength(2); i++)
                    {
                        for (int j = 0; j < Class.ClassGridProduct.grid.GetLength(3); j++)
                        {
                            if (Class.ClassGridProduct.grid[I, J, i, j] != null)
                            {
                                Button b = (Button)this.FindName("b_" + i + "x" + j);

                                if (Class.ClassGridProduct.grid[I, J, i, j].customerId == Guid.Empty)
                                {

                                    b.ToolTip = "Products";
                                }
                                else
                                {
                                    b.ToolTip = "Products id=[" + Class.ClassGridProduct.grid[I, J, i, j].customerId.ToString() + "]";

                                    ((TextBlock)b.Content).Text = Class.ClassGridProduct.grid[I, J, i, j].Description;
                                    b.Background = Class.ClassGridProduct.grid[I, J, i, j].background;
                                    b.Foreground = Class.ClassGridProduct.grid[I, J, i, j].font;
                                }
                            }
                        }
                    }
        }


        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            W_setProduct w = new W_setProduct();
            w.Owner = this;
            string[] s = ((Button)sender).Name.Split('x');
            string x = s[0].Substring(2, s[0].Length - 2);
            string y = s[1];

            w.x = int.Parse(x);

            w.y = int.Parse(y);


            int X = Convert.ToInt16(x);
            int Y = Convert.ToInt16(y);
            var gridElm = Class.ClassGridProduct.grid;

            if (gridElm[I, J, X, Y] != null)
            {
                w.xColor.Background = gridElm[I, J, X, Y].background;
                w.xFontColor.Background = gridElm[I, J, X, Y].font;
                w.xDescription.Text = gridElm[I, J, X, Y].Description;

              
            }


            w.WindowStyle = WindowStyle.None;
            w.AllowsTransparency = true;
            w.Owner = this;
            w.ShowDialog();

        }


        private void Click(object sender, RoutedEventArgs e)
        {
            
            new Class.ClassFunctuon().Click(sender);
        }
    }
}
