using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace TicketWindow.Winows.OtherWindows.Message
{
    /// <summary>
    /// Логика взаимодействия для W_MessageTime.xaml
    /// </summary>
    public partial class WMessageTime : Window
    {
        public int C = 2;

        public WMessageTime(string message)
        {
            InitializeComponent();

            Message.Text = message;
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Start();
            if (C != 0)
                Tbtimer.Text = "(" + C + " sec )";
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            Tbtimer.Text = "(" + C + " sec )";
            if (C < 0) Close();
            C = C - 1;
            CommandManager.InvalidateRequerySuggested();
        }

        private void BcanClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
