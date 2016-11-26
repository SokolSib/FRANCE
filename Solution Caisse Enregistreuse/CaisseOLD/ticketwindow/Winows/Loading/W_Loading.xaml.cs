using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ticketwindow.Winows.Loading
{
    /// <summary>
    /// Логика взаимодействия для W_Loading.xaml
    /// </summary>
    public partial class W_Loading : Window
    {
        private Window W = new Window();
        public W_Loading(object arg)
        {
            InitializeComponent();
           
        }
    
        public void close ()
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
        private void load()
        {
            
           
        }
        private void showText(string txt)
        {
        }
        private void hideText()
        {
        }


    }
}
