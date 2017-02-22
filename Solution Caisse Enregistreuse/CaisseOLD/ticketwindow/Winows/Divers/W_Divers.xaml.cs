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

namespace ticketwindow.Winows.Divers
{
    /// <summary>
    /// Логика взаимодействия для W_Divers.xaml
    /// </summary>
    public partial class W_Divers : Window
    {
        public W_Divers()
        {
            InitializeComponent();
        }

        public int tva = 1;

        public decimal prix = 0.0m;

        public bool ballance = false;

        public string nameProduct = "";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.numPad.textBox = xValue;
            this.numPad.bEnter = xEnter;

        }

        private void xEnter_Click(object sender, RoutedEventArgs e)
        {
           
          
                new Class.ClassFunctuon().Click(sender
                    );

          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
