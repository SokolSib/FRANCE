using System;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.Global;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Setting
{
    /// <summary>
    ///     Логика взаимодействия для W_Close_TicketWindow.xaml
    /// </summary>
    public partial class WCloseTicketWindow : Window
    {
        public WCloseTicketWindow(string str)
        {
            var tickedWindowId = GlobalVar.TicketWindow != Guid.Empty ? GlobalVar.TicketWindow.ToString() : string.Empty;

            str += Environment.NewLine +
                   Properties.Resources.LabelCashBox + " : " + Config.NameTicket + Environment.NewLine +
                   Properties.Resources.LabelNumberCheck + " : " + Config.NumberTicket + Environment.NewLine +
                   Properties.Resources.LabelUserName + " : " + Config.User + Environment.NewLine + Environment.NewLine +
                   "--------------------------------" + Environment.NewLine +
                   Properties.Resources.LabelOpenTotalTW + " : " + GlobalVar.TicketWindowG + Environment.NewLine +
                   Properties.Resources.LabelOpenLocal + " : " + tickedWindowId + Environment.NewLine;

            InitializeComponent();
            errorlist.Text = str;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
            {
                bs.Click += ButtonClick;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}