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

namespace ticketwindow.Winows.Statistique.ModifStatNationPopup
{
    /// <summary>
    /// Логика взаимодействия для W_del.xaml
    /// </summary>
    public partial class W_del : Window
    {
        public List<Class.ClassSync.Stat.StatNationPopup> SNP;

        public Guid CustomerId { get; set; }

        public W_del()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {



            int indx = SNP.FindIndex(l => l.IdCustomer == this.CustomerId);

            if (indx != -1)
            {

                Class.ClassSync.Stat.StatNationPopup.del(SNP[indx]);

                SNP.RemoveAt(indx);
            }
            else
            {
                new Class.ClassFunctuon().showMessageTime("Erroer ");
            }

           // if (this.Owner != null)
                CollectionViewSource.GetDefaultView((this.Owner as W_Grid).dataGrid.ItemsSource).Refresh();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(W_Stat))
                {
                    W_Stat w = (window as W_Stat);

                    w.reload();
                }
            }

            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
