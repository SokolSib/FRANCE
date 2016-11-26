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

namespace ticketwindow.Winows.Payment.TypePayCreate
{
    /// <summary>
    /// Логика взаимодействия для W_Pay_Create.xaml
    /// </summary>
    public partial class W_Pay_Create : Window
    {
        public string sub { get; set; }
        private W_TypePay TypePay { get; set; }
        private Button b { get; set; }
        public W_Pay_Create()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           if (b != null)
            {
                b.Background = new SolidColorBrush(xColor.SelectedColor);

                b.Content = (this.xCaption.Text);

                b.ToolTip = ((ClassSync.TypesPayDB)cb.SelectedItem).Id;

                new ClassGridGroup().save(b);

                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TypePay = this.Owner as W_TypePay;

            cb.ItemsSource = ClassSync.TypesPayDB.t;

            string name = sub + "_" + this.xName.Content + "x" + this.yName.Content;

            b = (Button)(TypePay.FindName(name));

            if (b != null)
            {

                int indx = -1;

                if (int.TryParse(b.ToolTip == null ? "-1" : b.ToolTip.ToString().Replace("TypesPay",""), out indx))

                    cb.SelectedItem = ClassBond.getTypesPayDBFromId(indx);

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
