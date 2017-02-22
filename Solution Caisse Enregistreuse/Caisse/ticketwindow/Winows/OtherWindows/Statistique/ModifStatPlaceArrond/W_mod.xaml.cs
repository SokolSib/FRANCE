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
    /// Логика взаимодействия для W_mod.xaml
    /// </summary>
    public partial class WMod : Window
    {
        public List<StatPlaceArrond> Snp;
        public Guid CustomerId { get; set; }

        public WMod()
        {
            InitializeComponent();

        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string name = xNamePlaceArrond.Text;
            string QTY = xQTY.Text;

            if (name.Length == 0)
                FunctionsService.ShowMessageTime("Пустое знаение ");

            else
            {
                if (Snp.FindAll(l => l.NamePlaceArrond == name).Count > 0)
                    FunctionsService.ShowMessageTime("Такое имя сущ-ет ");
                else
                {
                    int qty;

                    if (int.TryParse(QTY, out qty))
                    {
                        int indx = Snp.FindIndex(l => l.CustomerId == this.CustomerId);

                        if (indx != -1)
                        {
                            Snp[indx].NamePlaceArrond = name;
                            Snp[indx].Qty = qty;
                            RepositoryStatPlaceArrond.Update(Snp[indx]);
                        }
                        else
                            FunctionsService.ShowMessageTime("Erroer ");

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
