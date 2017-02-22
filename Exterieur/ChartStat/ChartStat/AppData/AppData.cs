using System;
using System.Collections.Generic;
using System.ComponentModel;
using ChartStat.ChartUI.Enums;
using ChartStat.Enums;
using ChartStat.Model.Models;

namespace ChartStat.AppData
{
    public class AppData : IAppData, INotifyPropertyChanged
    {
        private ICollection<GroupType> _groupTypes;
        private ICollection<ProductType> _productTypes;

        protected AppData()
        {
        }

        public static AppData Instance
        {
            get { return SingletonCreator.Instance; }
        }

        public ICollection<StatSalesType> Data { get; set; }

        public ICollection<GroupType> GroupTypes
        {
            get { return _groupTypes; }
            set
            {
                _groupTypes = value;
                OnPropertyChanged("GroupTypes");
            }
        }

        public ICollection<ProductType> ProductTypes
        {
            get { return _productTypes; }
            set
            {
                _productTypes = value;
                OnPropertyChanged("ProductTypes");
            }
        }

        public int? SelectedGroupFilter { get; set; }

        public ICollection<int> SelectedSubGroupFilter { get; set; }

        public ICollection<Guid> SelectedProductFilter { get; set; }

        public DateTime? StartDateFilter { get; set; }

        public DateTime? EndDateFilter { get; set; }

        public StatTypeEnum StatType { get; set; }

        public ViewTypeEnum ViewType { get; set; }

        public ChartTypeEnum ChartType { get; set; }

        public FilterTypeEnum FilterType { get; set; }

        public DataDestinationEnum DataDestination { get; set; }

        public bool IsCount { get; set; }

        private sealed class SingletonCreator
        {
            private static readonly AppData instance = new AppData();

            public static AppData Instance
            {
                get { return instance; }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}