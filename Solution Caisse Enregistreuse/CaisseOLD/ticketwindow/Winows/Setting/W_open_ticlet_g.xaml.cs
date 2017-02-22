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

namespace ticketwindow.Winows.Setting
{
    /// <summary>
    /// Логика взаимодействия для W_open_ticlet_g.xaml
    /// </summary>
    public partial class W_open_ticlet_g : Window
    {
        public W_open_ticlet_g(string mes)
        {
            InitializeComponent();
            status.Content += mes;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            if (!Class.ClassSync.General.opn())
            {
                status.Content += Class.ClassSync.General.mess;

               
            }
            else
            {
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
           // new Class.ClassFunctuon().Click(sender);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
