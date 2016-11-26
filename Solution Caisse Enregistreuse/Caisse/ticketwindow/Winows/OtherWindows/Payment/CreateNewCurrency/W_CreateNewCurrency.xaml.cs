using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicketWindow.Class;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.Payment.CreateNewCurrency
{
    /// <summary>
    /// Логика взаимодействия для W_CreateNewCurrency.xaml
    /// </summary>
    public partial class WCreateNewCurrency : Window
    {
        public string Sub { get; set; }
        private WGridPay GridPay { get; set; }
        private Button B { get; set; }
        public DAL.Models.Currency TypesPay { get; set; }

        public WCreateNewCurrency()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (B != null)
            {
                B.Background = new SolidColorBrush(xColor.SelectedColor);

                ((Label)((StackPanel)B.Content).FindName(Sub + "lb_" + xName.Content + "x" + yName.Content)).Content = (xCaption.Text);

                B.ToolTip = ((DAL.Models.Currency)cb.SelectedItem).CustomerId;
                ClassGridGroup.Save(B);
                Close();
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            GridPay = Owner as WGridPay;

            cb.ItemsSource = RepositoryCurrency.Currencys.FindAll(l => l.TypesPay == GridPay.TypesPay);

            var name = Sub + "_" + xName.Content + "x" + yName.Content;

            B = (Button)(GridPay.FindName(name));

            if (B != null)
            {
                Guid id;
                if ((B.ToolTip != null)&&(Guid.TryParse( B.ToolTip.ToString(), out id)))
                    cb.SelectedItem = RepositoryCurrency.Currencys.Find(l => l.CustomerId == id);
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
