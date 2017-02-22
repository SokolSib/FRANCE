using System;
using System.Windows;
using TicketWindow.Classes;

namespace TicketWindow.Winows.OtherWindows.Setting
{
    /// <summary>
    /// Логика взаимодействия для W_dateTimeSrv.xaml
    /// </summary>
    public partial class WDateTimeSrv : Window
    {
        public WDateTimeSrv(string mess)
        {
            InitializeComponent();

            status.Content = mess;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ClassDataTimeSrv.GetDateTimeFromSrv();

            if (!ClassDataTimeSrv.SetDateTimeFromSrv())
                status.Content += Properties.Resources.LabelDataError + Environment.NewLine;
            else
                status.Content += Properties.Resources.LabelDateUpdate + Environment.NewLine +Properties.Resources.LabelCashBoxDate + " : " + DateTime.Now;
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
