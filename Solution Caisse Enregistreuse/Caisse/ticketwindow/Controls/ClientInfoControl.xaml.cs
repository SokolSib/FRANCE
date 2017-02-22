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
        private Guid _infoClientsCustomerId;

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

        public ClientInfo GetClientInfo(DiscountCard card, ClientInfo info)
        {
            if (card.InfoClientsCustomerId == Guid.Empty)
                card.InfoClientsCustomerId = Guid.NewGuid();

            _infoClientsCustomerId = card.InfoClientsCustomerId;

            var customerId = info?.CustomerId ?? Guid.NewGuid();
            var sex = info?.Sex ?? 0;
            var password = info?.Password ?? string.Empty;
            var countrysCustomerId = info?.CountrysCustomerId ?? Guid.Empty;
            var favoritesProductAutoCustomerId = info?.FavoritesProductAutoCustomerId ?? Guid.Empty;
            var nclient = info?.Nclient ?? string.Empty;
            
            var newInfo = new ClientInfo(customerId, 1, sex, BoxName.Text, BoxSurname.Text, BoxNameCompany.Text,
                           BoxInn.Text, BoxFrtva.Text, BoxOfficeAddress.Text, BoxOfficeZipCode.Text,
                           BoxOfficeCity.Text, BoxHomeAddress.Text, BoxHomeZipCode.Text, BoxHomeCity.Text,
                           BoxTelephone.Text, BoxMail.Text, password, countrysCustomerId, favoritesProductAutoCustomerId, nclient, card.InfoClientsCustomerId);

            if (newInfo.DiscountCards.Count == 0)
                newInfo.DiscountCards.Add(card);

            return newInfo;
        }

        public DiscountCard GetDiscountCard(DiscountCard card)
        {
            card.Points = BoxPoints.Text.ToInt();
            card.InfoClientsCustomerId = _infoClientsCustomerId;
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
                    result += Environment.NewLine + LabelPoints.Text + " " + Properties.Resources.LabelIncorrect;
            }
            else result += Environment.NewLine + LabelPoints.Text + " " + Properties.Resources.LabelIncorrect;

            BoxError.Text = result;
            return result == string.Empty;
        }
    }
}
