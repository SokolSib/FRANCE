using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.Product
{
    /// <summary>
    /// Логика взаимодействия для W_setProduct.xaml
    /// </summary>
    public partial class WSetProduct : Window
    {
        public int X, y;
 
        public WSetProduct()
        {
            InitializeComponent();

            xDescription.ItemsSource = RepositoryProduct.Products;
            xDescription.ItemFilter += (searchText, obj) =>
                    (searchText.Length > 3) && (obj as ProductType).Name.ToUpper().IndexOf(searchText.ToUpper()) != -1;
        }

        private void ExpanderExpanded(object sender, RoutedEventArgs e)
        {
            statusMes.Content = ("Choisissez une seule produit dans la liste :");
            var val = xDescription.Text;

            if (val != "")
            {
                var lp = RepositoryProduct.GetProductsByName(val.ToUpper());
                list.DataContext = lp;
                CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();

            }
        }

        private void Go(ProductType p)
        {
            var wProduct = Owner as MainWindow;

            Class.ClassGridProduct.Elm el = new Class.ClassGridProduct.Elm
                                            {
                                                Background = new SolidColorBrush(xColor.SelectedColor),
                                                Description = xDescription.Text,
                                                CustomerId = p.CustomerId,
                                                Font = new SolidColorBrush(xFontColor.SelectedColor)
                                            };

            el.X = Convert.ToByte(X);
            el.Y = Convert.ToByte(y);

            var v = ((Button)wProduct.FindName("_b_" + X + "x" + y));

            if (v != null)
            {
                ((TextBlock)v.Content).Text = el.Description;

                v.ToolTip = "Products id=[" + p.CustomerId + "]"; 
                v.Background = new SolidColorBrush(xFontColor.SelectedColor);
                v.Foreground = new SolidColorBrush(xColor.SelectedColor);
                new Class.ClassGridProduct().Save(el, wProduct.I, wProduct.J);
                Close();
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var lp = RepositoryProduct.GetProductsByName(xDescription.Text.ToUpper());

            switch (lp.Count)
            {
                case 0: statusMes.Content = (" Ce produit n'est pas trouvé "); _expander.IsExpanded = true; break;
                case 1: statusMes.Content = (""); Go(lp[0]);  break; 
                default :
                    var pe = list.SelectedItem as ProductType;
                    if (pe == null)
                        _expander.IsExpanded = true;
                    else
                        Go(pe);
                    break;
            }
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
                         Y = Convert.ToByte(y)
                     };

            var v = ((Button)wProduct.FindName("_b_" + X + "x" + y));

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
