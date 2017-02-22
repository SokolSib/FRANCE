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
    /// Логика взаимодействия для W_add.xaml
    /// </summary>
    public partial class WAdd : Window
    {
        public List<StatNationPopup> Snp;

        public WAdd()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var name = xNameNation.Text;
            var qtyText = xQTY.Text;

            if (name.Length == 0) 
                FunctionsService.ShowMessageTime("Пустое знаение ");
            else
            {
                if (Snp.Find (l=>l.NameNation == name) != null)
                    FunctionsService.ShowMessageTime("Такое имя сущ-ет ");
                else
                {
                    int qty = 0;

                    if (int.TryParse(qtyText, out qty))
                    {
                        var sn = new StatNationPopup(Guid.NewGuid(), name, qty);
                        RepositoryStatNationPopup.Add(sn);
                        Snp.Add(sn);

                        CollectionViewSource.GetDefaultView((Owner as WGrid).dataGrid.ItemsSource).Refresh();

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
                    else
                        FunctionsService.ShowMessageTime("Неверое значние поля QTY ");
                }
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
