using System.Windows.Controls;
using ChartStat.ChartUI.Enums;

namespace ChartStat.FilterControlsFactory
{
    public interface IFilterControlsFactory
    {
        Control CreateControl(FilterTypeEnum step);
    }
}
