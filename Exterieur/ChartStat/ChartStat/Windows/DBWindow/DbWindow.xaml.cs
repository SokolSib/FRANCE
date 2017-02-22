using System.Windows;

namespace ChartStat.Windows.DBWindow
{
    /// <summary>
    /// Interaction logic for DbWindow.xaml
    /// </summary>
    public partial class DbWindow : Window
    {
        public DbWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        public string ConnectionString
        {
            get { return ((ViewModel) DataContext).ConnectionString; }
        }
    }
}
