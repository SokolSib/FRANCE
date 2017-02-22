using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Product
{
    /// <summary>
    /// Логика взаимодействия для W_product.xaml
    /// </summary>
    public partial class WProduct : Window
    {
        public int I, J;

        public WProduct(int I, int J)
        {
            InitializeComponent();

            this.J = J;

            this.I = I;

            for (int i = 0; i < Class.ClassGridProduct.Grid.GetLength(2); i++)
                    {
                        for (int j = 0; j < Class.ClassGridProduct.Grid.GetLength(3); j++)
                        {
                            if (Class.ClassGridProduct.Grid[I, J, i, j] != null)
                            {
                                Button b = (Button)FindName("b_" + i + "x" + j);

                                if (Class.ClassGridProduct.Grid[I, J, i, j].CustomerId == Guid.Empty)
                                    b.ToolTip = "Products";
                                else
                                {
                                    var productData = Class.ClassGridProduct.Grid[I, J, i, j];
                                    b.ToolTip = "Products id=[" + productData.CustomerId + "]";

                                    ((TextBlock)b.Content).Text = productData.Description;
                                    b.Background = productData.Background;
                                    b.Foreground = productData.Font;
                                }
                            }
                        }
                    }
        }

        private void ButtonMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var w = new WSetProduct {Owner = this};
            string[] s = ((Button)sender).Name.Split('x');
            string x = s[0].Substring(2, s[0].Length - 2);
            string y = s[1];

            w.X = int.Parse(x);
            w.Y = int.Parse(y);

            int X = Convert.ToInt16(x);
            int Y = Convert.ToInt16(y);
            var gridElm = Class.ClassGridProduct.Grid;

            if (gridElm[I, J, X, Y] != null)
            {
                w.xColor.Background = gridElm[I, J, X, Y].Background;
                w.xFontColor.Background = gridElm[I, J, X, Y].Font;
                w.FindProduct.FilterBox.Text = gridElm[I, J, X, Y].Description;
            }
            
            w.WindowStyle = WindowStyle.None;
            w.AllowsTransparency = true;
            w.Owner = this;
            w.ShowDialog();
        }
    }
}
