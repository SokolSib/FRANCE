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

namespace ticketwindow.Winows.Payment.CreateNewCurrency
{
    /// <summary>
    /// Логика взаимодействия для W_CreateNewCurrency.xaml
    /// </summary>
    public partial class W_CreateNewCurrency : Window
    {
        public string sub { get; set; }
        private W_GridPay GridPay { get; set; }
        private Button b { get; set; }
        public ClassSync.Currency typesPay { get; set; }
        public W_CreateNewCurrency()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (b != null)
            {
                b.Background = new SolidColorBrush(xColor.SelectedColor);

                ((Label)((StackPanel)b.Content).FindName(sub + "lb_" + this.xName.Content + "x" + this.yName.Content)).Content = (this.xCaption.Text);

                b.ToolTip = ((ClassSync.Currency)cb.SelectedItem).CustomerId;

                new ClassGridGroup().save(b);

                this.Close();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GridPay = this.Owner as W_GridPay;

            cb.ItemsSource = ClassSync.Currency.List_Currency.FindAll(l=>l.TypesPay == GridPay.typesPay);

            string name = sub + "_" + this.xName.Content + "x" + this.yName.Content;

            b = (Button)(GridPay.FindName(name));

            if (b != null)
            {
                Guid id ;

                if ((b.ToolTip != null)&&(Guid.TryParse( b.ToolTip.ToString(), out id)))
              
                cb.SelectedItem = ClassSync.Currency.List_Currency.Find(l=>l.CustomerId == id );

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
