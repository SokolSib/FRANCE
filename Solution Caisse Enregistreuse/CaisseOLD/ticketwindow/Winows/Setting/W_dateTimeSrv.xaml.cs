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
    /// Логика взаимодействия для W_dateTimeSrv.xaml
    /// </summary>
    public partial class W_dateTimeSrv : Window
    {
        public W_dateTimeSrv(string mess)
        {
            InitializeComponent();

            status.Content = mess;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassSync.ClassDataTimeSrv.getDateTimeFromSrv();
            
            if (!Class.ClassSync.ClassDataTimeSrv.setDateTimeFromSrv())
            {
                status.Content += "Erreur - la date n'a pas changé! Redémarre dans le mode administrateur" + Environment.NewLine;
            }
            else
            {
                status.Content += "La date été mise à jour" + Environment.NewLine + "L'heure actuelle du Caisse : " + DateTime.Now;
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
