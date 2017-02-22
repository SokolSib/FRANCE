using System.Windows.Controls;
using Microsoft.Reporting.WinForms;

namespace ChartStat.ChartUI.ChartControl
{
    /// <summary>
    /// Interaction logic for ChartConrol.xaml
    /// </summary>
    public partial class ChartConrol : UserControl
    {
        public ChartConrol()
        {
            InitializeComponent();
            DataContext = new ViewModel(ReportViewer); 
            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            ReportViewer.ZoomMode = ZoomMode.Percent;
            ReportViewer.ZoomPercent = 100;
        }
    }
}
