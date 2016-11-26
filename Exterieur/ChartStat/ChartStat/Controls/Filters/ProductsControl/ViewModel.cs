using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChartStat.Additional;
using ChartStat.AppData;
using ChartStat.Model.Models;

namespace ChartStat.Controls.Filters.ProductsControl
{
    public class ViewModel : INotifyPropertyChanged, ISaveDataControl, IValidateControl
    {
        private readonly IAppData _appData;

        private IEnumerable<ProductType> _searchedProductTypes;
        private IEnumerable<ProductType> _productTypes;
        private ProductType _selectedProductType;
        private string _searchText;
        public ICommand SearchCommand { get; set; }

        public ViewModel(IAppData appData)
        {
            _appData = appData;
            SearchCommand = new RelayCommand(SearchFunc);

            if (appData.ProductTypes != null)
                ProductTypes = appData.ProductTypes;

            SearchedProductTypes = ProductTypes;

            appData.PropertyChanged +=
                (sender, args) =>
                {
                    switch (args.PropertyName)
                    {
                        case "ProductTypes":
                            if (appData.ProductTypes != null)
                                ProductTypes = appData.ProductTypes;

                            SearchedProductTypes = ProductTypes;
                            break;
                    }
                };
        }

        public void SearchFunc(object parameter)
        {
            if (parameter != null) SearchText = parameter.ToString();

            SearchedProductTypes = string.IsNullOrEmpty(SearchText) ?
                ProductTypes :
                ProductTypes.Where(p => p.BarCode.ToLower().Contains(SearchText.ToLower()) || p.Name.ToLower().Contains(SearchText.ToLower()));
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
            }
        }

        public ProductType SelectedProductType
        {
            get { return _selectedProductType; }
            set
            {
                _selectedProductType = value;
                OnPropertyChanged("SelectedProductType");
            }
        }

        public IEnumerable<ProductType> SearchedProductTypes
        {
            get { return _searchedProductTypes; }
            set
            {
                _searchedProductTypes = value;
                OnPropertyChanged("SearchedProductTypes");
            }
        }
        public IEnumerable<ProductType> ProductTypes
        {
            get { return _productTypes; }
            set
            {
                _productTypes = value;
                OnPropertyChanged("ProductTypes");
            }
        }

        public void SaveData()
        {
            _appData.SelectedProductFilter = new[] {SelectedProductType.Id};
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

        public bool Validate()
        {
            var result = SelectedProductType != null;
            if (!result) MessageBox.Show("необходимо сделать выбор", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return result;
        }
    }
}