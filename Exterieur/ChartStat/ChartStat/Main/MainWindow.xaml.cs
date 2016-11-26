using System.Windows;
using System.Windows.Threading;
using ChartStat.FilterControlsFactory;

namespace ChartStat.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.DispatcherUnhandledException += (s, e) =>
                                                                {
                                                                    MessageBox.Show(e.Exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                                                    e.Handled = true;
                                                                };
            DataContext = new ViewModel();
        }
    }
}
