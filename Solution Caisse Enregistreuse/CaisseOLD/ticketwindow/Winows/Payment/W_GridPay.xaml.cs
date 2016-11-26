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
using ticketwindow.Class;
using ticketwindow.Winows.Payment.CreateNewCurrency;

namespace ticketwindow.Winows.Payment
{
    /// <summary>
    /// Логика взаимодействия для W_GridPay.xaml
    /// </summary>
    public partial class W_GridPay : Window
    {
        public ClassSync.TypesPayDB typesPay { get; set; }
        public string countCurrency{get;set;}
        private void gridLoad(string s, Class.ClassGridGroup.elm[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        Button b = (Button)this.FindName(s  + "_" + i + "x" + j);

                        ((Label)((StackPanel)b.Content).FindName(s + "lb_" + i + "x" + j)).Content = grid[i, j].caption;
                        b.Background = grid[i, j].background;
                        b.ToolTip = grid[i, j].func;
                    }
                }
            }
        }

        public W_GridPay()
        {
            InitializeComponent();
           
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ClassFunctuon().Click(sender);
        }
        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var we = new W_CreateNewCurrency();

            var b = (Button)sender;

            string[] t = b.Name.Split('x');

            int[] xy = { int.Parse(t[0].Substring(2, t[0].Length - 2)), int.Parse(t[1]) };

            we.xName.Content = xy[0];

            we.yName.Content = xy[1];

            Label l = ((Label)((StackPanel)b.Content).FindName( "mlb_" + xy[0] + "x" + xy[1]));

            we.xCaption.Text = l.Content ==null ? "" : l.Content.ToString();

            we.xColor.Background = b.Background;

            if (b.ToolTip != null)
            {
                we.cb.SelectedValue = b.ToolTip;
            }
            else
            {
                // we.setSelected(we.xGBFunction, "Product");
            }
            try
            {
                we.xColor.SelectedColor = (b.Background as SolidColorBrush).Color;
            }

            catch
            {

            }
            we.Owner = this;
            we.sub = b.Name.Substring(0, 1);
         //   we.WindowStyle = WindowStyle.None;
         //   we.AllowsTransparency = true;
           
            we.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridLoad("m", ClassGridGroup.gridCurrencyPath_get_path(typesPay));
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
            {
                bs.Click += Button_Click;
            }

            if (this.Owner is MainWindow)
            {


                List<ClassBond.count_Currency> cc = ClassBond.transform(new ClassFunctuon().getMoney((this.Owner as MainWindow).qty_label), this.typesPay);
             
                foreach (var h in cc)
                    new ClassFunctuon().add_Currency(h.currency, h.count, this);



                (this.Owner as MainWindow).qty_label.Text = "__";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

       
    }
}
