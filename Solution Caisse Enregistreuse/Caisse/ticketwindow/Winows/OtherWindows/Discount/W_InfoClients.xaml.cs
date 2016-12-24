using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicketWindow.DAL.Repositories;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Discount
{
    /// <summary>
    ///     Логика взаимодействия для W_InfoClients.xaml
    /// </summary>
    public partial class WInfoClients : Window
    {
        public WInfoClients()
        {
            InitializeComponent();
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            ebarcode.Focus();
        }

        private void EbarcodeKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var discountCard = RepositoryDiscountCard.GetOneByNumber(ebarcode.Text);

                if (discountCard != null)
                {
                    var ic = RepositoryDiscount.GetClientInfoById(discountCard.InfoClientsCustomerId);
                    if (ic != null)
                    {
                        lTypeClient.Content = ic.TypeClient;
                        lName.Content = ic.Name;
                        lSurname.Content = ic.Surname;
                        lNameCompany.Content = ic.NameCompany;
                        lSIRET.Content = ic.Siret;
                        lFRTVA.Content = ic.Frtva;
                        lOfficeAddress.Content = ic.OfficeAddress;
                        lOfficeZipCode.Content = ic.OfficeZipCode;
                        lOfficeCity.Content = ic.OfficeCity;
                        lHomeAddress.Content = ic.HomeAddress;
                        lHomeZipCode.Content = ic.HomeZipCode;
                        lHomeCity.Content = ic.HomeCity;
                        lTelephone.Content = ic.Telephone;
                        lMail.Content = ic.Mail;
                        lnumberCard.Content = ic.DiscountCards.First().NumberCard;
                        lpoints.Content = ic.DiscountCards.First().Points;
                        lActive.Content = ic.DiscountCards.First().IsActive;
                    }
                }
                else
                    foreach (var la in ClassEtcFun.FindVisualChildren<Label>(this))
                    {
                        la.Content = Properties.Resources.LabelNofFound;
                        break;
                    }
                ebarcode.Text = "";
                
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}