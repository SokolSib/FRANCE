using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Services;
using TicketWindow.Winows.OtherWindows.Message;

namespace TicketWindow.Winows.OtherWindows.Return
{
    /// <summary>
    ///     Логика взаимодействия для W_Return.xaml
    /// </summary>
    public partial class WReturn : Window
    {
        public WReturn()
        {
            InitializeComponent();
            codebare.Focus();
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            var bc = ((TextBox) sender).Text.Trim().TrimEnd().TrimStart();

            if ((e.Key == Key.Return) && (bc != ""))
            {
                var check = GetCheck(bc);

                if (check != null)
                    FunctionsService.Click(button, check);
                else
                    FunctionsService.ShowMessageTime("pas trouvé");

                ((TextBox) sender).Text = "";
            }
        }

        private static CheckTicket GetCheck(string barcode)
        {
            RepositoryCheck.GetDucument();

            var foundedCheckElement = RepositoryCheck.Document.GetXElements("checks", "check")
                    .FirstOrDefault(c => c.GetXAttributeValue("barcodeCheck") == barcode);

            return foundedCheckElement != null
                ? CheckTicket.FromCheckXElement(foundedCheckElement, Guid.NewGuid(), Guid.NewGuid())
                : null;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var bc = codebare.Text.Trim();
            var check = GetCheck(bc);

            if (check != null)
                FunctionsService.Click(button, check);
            else
                FunctionsService.ShowMessageTime(Properties.Resources.LabelNofFound);

            codebare.Text = "";
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            codebare.Focus();
        }

        private void ButtoncClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            numPad.TextBox = codebare;
            numPad.BEnter = button;
        }
    }
}