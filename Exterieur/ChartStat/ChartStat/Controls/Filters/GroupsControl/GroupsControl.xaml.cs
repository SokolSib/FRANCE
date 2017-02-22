using System.Windows.Controls;

namespace ChartStat.Controls.Filters.GroupsControl
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class GroupsControl : UserControl
    {
        public GroupsControl()
        {
            InitializeComponent();
            DataContext=new ViewModel(AppData.AppData.Instance);
        }
    }
}
