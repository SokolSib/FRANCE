using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TicketWindow.Class;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;
using TicketWindow.Winows.OtherWindows.Payment.CreateNewCurrency;

namespace TicketWindow.Winows.OtherWindows.Payment
{
    /// <summary>
    /// Логика взаимодействия для W_GridPay.xaml
    /// </summary>
    public partial class WGridPay : Window
    {
        public TypePay TypesPay { get; set; }
        public string CountCurrency{get;set;}

        private void GridLoad(string s, ClassGridGroup.Elm[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        var b = (Button)FindName(s  + "_" + i + "x" + j);

                        ((Label)((StackPanel)b.Content).FindName(s + "lb_" + i + "x" + j)).Content = grid[i, j].Caption;
                        b.Background = grid[i, j].Background;
                        b.Tag = grid[i, j].Func;
                    }
                }
            }
        }

        public WGridPay()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var we = new WCreateNewCurrency();
            var b = (Button)sender;
            string[] t = b.Name.Split('x');

            int[] xy = { int.Parse(t[0].Substring(2, t[0].Length - 2)), int.Parse(t[1]) };

            we.xName.Content = xy[0];
            we.yName.Content = xy[1];

            var l = ((Label)((StackPanel)b.Content).FindName( "mlb_" + xy[0] + "x" + xy[1]));

            we.xCaption.Text = l.Content ==null ? "" : l.Content.ToString();
            we.xColor.Background = b.Background;

            if (b.Tag != null)
                we.cb.SelectedValue = b.Tag;
            
            try
            {
                we.xColor.SelectedColor = ((SolidColorBrush) b.Background).Color;
            }
            catch
            {
                // ignored
            }
            we.Owner = this;
            we.Sub = b.Name.Substring(0, 1);
           
            we.ShowDialog();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            GridLoad("m", ClassGridGroup.GridCurrencyPathGetPath(TypesPay));

            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;

            var owner = Owner as MainWindow;
            if (owner != null)
            {
                var cc = RepositoryCurrencyRelations.Transform(FunctionsService.GetMoney((this.Owner as MainWindow).qty_label), this.TypesPay);
             
                foreach (var h in cc)
                    FunctionsService.AddCurrency(h.Currency, h.Count, this);

                owner.qty_label.Text = "__";
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }
}
