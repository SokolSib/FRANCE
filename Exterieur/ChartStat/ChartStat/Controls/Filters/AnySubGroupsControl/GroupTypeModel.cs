using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ChartStat.Controls.Filters.AnySubGroupsControl;
using ChartStat.Model.Models;

namespace ChartStat.Controls.AnySubGroupsControl
{
    public class GroupTypeModel : INotifyPropertyChanged
    {
        public GroupType Group { get; private set; }

        private IEnumerable<SubGroupTypeModel> _subGroupModels;

        public GroupTypeModel(GroupType group)
        {
            Group = group;
            SubGroupModels = group.SubGroups.Select(s => new SubGroupTypeModel(s)).ToArray();
        }

        public IEnumerable<SubGroupTypeModel> SubGroupModels
        {
            get { return _subGroupModels; }
            set
            {
                _subGroupModels = value;
                OnPropertyChanged("SubGroupModels");
            }
        }

        public override string ToString()
        {
            return Group.ToString();
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
