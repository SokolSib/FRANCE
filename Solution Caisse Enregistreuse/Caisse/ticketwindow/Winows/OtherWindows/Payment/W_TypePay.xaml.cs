using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TicketWindow.Class;
using TicketWindow.Classes;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;
using TicketWindow.Winows.OtherWindows.Payment.TypePayCreate;

namespace TicketWindow.Winows.OtherWindows.Payment
{
    /// <summary>
    /// Логика взаимодействия для W_TypePay.xaml
    /// </summary>
    public partial class WTypePay : Window
    {
        private void GridLoad(string s, ClassGridGroup.Elm[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        var b = (Button)this.FindName(s + "_" + i + "x" + j);
                        b.Content = grid[i, j].Caption;
                        b.Background = grid[i, j].Background;
                        b.ToolTip = "TypesPayDynamic" + grid[i, j].Func;
                    }
                }
            }
        }

        public WTypePay()
        {
            InitializeComponent();
         
            GridLoad("c", ClassGridGroup.GridTypePay);
            
            var m = new EuroConverter(CheckService.GetTotalPrice());
            lblTotal.Content = m.Euro + " euro(s) et " + m.Cent + " centime(s)";
            dataGrid1.DataContext = RepositoryCurrencyRelations.Document.Element("MoneySum");          
        }

        private void ButtonMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var we = new WPayCreate();
            var b = (Button)sender;

            string[] t = b.Name.Split('x');

            int[] xy = { int.Parse(t[0].Substring(2, t[0].Length - 2)), int.Parse(t[1]) };

            we.xName.Content = xy[0];
            we.yName.Content = xy[1];
            we.xCaption.Text = b.Content == null ? "" : b.Content.ToString();
            we.xColor.Background = b.Background;

            if (b.ToolTip != null)
                we.cb.SelectedValue = b.ToolTip;

            try
            {
                we.xColor.SelectedColor = (b.Background as SolidColorBrush).Color;
            }
            catch
            {
                // ignored
            }
            we.Owner = this;
            we.Sub = b.Name.Substring(0, 1);
            we.WindowStyle = WindowStyle.None;
            we.AllowsTransparency = true;
            we.ShowDialog();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            var mw = Owner as MainWindow;
            mw.qty_label.Text = "__";
            Close();
        }
    }
}
