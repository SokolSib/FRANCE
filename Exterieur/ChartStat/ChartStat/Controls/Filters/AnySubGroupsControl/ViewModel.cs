using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ChartStat.AppData;
using ChartStat.Controls.AnySubGroupsControl;

namespace ChartStat.Controls.Filters.AnySubGroupsControl
{
    public class ViewModel : INotifyPropertyChanged, ISaveDataControl, IValidateControl
    {
        private readonly IAppData _appData;
        private IEnumerable<GroupTypeModel> _groupTypes;
        private GroupTypeModel _selectedGroupType;

        public IEnumerable<GroupTypeModel> GroupTypes
        {
            get { return _groupTypes; }
            set
            {
                _groupTypes = value;
                OnPropertyChanged("GroupTypes");
            }
        }

        public GroupTypeModel SelectedGroupType
        {
            get { return _selectedGroupType; }
            set
            {
                _selectedGroupType = value;
                OnPropertyChanged("SelectedGroupType");
            }
        }

        public ViewModel(IAppData appData)
        {
            _appData = appData;

            if (appData.GroupTypes != null)
                GroupTypes = appData.GroupTypes.Select(g => new GroupTypeModel(g)).ToArray();

            appData.PropertyChanged +=
                (sender, args) =>
                {
                    switch (args.PropertyName)
                    {
                        case "GroupTypes":
                            if (appData.GroupTypes != null)
                                GroupTypes = appData.GroupTypes.Select(g => new GroupTypeModel(g)).ToArray();
                            break;
                    }
                };
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

        public void SaveData()
        {
            _appData.SelectedSubGroupFilter = SelectedGroupType.SubGroupModels.Where(s => s.IsSelected).Select(s => s.SubGroupType.Id).ToArray();
        }

        public bool Validate()
        {
            var result = SelectedGroupType.SubGroupModels.Count(s => s.IsSelected) > 0;
            if (!result) MessageBox.Show("необходимо сделать выбор", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return result;
        }
    }
}
