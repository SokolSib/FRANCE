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

namespace ticketwindow.Winows.Discount
{
    /// <summary>
    /// Логика взаимодействия для W_DiscountMini.xaml
    /// </summary>
    public partial class W_DiscountMini : Window
    {
        public W_DiscountMini()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            new ClassFunctuon().Click(sender);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
            {
                bs.Click += Button_Click;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
