using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.AdminWindows.UsersWindow
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();

            RoleBox.ItemsSource = RepositoryAccountRole.AccountRoles;
            RoleBox.SelectedItem = RepositoryAccountRole.AccountRoles[0];
        }

        public AddUser(AccountUser user)
            : this()
        {
            Fio = user.Fio;
            Login = user.Login;
            Password = user.Password;
            Role = user.Role;
            Pin = user.PinCode;
            BtnAdd.Content = Properties.Resources.BtnRedact;
        }

        public string Fio
        {
            get { return FioBox.Text; }
            set { FioBox.Text = value; }
        }

        public string Login
        {
            get { return LoginBox.Text; }
            set { LoginBox.Text = value; }
        }

        public string Password
        {
            get { return PasswordBox.Text; }
            set { PasswordBox.Text = value; }
        }

        public string Pin
        {
            get { return PinBox.Text; }
            set { PinBox.Text = value ?? string.Empty; }
        }

        public AccountRole Role
        {
            get { return (AccountRole)RoleBox.SelectedItem; }
            set { RoleBox.SelectedItem = value; }
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Fio) || string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)) return;
            if (Role.IsPermiss(Privelege.DeleteProductFromCheck) && PinBox.Text.Length == 0) return;
            
            DialogResult = true;
        }

        private void RoleBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var visibilityPin = Role.IsPermiss(Privelege.DeleteProductFromCheck) ? Visibility.Visible : Visibility.Collapsed;
            PinBox.Visibility = visibilityPin;
            PinLabel.Visibility = visibilityPin;
        }
    }
}
