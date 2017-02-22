using System.ComponentModel;
using ChartStat.Model.Models;

namespace ChartStat.Controls.Filters.AnyProductsControl
{
    public class ProductTypeModel : INotifyPropertyChanged
    {
        public ProductType ProductType { get; set; }

        private bool _isSelected;

        public ProductTypeModel(ProductType productType)
        {
            ProductType = productType;
        }

        public override string ToString()
        {
            return ProductType.ToString();
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
