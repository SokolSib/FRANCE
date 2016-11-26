using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ticketwindow.Class;

namespace ticketwindow.Winows.Discount
{
    /// <summary>
    /// Логика взаимодействия для W_InfoClients.xaml
    /// </summary>
    public partial class W_InfoClients : Window
    {
        public W_InfoClients()
        {
            InitializeComponent();
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            ebarcode.Focus();
        }

        private void ebarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ClassSync.Discount.DiscountCards d = ClassSync.Discount.DiscountCards.OneDiscountCards(ebarcode.Text);

                ClassSync.Discount.InfoClients ic = ClassDiscounts.getInfClt(d.InfoClients_custumerId);
                if (ic != null)
                {
                    lTypeClient.Content = ic.TypeClient;
                    lName.Content = ic.Name;
                    lSurname.Content = ic.Surname;
                    lNameCompany.Content = ic.NameCompany;
                    lSIRET.Content = ic.SIRET;
                    lFRTVA.Content = ic.FRTVA;
                    lOfficeAddress.Content = ic.OfficeAddress;
                    lOfficeZipCode.Content = ic.OfficeZipCode;
                    lOfficeCity.Content = ic.OfficeCity;
                    lHomeAddress.Content = ic.HomeAddress;
                    lHomeZipCode.Content = ic.HomeZipCode;
                    lHomeCity.Content = ic.HomeCity;
                    lTelephone.Content = ic.Telephone;
                    lMail.Content = ic.Mail;
                    lnumberCard.Content = ic.DiscountCards.First().numberCard;
                    lpoints.Content = ic.DiscountCards.First().points;
                    lActive.Content = ic.DiscountCards.First().active;
                }
                else
                {
                    foreach (Label la in ClassETC_fun.FindVisualChildren<Label>(this))
                    {
                        la.Content = "Не найден";
                    }
                }
                ebarcode.Text = "";
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
