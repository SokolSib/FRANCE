using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicketWindow.Class;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.Payment.TypePayCreate
{
    /// <summary>
    /// Логика взаимодействия для W_Pay_Create.xaml
    /// </summary>
    public partial class WPayCreate : Window
    {
        public string Sub { get; set; }
        private WTypePay TypePay { get; set; }
        private Button B { get; set; }

        public WPayCreate()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
           if (B != null)
            {
                B.Background = new SolidColorBrush(xColor.SelectedColor);
                B.Content = (xCaption.Text);
                B.ToolTip = ((TypePay)cb.SelectedItem).Id;
                ClassGridGroup.Save(B);
                Close();
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            TypePay = Owner as WTypePay;
            cb.ItemsSource = RepositoryTypePay.TypePays;

            var name = Sub + "_" + xName.Content + "x" + yName.Content;

            B = (Button)(TypePay.FindName(name));

            if (B != null)
            {
                int indx;
                if (int.TryParse(B.ToolTip == null ? "-1" : B.ToolTip.ToString().Replace("TypesPay",""), out indx))
                    cb.SelectedItem = RepositoryTypePay.GetById(indx);
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
