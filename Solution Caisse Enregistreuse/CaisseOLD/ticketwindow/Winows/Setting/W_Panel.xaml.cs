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

namespace ticketwindow.Winows.Setting
{
    /// <summary>
    /// Логика взаимодействия для W_Panel.xaml
    /// </summary>
    public partial class W_Panel : Window
    {
        public W_Panel()
        {
            InitializeComponent();

            this.Topmost = true;
               
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        
            this.Left = 0;
            this.Top = 0;
        }
    }
}
