using System;
using System.Windows;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.Winows.AdminWindows.TvaWindow
{
    /// <summary>
    /// Interaction logic for TvasesWindow.xaml
    /// </summary>
    public partial class TvasesWindow : Window
    {
        public TvasesWindow()
        {
            InitializeComponent();
            DataContext = RepositoryTva.Tvases;
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            var window = new TextWindow(false, Properties.Resources.LabelVat);
            if (window.ShowDialog() == true)
            {
                var tva = new Tva(Guid.NewGuid(), RepositoryTva.Tvases.Count + 1, window.NameText.ToDecimal());
                RepositoryTva.Add(tva);

                DataGridGroups.ItemsSource = null;
                DataGridGroups.ItemsSource = RepositoryTva.Tvases;
            }
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
