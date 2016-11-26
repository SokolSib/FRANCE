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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ticketwindow.Winows.Keyboard
{
    /// <summary>
    /// Логика взаимодействия для W.xaml
    /// </summary>
    public partial class W : Window
    {
        public W()
        {
            InitializeComponent();
        }

      

      
        private void W_keyboard_MouseDown(object sender, MouseButtonEventArgs e)
        {

          

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

            this.Focusable = false;
        }

   
            
    }
}
