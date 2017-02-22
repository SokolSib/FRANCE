using System.Windows;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Winows.SyncSettingWindow
{
    /// <summary>
    /// Interaction logic for SyncSettingWindow.xaml
    /// </summary>
    public partial class SyncSettingWindow : Window
    {
        public SyncSettingWindow()
        {
            InitializeComponent();

            BoxPayProduct.Content = Properties.Resources.LabelPayProduct;
            BoxCloseTicket.Content = Properties.Resources.LabelCloseTicket;
            BoxPayProductTmp.Content = Properties.Resources.LabelPayProductTmp;
            BoxCheckTicketTmp.Content = Properties.Resources.LabelCheckTicketTmp;
            BoxCheckTicket.Content = Properties.Resources.LabelCheckTicket;
            BoxClientInfo.Content = Properties.Resources.LabelClientInfo;
            BoxPro.Content = Properties.Resources.LabelProviders;
            BoxDiscountCard.Content = Properties.Resources.LabelDiscountCard;
            BoxDevisWeb.Content = Properties.Resources.LabelDevisWeb;
            BoxXmlFile.Content = Properties.Resources.LabelXmlFile;

            BoxPayProduct.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.PayProduct);
            BoxCloseTicket.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.CloseTicket);
            BoxPayProductTmp.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.PayProductTmp);
            BoxCheckTicketTmp.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.CheckTicketTmp);
            BoxCheckTicket.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.CheckTicket);
            BoxClientInfo.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.ClientInfo);
            BoxPro.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.Pro);
            BoxDiscountCard.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.DiscountCard);
            BoxDevisWeb.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.DevisWeb);
            BoxXmlFile.IsChecked = RepositorySyncIsLoading.IsLoading(SyncEnum.XmlFile);
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.PayProduct, BoxPayProduct.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.CloseTicket, BoxCloseTicket.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.PayProductTmp, BoxPayProductTmp.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.CheckTicketTmp, BoxCheckTicketTmp.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.CheckTicket, BoxCheckTicket.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.ClientInfo, BoxClientInfo.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.Pro, BoxPro.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.DiscountCard, BoxDiscountCard.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.DevisWeb, BoxDevisWeb.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SetSyncIsLoading(SyncEnum.XmlFile, BoxXmlFile.IsChecked.GetValueOrDefault());
            RepositorySyncIsLoading.SaveFile();
            Close();
        }
    }
}
