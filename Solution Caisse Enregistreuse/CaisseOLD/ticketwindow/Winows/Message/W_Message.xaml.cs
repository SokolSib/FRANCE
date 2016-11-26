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

namespace ticketwindow.Winows.Message
{
    /// <summary>
    /// Логика взаимодействия для W_Message.xaml
    /// </summary>
    public partial class W_Message : Window
    {
        public W_Message()
        {
            InitializeComponent();
        }

        private void bok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new Class.ClassFunctuon().Click(sender);
           
        }

        private void bcan_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
