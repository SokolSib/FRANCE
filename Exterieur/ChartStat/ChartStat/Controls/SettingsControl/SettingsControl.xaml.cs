using System.Windows.Controls;

namespace ChartStat.Controls.SettingsControl
{
    /// <summary>
    /// Interaction logic for LoadDataControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            DataContext = new ViewModel(AppData.AppData.Instance, new FilterControlsFactory.FilterControlsFactory());
        }
    }
}
