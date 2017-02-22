using System.Windows.Controls;

namespace ChartStat.Controls.Chart
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
            DataContext = new ViewModel(AppData.AppData.Instance, ChartControl.DataContext as ChartUI.ChartControl.ViewModel, ChartControl);
        }
    }
}
