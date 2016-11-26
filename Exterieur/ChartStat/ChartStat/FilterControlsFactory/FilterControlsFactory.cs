using System;
using System.Windows.Controls;
using ChartStat.ChartUI.Enums;
using ChartStat.Controls.AnySubGroupsControl;
using ChartStat.Controls.Filters.AnyProductsControl;
using ChartStat.Controls.Filters.AnySubGroupsControl;
using ChartStat.Controls.Filters.GroupsControl;
using ChartStat.Controls.Filters.ProductsControl;

namespace ChartStat.FilterControlsFactory
{
    public class FilterControlsFactory : IFilterControlsFactory
    {
        public Control CreateControl(FilterTypeEnum filter)
        {
            switch (filter)
            {
                case FilterTypeEnum.GroupOrSubgroup:
                    return new GroupsControl();
                case FilterTypeEnum.ProductOrBarcode:
                    return new ProductsControl();
                case FilterTypeEnum.AnyProducts:
                    return new AnyProductsControl();
                case FilterTypeEnum.AnySubgroups:
                    return new AnySubGroupsControl();
                default:
                    throw new ArgumentOutOfRangeException("filter", filter, null);
            }
        }
    }
}
