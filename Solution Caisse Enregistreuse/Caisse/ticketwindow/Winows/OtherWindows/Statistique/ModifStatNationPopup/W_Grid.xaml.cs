using System.Windows;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Statistique.ModifStatNationPopup
{
    /// <summary>
    /// Логика взаимодействия для W_Grid.xaml
    /// </summary>
    public partial class WGrid : Window
    {
        public WGrid()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = ((WStat) Owner).A;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender, ((WStat) Owner).A);
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            FunctionsService.Click(sender, ((WStat) Owner).A, dataGrid.SelectedItem);
        }

        private void ButtonClick2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
