using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using UserControl = System.Windows.Controls.UserControl;

namespace TicketWindow.Controls
{
    /// <summary>
    /// Логика взаимодействия для InfoClientControl.xaml
    /// </summary>
    public partial class ClientInfoControl : UserControl
    {
        public ClientInfoControl()
        {
            InitializeComponent();
        }

        public void SetClientInfo(ClientInfo info)
        {
            if (info != null)
            {
                BoxName.Text = info.Name;
                BoxSurname.Text = info.Surname;
                BoxNameCompany.Text = info.NameCompany;
                BoxInn.Text = info.Siret;
                BoxFrtva.Text = info.Frtva;
                BoxOfficeAddress.Text = info.OfficeAddress;
                BoxOfficeZipCode.Text = info.OfficeZipCode;
                BoxOfficeCity.Text = info.OfficeCity;
                BoxHomeAddress.Text = info.HomeAddress;
                BoxHomeZipCode.Text = info.HomeZipCode;
                BoxHomeCity.Text = info.HomeCity;
                BoxTelephone.Text = info.Telephone;
                BoxMail.Text = info.Mail;
                BoxNumberCard.Text = info.DiscountCards.First().NumberCard;
                BoxPoints.Text = info.DiscountCards.First().Points.ToString();

                if (info.DiscountCards.First().IsActive)
                {
                    BoxActive.Text = Properties.Resources.LabelIsActiveTrue;
                    BoxActive.Foreground = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    BoxActive.Text = Properties.Resources.LabelIsActiveFalse;
                    BoxActive.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        public ClientInfo GetClientInfo(DiscountCard card)
        {
            //Sex 
            //Password
            //CountrysCustomerId
            //FavoritesProductAutoCustomerId
            //Nclient
            var info = new ClientInfo(card.InfoClientsCustomerId, 1, 1, BoxName.Text, BoxSurname.Text, BoxNameCompany.Text,
                           BoxInn.Text, BoxFrtva.Text, BoxOfficeAddress.Text, BoxOfficeZipCode.Text,
                           BoxOfficeCity.Text, BoxHomeAddress.Text, BoxHomeZipCode.Text, BoxHomeCity.Text,
                           BoxTelephone.Text, BoxMail.Text, "", Guid.Empty, Guid.Empty, "", card.InfoClientsCustomerId);

            if (info.DiscountCards.Count == 0)
                info.DiscountCards.Add(card);

            return info;
        }

        public DiscountCard GetDiscountCard(DiscountCard card)
        {
            card.Points = BoxPoints.Text.ToInt();
            return card;
        }

        public bool Validate()
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(BoxInn.Text))
            {
                long valueLong;
                if (!long.TryParse(BoxInn.Text.Trim(), out valueLong))
                    result += Environment.NewLine + LabelInn.Text + " " + Properties.Resources.LabelIncorrect;
            }

            //if (!string.IsNullOrEmpty(BoxNumberCard.Text))
            //{
            //    long valueLong;
            //    if (!long.TryParse(BoxNumberCard.Text.Trim(), out valueLong))
            //        result += Environment.NewLine + LabelNumberCard.Text + " " + Properties.Resources.LabelIncorrect;
            //}

            if (!string.IsNullOrEmpty(BoxPoints.Text))
            {
                int valueInt;
                if (!int.TryParse(BoxPoints.Text.Trim(), out valueInt))
                    result += Environment.NewLine + LabelInn.Text + " " + Properties.Resources.LabelPoints;
            }
            else result += Environment.NewLine + LabelInn.Text + " " + Properties.Resources.LabelPoints;

            BoxError.Text = result;
            return result == string.Empty;
        }
    }
}
