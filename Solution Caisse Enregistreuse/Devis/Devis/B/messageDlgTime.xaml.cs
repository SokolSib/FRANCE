using System;
using System.Windows;
using System.Windows.Input;

namespace Devis.B
{
    /// <summary>
    /// Логика взаимодействия для messageDlgTime.xaml
    /// </summary>
    public partial class messageDlgTime : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public int c = 15;
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
        public messageDlgTime()
        {
            Class.ClassF.wm_sound(@"Data\Computer_Error.wav");

            InitializeComponent();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Start();
            if (c != 0)
                tbtimer.Text = "(" + c + " sec )";
        }
    }
}
