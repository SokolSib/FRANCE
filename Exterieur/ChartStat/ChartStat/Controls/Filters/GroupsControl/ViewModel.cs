using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ChartStat.AppData;
using ChartStat.Model.Models;

namespace ChartStat.Controls.Filters.GroupsControl
{
    public class ViewModel : INotifyPropertyChanged, ISaveDataControl, IValidateControl
    {
        private readonly IAppData _appData;
        private IEnumerable<GroupTypeModel> _groupTypes;
        private GroupTypeModel _selectedGroupType;
        private SubGroupType _selectedSubgroupType;

        public ViewModel(IAppData appData)
        {
            _appData = appData;

            if (appData.GroupTypes != null)
                GroupTypes = appData.GroupTypes.Select(g => new GroupTypeModel(g));

            appData.PropertyChanged +=
                (sender, args) =>
                {
                    switch (args.PropertyName)
                    {
                        case "GroupTypes":
                            if (appData.GroupTypes != null)
                                GroupTypes = appData.GroupTypes.Select(g => new GroupTypeModel(g));
                            break;
                    }
                };
        }

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
                if (_selectedGroupType != null)
                    SelectedSubgroupType = _selectedGroupType.Group.SubGroups.First();
            }
        }

        public SubGroupType SelectedSubgroupType
        {
            get { return _selectedSubgroupType; }
            set
            {
                _selectedSubgroupType = value;
                OnPropertyChanged("SelectedSubgroupType");
            }
        }

        public void SaveData()
        {
            _appData.SelectedSubGroupFilter = SelectedSubgroupType != null && SelectedSubgroupType.Id != -1 ? new[] {SelectedSubgroupType.Id} : null;

            if (_appData.SelectedSubGroupFilter == null)
                _appData.SelectedGroupFilter = SelectedGroupType.Group.Id;
            else _appData.SelectedGroupFilter = null;
        }

        public bool Validate()
        {
            var result = SelectedGroupType != null;
            if (!result) MessageBox.Show("необходимо сделать выбор", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return result;
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