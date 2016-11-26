using System.Windows;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Divers
{
    /// <summary>
    /// Логика взаимодействия для W_ModifierPrix.xaml
    /// </summary>
    public partial class WModifierPrix : Window
    {
        public object Product;

        public WModifierPrix(object product)
        {
            InitializeComponent();
            Product = product ;
        }


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            xValue.Text = ((ProductType) Product).Price.ToString();
            xNameProduct.Text = ((ProductType) Product).Name;
            numPad.TextBox = xValue;
            numPad.BEnter = xEnter;
        }
        private void XEnterClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (((ProductType) Product).Price == 0.0m)
            {
                var window = ClassEtcFun.FindWindow("MainWindow_") as MainWindow;
                var dg = window.GridProducts;
                var selected = (XElement)dg.SelectedItem;
                if (selected != null)
                    CheckService.DelProductCheck(selected.GetXElementValue("ii").ToInt());
                
            }
            Close();
        }
    }
}
