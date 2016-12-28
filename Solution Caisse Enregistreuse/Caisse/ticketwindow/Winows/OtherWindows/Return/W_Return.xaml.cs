using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
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
                ButtonClick(null, null);
        }

        private static CheckTicket GetCheck(string barcode)
        {
            RepositoryCheck.GetDucument();

            var foundedCheckElement = RepositoryCheck.Document.GetXElements("checks", "check")
                    .FirstOrDefault(c => c.GetXAttributeValue("barcodeCheck") == barcode);

            // Если в текущем чеке нет ищем в прошлом
            if (foundedCheckElement == null)
            {
                var dir = new DirectoryInfo(Path.Combine(Config.AppPath, "Data"));
                foreach (var yearDir in dir.GetDirectories().Where(d=> IsDigitalName(d.Name)).OrderByDescending(p => p.CreationTime))
                {
                    foreach (var monthDir in yearDir.GetDirectories().OrderByDescending(p => p.CreationTime))
                    {
                        foreach (var file in monthDir.GetFiles().OrderByDescending(p => p.CreationTime))
                        {
                            var document = XDocument.Load(file.FullName);

                            foundedCheckElement = document.GetXElements("checks", "check")
                                    .FirstOrDefault(c => c.GetXAttributeValue("barcodeCheck") == barcode);

                            if (foundedCheckElement != null)
                                break;
                        }

                        if (foundedCheckElement != null)
                            break;
                    }

                    if (foundedCheckElement != null)
                        break;
                }

            }

            return foundedCheckElement != null
                ? CheckTicket.FromCheckXElement(foundedCheckElement, Guid.NewGuid(), Guid.NewGuid())
                : null;
        }

        private static bool IsDigitalName(string name)
        {
            var digits=new List<char> {'0','1','2','3','4','5','6','7','8','9'};
            return name.All(c => digits.Contains(c));
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var barCode = codebare.Text.Trim();
            var check = GetCheck(barCode);

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