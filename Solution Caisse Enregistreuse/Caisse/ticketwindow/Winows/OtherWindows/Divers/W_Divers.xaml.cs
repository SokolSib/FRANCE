using System.Windows;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Divers
{
    /// <summary>
    /// Логика взаимодействия для W_Divers.xaml
    /// </summary>
    public partial class WDivers : Window
    {
        public WDivers()
        {
            InitializeComponent();
        }

        public int Tva = 1;
        public decimal Prix = 0.0m;
        public bool Ballance = false;
        public string NameProduct = "";

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            numPad.TextBox = xValue;
            numPad.BEnter = xEnter;
        }

        private void XEnterClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
