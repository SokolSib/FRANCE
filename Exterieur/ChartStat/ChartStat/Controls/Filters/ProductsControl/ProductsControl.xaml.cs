using System.Windows.Controls;

namespace ChartStat.Controls.Filters.ProductsControl
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class ProductsControl : UserControl
    {
        public ProductsControl()
        {
            InitializeComponent();
            DataContext=new ViewModel(AppData.AppData.Instance);
        }
    }
}
