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
using ticketwindow.Winows.Product.ChangeProduct;

namespace ticketwindow.Winows.Product
{
    /// <summary>
    /// Логика взаимодействия для W_Grid_Product.xaml
    /// </summary>
    public partial class W_Grid_Product : Window
    {
        public W_Grid_Product()
        {
            InitializeComponent();

            ClassSync.ProductDB.setAsynch();

            dataGrid1.DataContext = Class.ClassProducts.x.Element("Product").Elements("rec");
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            XElement es = (XElement)dataGrid1.SelectedItem;

            Button b = (Button)sender;


            if ((es != null) || (b.ToolTip.ToString() == "Add Product") || (b.ToolTip.ToString() == "Find Product"))
            {
                new ClassFunctuon().Click(sender,
                    b.ToolTip.ToString() != "Set Product" ?
                    Class.ClassProducts.transform((XElement)dataGrid1.SelectedItem) :
                    dataGrid1.SelectedItem);
            }
            else
            {
                ClassSync.ProductDB.setAsynch();

            }
        }

        private void xClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
