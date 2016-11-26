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

namespace ticketwindow.Winows.Divers
{
    /// <summary>
    /// Логика взаимодействия для W_ModifierPrix.xaml
    /// </summary>
    public partial class W_ModifierPrix : Window
    {
        public object product;

        public W_ModifierPrix(object product)
        {
            InitializeComponent();

            this.product = product ;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            xValue.Text = (this.product as Class.ClassProducts.product).price.ToString();
            xNameProduct.Text = (this.product as Class.ClassProducts.product).Name;
            this.numPad.textBox = xValue;
            this.numPad.bEnter = xEnter;
        }
        private void xEnter_Click(object sender, RoutedEventArgs e)
        {
        
             new Class.ClassFunctuon().Click(sender);
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ( (this.product as Class.ClassProducts.product).price == 0.0m )
            {
                MainWindow mw = Class.ClassETC_fun.findWindow("MainWindow_") as MainWindow;
                DataGrid dg = mw.dataGrid1;
                XElement x = (XElement)dg.SelectedItem;
                if (x != null)
                ticketwindow.Class.ClassCheck.delProductCheck(int.Parse(x.Element("ii").Value));
                
            }
            Close();
        }
    }
}
