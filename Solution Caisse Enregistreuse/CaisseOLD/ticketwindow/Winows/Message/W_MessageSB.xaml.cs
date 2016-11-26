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
    /// Логика взаимодействия для W_MessageSB.xaml
    /// </summary>
    /// 
    public partial class W_MessageSB : Window
    {

        public W_MessageSB()
        {
            InitializeComponent();      

        }

        
        private void bcan_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Class.ClassETC_fun.wm_sound(@"Data\Computer_Error.wav");
            }
        }
    }
}
