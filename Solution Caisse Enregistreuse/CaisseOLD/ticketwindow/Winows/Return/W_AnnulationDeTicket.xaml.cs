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
using ticketwindow.Class;

namespace ticketwindow.Winows.AnnulationDeTicket
{
    /// <summary>
    /// Логика взаимодействия для W_AnnulationDeTicket.xaml
    /// </summary>
    public partial class W_AnnulationDeTicket : Window
    {
        public W_AnnulationDeTicket()
        {
            InitializeComponent();

            codebare_.Focus();

            codebare_.IsEnabled = false;

            var check = ClassCheck.x.Element("checks").Elements("check").LastOrDefault();

            if (check != null) codebare_.Text = check.Attributes("barcodeCheck").FirstOrDefault().Value;


        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
          
            if ((e.Key == Key.Return))
                Button_Click(this.button, null);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ClassFunctuon().Click(sender);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            codebare_.Focus();
        }

        private void buttonc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.numPad.textBox = codebare_;
            this.numPad.bEnter = button;
        }


        
    }
}
