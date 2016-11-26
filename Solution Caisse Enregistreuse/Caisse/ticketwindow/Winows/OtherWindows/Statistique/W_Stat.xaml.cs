using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Statistique
{
    /// <summary>
    /// Логика взаимодействия для W_Stat.xaml
    /// </summary>
    public partial class WStat : Window
    {
        public List<StatNationPopup> A;
        public List<StatPlaceArrond> B;

        public void Reload ()
        {
            RepositoryStatNationPopup.Sync();
            A = RepositoryStatNationPopup.StatNationPopups.OrderByDescending(l => l.Qty).ToList();

            RepositoryStatPlaceArrond.Sync(); 
            B = RepositoryStatPlaceArrond.StatPlaceArronds.OrderByDescending(l => l.Qty).ToList();

            xNation.Items.Clear();
            xPlaceArround.Items.Clear();

            foreach (var elm in A)
                xNation.Items.Add(elm.NameNation + " ... " + elm.Qty);

            foreach (var elm in B)
                xPlaceArround.Items.Add(elm.NamePlaceArrond + " ... " + elm.Qty);

            xNation.SelectedIndex = 0;

            xPlaceArround.SelectedIndex = 0;
        }

        public WStat()
        {
            InitializeComponent();
            Reload();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            string  sia = (string) xNation.SelectedItem;
            string namea = sia.Substring(0, sia.IndexOf("..")-1);
            StatNationPopup fa = A.Find(l => l.NameNation == namea);

            if (fa != null)
            {
                fa.Qty += 1;
                RepositoryStatNationPopup.Update(fa);
            }
            
            string sib = (string)xPlaceArround.SelectedItem;
            string nameb = sib.Substring(0, sib.IndexOf("..") - 1);
            StatPlaceArrond fb = B.Find(l => l.NamePlaceArrond == nameb);

            if (fb != null)
            {
                fb.Qty += 1;
                RepositoryStatPlaceArrond.Update(fb);
            }

            StatNation sn = new StatNation(Guid.NewGuid(),
                DateTime.Now,
                Global.Config.CustomerId,
                fa != null ? fa.CustomerId : Guid.Empty,
                fb != null ? fb.CustomerId : Guid.Empty);
            
            RepositoryStatNation.Add(sn);
            Close();
        }

        private void XbNationClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
