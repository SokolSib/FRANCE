using System.Windows.Controls;

namespace ChartStat.Controls.Filters.AnySubGroupsControl
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class AnySubGroupsControl : UserControl
    {
        public AnySubGroupsControl()
        {
            InitializeComponent();
            DataContext=new ViewModel(AppData.AppData.Instance);
        }
    }
}
