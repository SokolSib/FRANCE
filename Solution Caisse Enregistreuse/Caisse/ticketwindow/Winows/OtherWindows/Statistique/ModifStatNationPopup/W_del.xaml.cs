using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Statistique.ModifStatNationPopup
{
    /// <summary>
    /// Логика взаимодействия для W_del.xaml
    /// </summary>
    public partial class WDel : Window
    {
        public List<StatNationPopup> Snp;

        public Guid CustomerId { get; set; }

        public WDel()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int indx = Snp.FindIndex(l => l.CustomerId == this.CustomerId);

            if (indx != -1)
            {
                RepositoryStatNationPopup.Delete(Snp[indx]);
                Snp.RemoveAt(indx);
            }
            else FunctionsService.ShowMessageTime("Erroer ");
            
            CollectionViewSource.GetDefaultView(((WGrid) Owner).dataGrid.ItemsSource).Refresh();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WStat))
                {
                    WStat w = (window as WStat);
                    w.Reload();
                }
            }

            Close();
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
