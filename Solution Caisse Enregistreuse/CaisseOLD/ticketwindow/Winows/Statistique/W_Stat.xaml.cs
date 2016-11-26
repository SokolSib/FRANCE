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
using ticketwindow.Winows.Statistique.ModifStatNationPopup;

namespace ticketwindow.Winows.Statistique
{
    /// <summary>
    /// Логика взаимодействия для W_Stat.xaml
    /// </summary>
    public partial class W_Stat : Window
    {
        public List<Class.ClassSync.Stat.StatNationPopup> A = Class.ClassSync.Stat.StatNationPopup.selAll().OrderByDescending(l => l.QTY).ToList();
        public List<Class.ClassSync.Stat.StatPlaceArrond> B = Class.ClassSync.Stat.StatPlaceArrond.selAll().OrderByDescending(l => l.QTY).ToList();

        public void reload ()
        {
            xNation.Items.Clear();
            xPlaceArround.Items.Clear();

            foreach (var elm in A)
                xNation.Items.Add(elm.NameNation + " ... " + elm.QTY);

            foreach (var elm in B)
                xPlaceArround.Items.Add(elm.NamePlaceArrond + " ... " + elm.QTY);

            xNation.SelectedIndex = 0;

            xPlaceArround.SelectedIndex = 0;
       
        }
        public W_Stat()
        {
            InitializeComponent();
            reload();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string  sia = (string) xNation.SelectedItem;

            string namea = sia.Substring(0, sia.IndexOf("..")-1);

            Class.ClassSync.Stat.StatNationPopup fa = A.Find(l => l.NameNation == namea);

            if (fa != null)
            {
                fa.QTY += 1;

                Class.ClassSync.Stat.StatNationPopup.mod(fa);
            }
            
            string sib = (string)xPlaceArround.SelectedItem;

            string nameb = sib.Substring(0, sib.IndexOf("..") - 1);

            Class.ClassSync.Stat.StatPlaceArrond fb = B.Find(l => l.NamePlaceArrond == nameb);

            if (fb != null)
            {
                fb.QTY += 1;

                Class.ClassSync.Stat.StatPlaceArrond.mod(fb);
            }

            Class.ClassSync.Stat.StatNation sn = new Class.ClassSync.Stat.StatNation();

            sn.CustomerId = Guid.NewGuid();

            sn.Date = DateTime.Now;

            sn.IdArrond = fb != null ? fb.IdCustomer : Guid.Empty;

            sn.IdNation = fa!=null? fa.IdCustomer : Guid.Empty;

            sn.IdCaisse = Class.ClassGlobalVar.CustumerId;

            Class.ClassSync.Stat.StatNation.ins(sn);

            this.Close();
        }

        private void xBNation_Click(object sender, RoutedEventArgs e)
        {
            new Class.ClassFunctuon().Click(sender);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
