using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Statistique.ModifStatPlaceArrond
{
    /// <summary>
    /// Логика взаимодействия для W_add.xaml
    /// </summary>
    public partial class WAdd : Window
    {
        public List<StatPlaceArrond> Snp;

        public WAdd()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string name = xNamePlaceArrond.Text;
            string qtyText = xQTY.Text;

            if (name.Length == 0)
                FunctionsService.ShowMessageTime("Пустое знаение ");
            else
            {
                if (Snp.Find(l => l.NamePlaceArrond == name) != null)
                    FunctionsService.ShowMessageTime("Такое имя сущ-ет ");
                else
                {
                    int qty;

                    if (int.TryParse(qtyText, out qty))
                    {
                        var sn = new StatPlaceArrond(Guid.NewGuid(), name, qty);
                        RepositoryStatPlaceArrond.Add(sn);
                        Snp.Add(sn);

                        CollectionViewSource.GetDefaultView((this.Owner as WGrid).dataGrid.ItemsSource).Refresh();

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
