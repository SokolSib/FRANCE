using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using ChartStat.Additional;

namespace ChartStat.Windows.DBWindow
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _connectionString;

        public ViewModel()
        {
            CloseCommand = new RelayCommand(CloseFunc);
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
                OnPropertyChanged("ConnectionString");
            }
        }

        public ICommand CloseCommand { get; set; }

        public void CloseFunc(object parameter)
        {
            ((Window) parameter).Close();
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