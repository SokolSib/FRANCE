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

namespace ticketwindow.Winows.DetailsProducts
{
    /// <summary>
    /// Логика взаимодействия для W_DetailsProducts.xaml
    /// </summary>
    public partial class W_DetailsProducts : Window
    {
        public W_DetailsProducts(object DataGrid_)
        {
            InitializeComponent();

            dg.DataContext = ((DataGrid)DataGrid_).DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Class.ClassFunctuon().Click(sender);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_bare_code.Focus();
        }

        private void tb_bare_code_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;
                string bc = tb.Text;
                ClassCheck.delProductCheck(bc);
                dg.DataContext = Class.ClassCheck.b.Element("check");
                CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
                dg.SelectedIndex = dg.Items.Count - 1;
                tb.Text = "";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
