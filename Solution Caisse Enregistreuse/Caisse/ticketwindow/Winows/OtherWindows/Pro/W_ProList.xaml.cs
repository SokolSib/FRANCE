using System.Windows;
using System.Windows.Controls;
using TicketWindow.Class;
using TicketWindow.DAL.Repositories;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Pro
{
    /// <summary>
    ///     Логика взаимодействия для W_ProList.xaml
    /// </summary>
    public partial class WProList : Window
    {
        public WProList()
        {
            InitializeComponent();
        }

        public void DataProLoaded(object sender, RoutedEventArgs e)
        {
            dataPro.ItemsSource = RepositoryPro.Pros;
            unset.IsEnabled = ClassProMode.ModePro;
        }

        private static void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;
        }
    }
}