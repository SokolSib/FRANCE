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

namespace ticketwindow.Winows.Return
{
    /// <summary>
    /// Логика взаимодействия для W_Return.xaml
    /// </summary>
    public partial class W_Return : Window
    {
        public W_Return()
        {
            InitializeComponent();
            codebare.Focus();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string bc = ((TextBox)sender).Text.Trim().TrimEnd().TrimStart();

            if ((e.Key == Key.Return) && (bc!=""))
            {
                ClassSync.ClassCloseTicketTmp.ChecksTicket check = getCheck(bc);

                if (check != null)

                    new ClassFunctuon().Click(button, check);
                else
                    new ClassFunctuon().showMessageTime("pas trouvé");

                ((TextBox)sender).Text = "";
            }
        }


        private ClassSync.ClassCloseTicketTmp.ChecksTicket getCheck(string barcode)
        {
            return ClassSync.ClassCloseTicketTmp.findCheck(barcode);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string bc = codebare.Text.Trim().TrimEnd().TrimStart();

            ClassSync.ClassCloseTicketTmp.ChecksTicket check = getCheck(bc);

            if (check != null)

                new ClassFunctuon().Click(button, check);
            else
                new ClassFunctuon().showMessageTime("pas trouvé");

            codebare.Text = "";
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            codebare.Focus();
        }

        private void buttonc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.numPad.textBox = codebare;
            this.numPad.bEnter = button;
        }

    }
}
