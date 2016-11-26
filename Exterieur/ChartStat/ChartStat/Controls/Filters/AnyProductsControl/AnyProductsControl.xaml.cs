using System.Windows.Controls;

namespace ChartStat.Controls.Filters.AnyProductsControl
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class AnyProductsControl : UserControl
    {
        public AnyProductsControl()
        {
            InitializeComponent();
            DataContext=new ViewModel(AppData.AppData.Instance);
        }
    }
}
