using System.Linq;
using System.Windows;
using System.Windows.Input;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Return
{
    /// <summary>
    ///     Логика взаимодействия для W_AnnulationDeTicket.xaml
    /// </summary>
    public partial class WAnnulationDeTicket : Window
    {
        public WAnnulationDeTicket()
        {
            InitializeComponent();

            codebare_.Focus();
            codebare_.IsEnabled = false;

            RepositoryCheck.GetDucument();
            var check = RepositoryCheck.Document.GetXElements("checks", "check").LastOrDefault();

            if (check != null) codebare_.Text = check.Attributes("barcodeCheck").FirstOrDefault().Value;
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Return))
                ButtonClick(button, null);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            codebare_.Focus();
        }

        private void ButtoncClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            numPad.TextBox = codebare_;
            numPad.BEnter = button;
        }
    }
}