using System;
using System.Windows;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.AdminWindows.RoleWindow
{
    /// <summary>
    /// Interaction logic for RolesWindow.xaml
    /// </summary>
    public partial class RolesWindow : Window
    {
        public RolesWindow()
        {
            InitializeComponent();
            DataGrid.ItemsSource = RepositoryAccountRole.AccountRoles;
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            var window = new AddRole();
            if (window.ShowDialog() == true)
            {
                RepositoryAccountRole.AccountRoles.Add(new AccountRole(Guid.NewGuid(), window.RoleName, window.Priveleges));
                RepositoryAccountRole.SaveFile();

                DataGrid.ItemsSource = null;
                DataGrid.ItemsSource = RepositoryAccountRole.AccountRoles;
            }
        }

        private void BtnRedactClick(object sender, RoutedEventArgs e)
        {
            var selected = DataGrid.SelectedItem as AccountRole;

            if (selected != null)
            {
                if (selected.RoleName == RepositoryAccountUser.MainRoleName)
                    MessageBox.Show(Properties.Resources.MessageIsAdmin, Properties.Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    var window = new AddRole(selected);

                    if (window.ShowDialog() == true)
                    {
                        selected.RoleName = window.RoleName;
                        selected.Privelegies = window.Priveleges;
                        RepositoryAccountRole.SaveFile();

                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = RepositoryAccountRole.AccountRoles;
                    }
                }
            }
        }

        private void BtnDelClick(object sender, RoutedEventArgs e)
        {
            var selected = DataGrid.SelectedItem as AccountRole;

            if (selected != null)
            {
                if (selected.RoleName == RepositoryAccountUser.MainRoleName)
                    MessageBox.Show(Properties.Resources.MessageIsAdmin, Properties.Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    var result = MessageBox.Show(Properties.Resources.MessageDeleteQuestion, Properties.Resources.LabelQuestion, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        RepositoryAccountRole.AccountRoles.Remove(selected);
                        RepositoryAccountRole.SaveFile();

                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = RepositoryAccountRole.AccountRoles;
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
