using System;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.History
{
    /// <summary>
    ///     Логика взаимодействия для W_G_History.xaml
    /// </summary>
    public partial class WgHistory : Window
    {
        public WgHistory()
        {
            InitializeComponent();
            list.DataContext = RepositoryCloseTicketG.Get(Guid.Empty);
        }

        private void ListSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var s = list.SelectedItem as CloseTicketG;
            if (s != null) CassieService.PrintCloseTicket(s.CustomerId);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}