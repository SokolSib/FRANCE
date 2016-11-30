using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicketWindow.DAL.Models;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Product
{
    /// <summary>
    /// Логика взаимодействия для W_setProduct.xaml
    /// </summary>
    public partial class WSetProduct : Window
    {
        public int X, Y;
 
        public WSetProduct()
        {
            InitializeComponent();
        }

        private void Go(ProductType p)
        {
            if (p != null)
            {
                var wProduct = Owner as MainWindow;

                var el = new Class.ClassGridProduct.Elm
                         {
                             Background = new SolidColorBrush(xColor.SelectedColor),
                             Font = new SolidColorBrush(xFontColor.SelectedColor),
                             X = Convert.ToByte(X),
                             Y = Convert.ToByte(Y)
                         };

                if (FindProduct.Product != null)
                {
                    el.Description = FindProduct.Product.Name;
                    el.CustomerId = p.CustomerId;
                }

                var v = (Button) wProduct.FindName("_b_" + X + "x" + Y);

                if (v != null)
                {
                    ((TextBlock) v.Content).Text = el.Description;

                    ProductType product;
                    v.ToolTip = FunctionsTranslateService.GetTranslatedFunctionWithProd(
                        "Products id=[" + p.CustomerId + "]", out product);

                    v.Background = new SolidColorBrush(xColor.SelectedColor);
                    v.Foreground = new SolidColorBrush(xFontColor.SelectedColor);
                    new Class.ClassGridProduct().Save(el, wProduct.I, wProduct.J);
                    Close();
                }
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Go(FindProduct.Product);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonClick2(object sender, RoutedEventArgs e)
        {
            var wProduct = Owner as MainWindow;

            var el = new Class.ClassGridProduct.Elm
                     {
                         CustomerId = Guid.Empty,
                         Description = "",
                         X = Convert.ToByte(X),
                         Y = Convert.ToByte(Y)
                     };

            var v = (Button)wProduct.FindName("_b_" + X + "x" + Y);

            if (v != null)
            {
                ((TextBlock)v.Content).Text = el.Description;
                v.ToolTip = "";
                v.ClearValue(BackgroundProperty);

                new Class.ClassGridProduct().Save(el, wProduct.I, wProduct.J);
                Close();
            }
        }
    }
}
