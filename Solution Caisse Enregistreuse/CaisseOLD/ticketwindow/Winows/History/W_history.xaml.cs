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

namespace ticketwindow.Winows.History
{
    public class MyConverter : IValueConverter
    {
        public object Convert(object o, Type type, object parameter, System.Globalization.CultureInfo culture)
        {

            switch (parameter.ToString())
            {
                case "Date":
                    return DateTime.Parse(o.ToString()).ToString("hh:mm", culture);
                case "Money":
                    return ( decimal.Parse( ((string)o).Replace('.',',') ) ).ToString("C", culture) ;
                default:
                    return o;
            }
        }
        public object ConvertBack(object o, Type type, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Логика взаимодействия для W_history.xaml
    /// </summary>
    public partial class W_history : Window
    {
      
        public W_history()
        {
            InitializeComponent();
            try
            {
                list.DataContext =

                   Class.ClassCheck.x.Element("checks").Elements("check").Reverse();


            }
            catch
            {
               new ClassFunctuon().showMessageTime("Пусто");
            }
        }
         private XElement x;
        private void list_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            XElement s = (XElement)list.SelectedItem;

            x = Class.ClassCheck.x.Element("checks").Elements("check").Where(l => l.Attribute("barcodeCheck").Value == s.Attribute("barcodeCheck").Value).FirstOrDefault();

            listDetails.DataContext = x.Elements("product");

            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
             XElement s = list.SelectedItem as  XElement ;



            if (s != null)
            {
                XDocument x = new XDocument(s);
                

                new Class.ClassPrintCheck(x , true);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
