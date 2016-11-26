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
using System.Xml.Linq;
using ticketwindow.Class;
using ticketwindow.Winows.Payment.TypePayCreate;

namespace ticketwindow.Winows.Payment
{
    /// <summary>
    /// Логика взаимодействия для W_TypePay.xaml
    /// </summary>
    public partial class W_TypePay : Window
    {
        private void gridLoad(string s, Class.ClassGridGroup.elm[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        Button b = (Button)this.FindName(s + "_" + i + "x" + j);
                        b.Content = grid[i, j].caption;
                        b.Background = grid[i, j].background;
                        b.ToolTip = "TypesPayDynamic" + grid[i, j].func;
                    }
                }
            }
        }
        public W_TypePay()
        {
            InitializeComponent();
         
            gridLoad("c", ClassGridGroup.gridTypePay);
            ClassMoney m = new ClassMoney(Class.ClassCheck.getTotalPrice());
            lblTotal.Content = m.Euro.ToString() + " euro(s) et " + m.Cent.ToString() + " centime(s)";

           

            dataGrid1.DataContext = ClassBond.x.Element("MoneySum");          
        }
        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var we = new W_Pay_Create();

            var b = (Button)sender;

            string[] t = b.Name.Split('x');

            int[] xy = { int.Parse(t[0].Substring(2, t[0].Length - 2)), int.Parse(t[1]) };

            we.xName.Content = xy[0];

            we.yName.Content = xy[1];

            we.xCaption.Text = b.Content == null ? "" : b.Content.ToString();

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
            we.WindowStyle = WindowStyle.None;
            we.AllowsTransparency = true;
            we.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Class.ClassFunctuon().Click(sender);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            

            MainWindow mw = this.Owner as MainWindow;

            mw.qty_label.Text = "__";

            Close();
        }
    }
}
