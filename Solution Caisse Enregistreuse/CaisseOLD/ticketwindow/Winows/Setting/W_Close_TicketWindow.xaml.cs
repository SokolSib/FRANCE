using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для W_Close_TicketWindow.xaml
    /// </summary>
    public partial class W_Close_TicketWindow : Window
    {
        public W_Close_TicketWindow( string str)
        {
            str += Environment.NewLine + 
                "Caisse : " + ClassGlobalVar.nameTicket + Environment.NewLine +
                "Post : " + ClassGlobalVar.numberTicket + Environment.NewLine +
                "Nom d'usager : " + ClassGlobalVar.user + Environment.NewLine + Environment.NewLine +
                "--------------------------------" + Environment.NewLine +
                "La clé d'ouverture générale : " + ClassGlobalVar.TicketWindowG + Environment.NewLine +
                "La clé d'ouverture local : " + ClassGlobalVar.TicketWindow + Environment.NewLine; 
            InitializeComponent();
            errorlist.Text = str;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
            {
                bs.Click += Button_Click;
            }

         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
                new ClassFunctuon().Click(sender);
               
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
