using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ticketwindow.Class;

namespace ticketwindow.Winows.Payment
{
    /// <summary>
    /// Логика взаимодействия для W_PayCheck.xaml
    /// </summary>
    public partial class W_PayETC : Window
    {
        public ClassSync.TypesPayDB typesPay { get; set; }
        public W_PayETC()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                new Class.ClassFunctuon().Click(sender);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _typesPay.Content = typesPay.Name;
            tbS.Text = ClassBond.residue().ToString();
            this.numPad.textBox = tbS;
            this.numPad.bEnter = xEnter;
            if ( this.Owner is MainWindow) 
            (this.Owner as MainWindow).qty_label.Text = "__";
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            tbS.Focus();
           
        }

        private void InvisibleClick(object sender, RoutedEventArgs e)
        {
            ToggleButton b = ((ToggleButton)sender);
            switch (b.Name)
            {
                case "NumPadVisible": xNumPad.Visibility = b.IsChecked != true ? Visibility.Hidden : Visibility.Visible; break;                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
       
    }
}
