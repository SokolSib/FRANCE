using System.ComponentModel;
using ChartStat.Model.Models;

namespace ChartStat.Controls.Filters.AnySubGroupsControl
{
    public class SubGroupTypeModel : INotifyPropertyChanged
    {
        public SubGroupType SubGroupType { get; set; }

        private bool _isSelected;

        public SubGroupTypeModel(SubGroupType subGroupType)
        {
            SubGroupType = subGroupType;
        }

        public override string ToString()
        {
            return SubGroupType.ToString();
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
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
