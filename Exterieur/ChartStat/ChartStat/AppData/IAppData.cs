using System;
using System.Collections.Generic;
using System.ComponentModel;
using ChartStat.ChartUI.Enums;
using ChartStat.Enums;
using ChartStat.Model.Models;

namespace ChartStat.AppData
{
    public interface IAppData
    {
        ICollection<StatSalesType> Data { get; set; }

        ICollection<GroupType> GroupTypes { get; set; }

        ICollection<ProductType> ProductTypes { get; set; }

        int? SelectedGroupFilter { get; set; }

        ICollection<int> SelectedSubGroupFilter { get; set; }

        ICollection<Guid> SelectedProductFilter { get; set; }

        DateTime? StartDateFilter { get; set; }

        DateTime? EndDateFilter { get; set; }

        StatTypeEnum StatType { get; set; }
        
        ViewTypeEnum ViewType { get; set; }

        ChartTypeEnum ChartType { get; set; }

        DataDestinationEnum DataDestination { get; set; }

        FilterTypeEnum FilterType { get; set; }

        bool IsCount { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
