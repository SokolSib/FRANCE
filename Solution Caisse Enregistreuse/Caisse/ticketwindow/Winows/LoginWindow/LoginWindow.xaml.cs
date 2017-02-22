using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;

namespace TicketWindow.Winows.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var langItem = LanguageBox.Items.Cast<ComboBoxItem>().FirstOrDefault(m => m.Tag.ToString() == Config.Language);
            LanguageBox.SelectedItem = langItem ?? DefoultLanguageItem;

#if DEBUG
            LoginBox.Text = "admin";
            PasswordBox.Text = "admin";
#endif
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!RepositoryAccountUser.Login(LoginBox.Text, PasswordBox.Text))
            {
                ErrorBox.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorBox.Visibility = Visibility.Collapsed;
                DialogResult = true;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoginBox.Focus();
        }

        private void LanguageBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var language = ((ComboBoxItem) LanguageBox.SelectedItem).Tag.ToString();
            SetLanguage(new CultureInfo(language));
            Config.Language = language;
        }

        private void SetLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Properties.Resources.Culture = culture;

            LabelLanguage.Text = Properties.Resources.MenuLanguage;
            LabelLogin.Text = Properties.Resources.LabelLogin;
            LabelPassword.Text = Properties.Resources.LabelPassword;
            BtnLogon.Content = Properties.Resources.BtnLogon;
            BtnClose.Content = Properties.Resources.BtnClose;
            ErrorBox.Text = Properties.Resources.LabelLogonError;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
