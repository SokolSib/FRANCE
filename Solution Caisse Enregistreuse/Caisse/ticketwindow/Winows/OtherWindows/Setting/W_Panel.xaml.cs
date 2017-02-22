using System.Windows;

namespace TicketWindow.Winows.OtherWindows.Setting
{
    /// <summary>
    /// Логика взаимодействия для W_Panel.xaml
    /// </summary>
    public partial class WPanel : Window
    {
        public WPanel()
        {
            InitializeComponent();
            Topmost = true;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Left = 0;
            Top = 0;
        }
    }
}
