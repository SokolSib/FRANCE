using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ChartStat.AppData;
using ChartStat.ChartUI.ChartControl;
using ChartStat.ChartUI.Enums;
using ChartStat.Enums;
using ChartStat.Model;
using ChartStat.Services;

namespace ChartStat.Controls.Chart
{
    public class ViewModel: INotifyPropertyChanged
    {
        private string _statusText;
        private readonly Dispatcher _dispatcher;
        private bool _isCount = true;
        private readonly IAppData _appdata;
        private readonly ChartUI.ChartControl.ViewModel _view;
        private ChartTypeEnum _chartType = ChartTypeEnum.ColumnarChart;
        private ViewTypeEnum _viewType = ViewTypeEnum.Days;
        private readonly ChartConrol _chartConrol;
        private readonly string _connectionString;
        private readonly string _dataDirectory;

        public ChartTypeEnum ChartType
        {
            get { return _chartType; }
            set
            {
                _chartType = value;
                OnPropertyChanged("ChartType");
                LoadChart();
            }
        }

        public bool IsCount
        {
            get { return _isCount; }
            set
            {
                _isCount = value;
                OnPropertyChanged("IsCount");
                LoadChart();
            }
        }

        public ViewTypeEnum ViewType
        {
            get { return _viewType; }
            set
            {
                _viewType = value;
                OnPropertyChanged("ViewType");
                LoadChart();
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        private void LoadChart()
        {
            _appdata.ChartType = ChartType;
            _appdata.ViewType = ViewType;
            _appdata.IsCount = IsCount;

            var isUseCodeInName = _appdata.FilterType == FilterTypeEnum.GroupOrSubgroup || _appdata.FilterType == FilterTypeEnum.AnySubgroups;
            if (_appdata.FilterType == FilterTypeEnum.GroupOrSubgroup && _appdata.SelectedSubGroupFilter != null)
                isUseCodeInName = false;

            if (_appdata.Data != null)
            {
                _view.LoadTableAndChart(_appdata.Data.ToArray(), _appdata.StatType, _appdata.ChartType, _appdata.ViewType, _appdata.FilterType, _appdata.IsCount, isUseCodeInName);
                _chartConrol.Visibility = Visibility.Visible;
            }
        }

        public ViewModel(IAppData appdata, ChartUI.ChartControl.ViewModel view, ChartConrol chartConrol)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _appdata = appdata;
            _view = view;
            _chartConrol = chartConrol;
            _chartConrol.Visibility = Visibility.Collapsed;
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            _dataDirectory = ConfigurationManager.AppSettings["DataDirectory"];
            Load();
        }

        public void Load()
        {
            StatusText = "ЗАГРУЗКА ДАННЫХ";
            Task<bool>.Factory
                .StartNew(() =>
                          {
                              if (_appdata.DataDestination == DataDestinationEnum.Db)
                              {
                                  var includeSumm = _appdata.StatType == StatTypeEnum.StatSalesWithReceipts;
                                  var isStatSales = _appdata.StatType == StatTypeEnum.StatSales || _appdata.StatType == StatTypeEnum.StatSalesWithReceipts;

                                  if (_appdata.FilterType == FilterTypeEnum.GroupOrSubgroup)
                                  {
                                      if (_appdata.SelectedGroupFilter.HasValue)
                                          _appdata.Data = DbService.GetStatSalesTypesByGroup(_connectionString, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                              _appdata.SelectedGroupFilter.Value, includeSumm, isStatSales);
                                      else if (_appdata.SelectedSubGroupFilter != null && _appdata.SelectedSubGroupFilter.Count == 1)
                                          _appdata.Data = DbService.GetStatSalesTypesBySubgroup(_connectionString, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                              _appdata.SelectedSubGroupFilter, includeSumm, isStatSales);
                                      else throw new Exception("не выбрана группа или подгруппа");
                                  }
                                  else if (_appdata.FilterType == FilterTypeEnum.ProductOrBarcode)
                                  {
                                      _appdata.Data = DbService.GetStatSalesTypesByProductType(_connectionString, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                          _appdata.SelectedProductFilter, includeSumm, isStatSales);
                                  }
                                  else if (_appdata.FilterType == FilterTypeEnum.AnyProducts)
                                  {
                                      _appdata.Data = DbService.GetStatSalesTypesByProductType(_connectionString, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                          _appdata.SelectedProductFilter, includeSumm, isStatSales);
                                  }
                                  else if (_appdata.FilterType == FilterTypeEnum.AnySubgroups)
                                  {
                                      _appdata.Data = DbService.GetStatSalesTypesBySubgroup(_connectionString, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                          _appdata.SelectedSubGroupFilter, includeSumm, isStatSales);
                                  }
                                  else throw new Exception("Нет функции для выборки");
                              }
                              else
                              {
                                  if (_appdata.FilterType == FilterTypeEnum.GroupOrSubgroup)
                                  {
                                      if (_appdata.SelectedGroupFilter.HasValue)
                                          _appdata.Data = XmlStructure.GetStatSalesTypesByGroup(_dataDirectory, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                              _appdata.SelectedGroupFilter.Value, _appdata.GroupTypes);
                                      else if (_appdata.SelectedSubGroupFilter != null && _appdata.SelectedSubGroupFilter.Count == 1)
                                          _appdata.Data = XmlStructure.GetStatSalesTypesBySubgroup(_dataDirectory, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                              _appdata.SelectedSubGroupFilter, _appdata.GroupTypes);
                                      else throw new Exception("не выбрана группа или подгруппа");
                                  }
                                  else if (_appdata.FilterType == FilterTypeEnum.ProductOrBarcode)
                                  {
                                      _appdata.Data = XmlStructure.GetStatSalesTypesByProductType(_dataDirectory, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                          _appdata.SelectedProductFilter);
                                  }
                                  else if (_appdata.FilterType == FilterTypeEnum.AnyProducts)
                                  {
                                      _appdata.Data = XmlStructure.GetStatSalesTypesByProductType(_dataDirectory, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                          _appdata.SelectedProductFilter);
                                  }
                                  else if (_appdata.FilterType == FilterTypeEnum.AnySubgroups)
                                  {
                                      _appdata.Data = XmlStructure.GetStatSalesTypesBySubgroup(_dataDirectory, _appdata.StartDateFilter, _appdata.EndDateFilter,
                                          _appdata.SelectedSubGroupFilter, _appdata.GroupTypes);
                                  }
                                  else throw new Exception("Нет функции для выборки");
                              }
                              return true;
                          })
                .ContinueWith(res =>
                              {
                                  _dispatcher.Invoke(DispatcherPriority.Background, new Action(LoadChart));
                                  StatusText = "ГОТОВО";
                              });
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