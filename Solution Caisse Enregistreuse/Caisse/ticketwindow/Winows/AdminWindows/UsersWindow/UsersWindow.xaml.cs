using System;
using System.Windows;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.AdminWindows.UsersWindow
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        public UsersWindow()
        {
            InitializeComponent();
            DataGrid.ItemsSource = RepositoryAccountUser.AccountUsers;
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            var window = new AddUser();
            if (window.ShowDialog() == true)
            {
                RepositoryAccountUser.AccountUsers.Add(new AccountUser(Guid.NewGuid(), window.Fio, window.Login, window.Password, window.Role.CustomerId, window.Pin));
                RepositoryAccountUser.SaveFile();

                DataGrid.ItemsSource = null;
                DataGrid.ItemsSource = RepositoryAccountUser.AccountUsers;
            }
        }

        private void BtnRedactClick(object sender, RoutedEventArgs e)
        {
            var selected = DataGrid.SelectedItem as AccountUser;

            if (selected != null)
            {
                if (selected.Role.RoleName == RepositoryAccountUser.MainRoleName)
                    MessageBox.Show(Properties.Resources.MessageIsAdmin, Properties.Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    var window = new AddUser(selected);

                    if (window.ShowDialog() == true)
                    {
                        selected.Fio = window.Fio;
                        selected.Login = window.Login;
                        selected.Password = window.Password;
                        selected.Role = window.Role;
                        RepositoryAccountRole.SaveFile();

                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = RepositoryAccountUser.AccountUsers;
                    }
                }
            }
        }

        private void BtnDelClick(object sender, RoutedEventArgs e)
        {
            var selected = DataGrid.SelectedItem as AccountUser;

            if (selected != null)
            {
                if (selected.Role.RoleName == RepositoryAccountUser.MainRoleName)
                    MessageBox.Show(Properties.Resources.MessageIsAdmin, Properties.Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    var result = MessageBox.Show(Properties.Resources.MessageDeleteQuestion, Properties.Resources.LabelQuestion, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        RepositoryAccountUser.AccountUsers.Remove(selected);
                        RepositoryAccountUser.SaveFile();

                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = RepositoryAccountUser.AccountUsers;
                    }
                }
            }
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
