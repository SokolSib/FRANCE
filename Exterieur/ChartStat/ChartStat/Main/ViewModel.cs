using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChartStat.Additional;
using ChartStat.Controls;
using ChartStat.Controls.Chart;
using ChartStat.Controls.SettingsControl;

namespace ChartStat.Main
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Control _settingsControl = new SettingsControl();
        private string _buttonText;
        private Control _control;

        public ViewModel()
        {
            SetContent();
            SetContentCommand = new RelayCommand(SetContentFunc);
            ExitCommand = new RelayCommand(ExitFunc);
        }

        public ICommand SetContentCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public Control Control
        {
            get { return _control; }
            set
            {
                _control = value;
                OnPropertyChanged("Control");
            }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged("ButtonText");
            }
        }

        public void ExitFunc(object parameter)
        {
            var window = (Window) parameter;
            window.Close();
        }

        public void SetContentFunc(object parameter)
        {
            if (Equals(Control, _settingsControl) && ((IValidateControl) _settingsControl.DataContext).Validate())
            {
                ((ISaveDataControl) _settingsControl.DataContext).SaveData();
                SetContent();
            }
            else if (!Equals(Control, _settingsControl)) SetContent();
        }

        private void SetContent()
        {
            Control = Equals(Control, _settingsControl) ? new Chart() : _settingsControl;

            ButtonText = Equals(Control, _settingsControl) ? "Показать график" : "Изменить фильтр";
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