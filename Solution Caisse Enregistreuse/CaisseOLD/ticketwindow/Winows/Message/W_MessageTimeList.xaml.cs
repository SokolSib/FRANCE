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

namespace ticketwindow.Winows.Message
{
    /// <summary>
    /// Логика взаимодействия для W_MessageTimeList.xaml
    /// </summary>
    public partial class W_MessageTimeList : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public int c = 2;

        public W_MessageTimeList()
        {
            InitializeComponent();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Start();
            if (c != 0)
                tbtimer.Text = "(" + c + " sec )";
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;

            tbtimer.Text = "(" + c + " sec )";


            if (c < 0) this.Close();

            c = c - 1;

            CommandManager.InvalidateRequerySuggested();
        }

        private void bcan_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
