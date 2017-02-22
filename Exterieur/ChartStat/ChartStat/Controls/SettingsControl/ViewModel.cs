using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ChartStat.Additional;
using ChartStat.AppData;
using ChartStat.ChartUI.Enums;
using ChartStat.Enums;
using ChartStat.FilterControlsFactory;
using ChartStat.Model;
using ChartStat.Services;
using Control = System.Windows.Controls.Control;
using MessageBox = System.Windows.MessageBox;

namespace ChartStat.Controls.SettingsControl
{
    public class ViewModel : INotifyPropertyChanged, IValidateControl, ISaveDataControl
    {
        private readonly IAppData _appData;
        private readonly Dictionary<FilterTypeEnum, Control> _controls = new Dictionary<FilterTypeEnum, Control>();
        private readonly IFilterControlsFactory _filterControlsFactory;
        private DataDestinationEnum _dataDestination = DataDestinationEnum.Db;
        private DateTime? _endDateFilter;
        private Control _filterContent;
        private FilterTypeEnum _filterType;
        private LoadProcessEnum _processStatus = LoadProcessEnum.None;
        private DateTime? _startDateFilter;
        private StatTypeEnum _statType = StatTypeEnum.StatSales;

        public ViewModel(IAppData appdata, IFilterControlsFactory filterControlsFactory)
        {
            _filterControlsFactory = filterControlsFactory;
            _appData = appdata;
            StartDateFilter = _appData.StartDateFilter;
            EndDateFilter = _appData.EndDateFilter;

            LoadDbCommand = new RelayCommand(LoadDbFunc);
            LoadXmlCommand = new RelayCommand(LoadXmlFunc);
            TestConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            FilterType = FilterTypeEnum.ProductOrBarcode;

            // По умолчанию фильтр даты = сегодня
            if (!StartDateFilter.HasValue) StartDateFilter = DateTime.Now;
        }

        public ICommand LoadDbCommand { get; set; }
        public ICommand LoadXmlCommand { get; set; }

        public Control FilterContent
        {
            get { return _filterContent; }
            set
            {
                _filterContent = value;
                OnPropertyChanged("FilterContent");
            }
        }

        public FilterTypeEnum FilterType
        {
            get { return _filterType; }
            set
            {
                _filterType = value;
                OnPropertyChanged("FilterType");
                SetFilterContent();
            }
        }

        public DateTime? StartDateFilter
        {
            get { return _startDateFilter; }
            set
            {
                _startDateFilter = value;
                OnPropertyChanged("StartDateFilter");
            }
        }

        public DateTime? EndDateFilter
        {
            get { return _endDateFilter; }
            set
            {
                _endDateFilter = value;
                OnPropertyChanged("EndDateFilter");
            }
        }

        public StatTypeEnum StatType
        {
            get { return _statType; }
            set
            {
                _statType = value;
                OnPropertyChanged("StatType");
            }
        }

        public DataDestinationEnum DataDestination
        {
            get { return _dataDestination; }
            set
            {
                _dataDestination = value;
                OnPropertyChanged("DataDestination");
            }
        }

        public LoadProcessEnum ProcessStatus
        {
            get { return _processStatus; }
            set
            {
                _processStatus = value;
                OnPropertyChanged("ProcessStatus");
            }
        }

        public void SaveData()
        {
            _appData.StatType = StatType;
            _appData.DataDestination = DataDestination;
            _appData.StartDateFilter = StartDateFilter;
            _appData.EndDateFilter = EndDateFilter;
            _appData.FilterType = FilterType;

            var control = FilterContent.DataContext as ISaveDataControl;
            if (control != null) control.SaveData();
        }

        public bool Validate()
        {
            if (ProcessStatus != LoadProcessEnum.Success)
                MessageBox.Show("нет данных", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

            if (ProcessStatus == LoadProcessEnum.Success && FilterContent != null)
            {
                var control = FilterContent.DataContext as IValidateControl;
                return control == null || control.Validate();
            }
            return false;
        }

        private void SetFilterContent()
        {
            if (!_controls.ContainsKey(FilterType))
                _controls[FilterType] = _filterControlsFactory.CreateControl(FilterType);

            FilterContent = _controls[FilterType];
        }

        public void LoadDbFunc(object parameter)
        {
            //var openDbWindow = new DbWindow();
            //openDbWindow.ShowDialog();

            TestConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public void LoadXmlFunc(object parameter)
        {
            //var dialog = new FolderBrowserDialog();
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            ProcessStatus = LoadProcessEnum.InProcess;

            Task<bool>.Factory
                .StartNew(() =>
                          {
                              try
                              {
                                  var xml = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "GroupProduct.xml"));
                                  _appData.GroupTypes = XmlStructure.GetGroups(xml);

                                  xml = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "Product.xml"));
                                  _appData.ProductTypes = XmlStructure.GetProducts(xml);

                                  return true;
                              }
                              catch (Exception)
                              {
                                  return false;
                              }
                          })
                .ContinueWith(
                    res => { ProcessStatus = res.Result ? LoadProcessEnum.Success : LoadProcessEnum.Fail; });

            //}
        }

        private void TestConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) return;

            ProcessStatus = LoadProcessEnum.InProcess;

            Task<bool>.Factory
                .StartNew(() =>
                          {
                              if (DbService.IsDbAvaliable(connectionString))
                              {
                                  try
                                  {
                                      _appData.GroupTypes = DbService.GetGroupTypes(connectionString);
                                      _appData.ProductTypes = DbService.GetProductTypes(connectionString);
                                      DataDestination = DataDestinationEnum.Db;
                                      return true;
                                  }
                                  catch (Exception)
                                  {
                                      return false;
                                  }
                              }
                              return false;
                          })
                .ContinueWith(
                    res => { ProcessStatus = res.Result ? LoadProcessEnum.Success : LoadProcessEnum.Fail; });
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