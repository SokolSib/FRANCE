using System.Windows;
using System.Windows.Input;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.OtherWindows.Discount
{
    /// <summary>
    ///     Логика взаимодействия для W_InfoClients.xaml
    /// </summary>
    public partial class WInfoClients : Window
    {
        private DiscountCard _card;
        private ClientInfo _info;

        public WInfoClients()
        {
            InitializeComponent();
            SetInfo();
            BoxNotFound.Visibility = Visibility.Collapsed;
        }
        
        private void EbarcodeKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                _info = null;
                _card = RepositoryDiscountCard.GetOneByNumber(BoxBarCode.Text);
                SetInfo();
            }
        }

        private void SetInfo()
        {
            if (_card != null)
                _info = RepositoryDiscount.GetClientInfoById(_card.InfoClientsCustomerId);

            ClientInfoControl.SetClientInfo(_info);

            if (_card != null)
            {
                BtnActive.Content = _card.IsActive
                    ? Properties.Resources.BtnToDeactive
                    : Properties.Resources.BtnToActive;

                BtnActive.Visibility = Visibility.Visible;
                BtnSave.Visibility = Visibility.Visible;
                ClientInfoControl.Visibility = Visibility.Visible;
                BoxNotFound.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnActive.Visibility = Visibility.Collapsed;
                BtnSave.Visibility = Visibility.Collapsed;
                ClientInfoControl.Visibility = Visibility.Collapsed;
                BoxNotFound.Visibility = Visibility.Visible;
            }
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            RepositoryClientInfo.Sync();
            if (ClientInfoControl.Validate())
            {
                if (_info == null) RepositoryClientInfo.Add(ClientInfoControl.GetClientInfo(_card, _info));
                else RepositoryClientInfo.Update(ClientInfoControl.GetClientInfo(_card, _info));

                RepositoryDiscountCard.Update(ClientInfoControl.GetDiscountCard(_card));
            }
        }

        private void BtnActiveClick(object sender, RoutedEventArgs e)
        {
            _card.IsActive = !_card.IsActive;
            RepositoryDiscountCard.Update(_card);
            SetInfo();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            BoxBarCode.Focus();
        }
    }
}