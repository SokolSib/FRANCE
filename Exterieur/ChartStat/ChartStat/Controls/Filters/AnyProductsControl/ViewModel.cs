using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ChartStat.Additional;
using ChartStat.AppData;

namespace ChartStat.Controls.Filters.AnyProductsControl
{
    public class ViewModel : INotifyPropertyChanged, ISaveDataControl, IValidateControl
    {
        private readonly IAppData _appData;

        private IEnumerable<ProductTypeModel> _searchedProductTypes;
        private IEnumerable<ProductTypeModel> _productTypes;
        private string _searchText;
        public ICommand SearchCommand { get; set; }

        public ViewModel(IAppData appData)
        {
            _appData = appData;
            SearchCommand = new RelayCommand(SearchFunc);

            if (appData.ProductTypes != null)
                ProductTypes = appData.ProductTypes.Select(p => new ProductTypeModel(p));

            SearchedProductTypes = ProductTypes.ToArray();

            appData.PropertyChanged +=
                (sender, args) =>
                {
                    switch (args.PropertyName)
                    {
                        case "ProductTypes":
                            if (appData.ProductTypes != null)
                                ProductTypes = appData.ProductTypes.Select(p=>new ProductTypeModel(p));

                            SearchedProductTypes = ProductTypes.ToArray();
                            break;
                    }
                };
        }

        public void SearchFunc(object parameter)
        {
            if (parameter != null) SearchText = parameter.ToString();

            SearchedProductTypes = string.IsNullOrEmpty(SearchText) ?
                ProductTypes.ToArray() :
                ProductTypes.Where(p => p.ProductType.BarCode.ToLower().Contains(SearchText.ToLower()) || p.ProductType.Name.ToLower().Contains(SearchText.ToLower())).ToArray();
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

        public IEnumerable<ProductTypeModel> SearchedProductTypes
        {
            get { return _searchedProductTypes; }
            set
            {
                _searchedProductTypes = value;
                OnPropertyChanged("SearchedProductTypes");
            }
        }

        public IEnumerable<ProductTypeModel> ProductTypes
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
            _appData.SelectedProductFilter = SearchedProductTypes.Where(p => p.IsSelected).Select(p => p.ProductType.Id).ToArray();
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
            var result = SearchedProductTypes.Count(p => p.IsSelected) > 0;
            if (!result) MessageBox.Show("необходимо сделать выбор", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return result;
        }
    }
}