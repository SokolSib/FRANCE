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

namespace ticketwindow.Winows.Keyboard
{
    /// <summary>
    /// Логика взаимодействия для W_KeyBoard_NumPad.xaml
    /// </summary>
    public partial class W_KeyBoard_NumPad : Window
    {
        public W_KeyBoard_NumPad()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
