using System.ComponentModel;
using System.Configuration;
using System.Windows;

namespace TicketWindow.Global
{
    public class GridSize : INotifyPropertyChanged
    {
        private double _column1Width = double.NaN;
        private double _row1Height = double.NaN;
        private double _row2Height = double.NaN;
        private double _row3Height = double.NaN;

        public GridLength Column1WidthSetting
        {
            get { return Get(ref _column1Width, "Column1WidthSetting"); }
            set { Set(ref _column1Width, value, "Column1WidthSetting"); }
        }

        public GridLength Row1HeightSetting
        {
            get { return Get(ref _row1Height, "Row1HeightSetting"); }
            set { Set(ref _row1Height, value, "Row1HeightSetting"); }
        }

        public GridLength Row2HeightSetting
        {
            get { return Get(ref _row2Height, "Row2HeightSetting"); }
            set { Set(ref _row2Height, value, "Row2HeightSetting"); }
        }

        public GridLength Row3HeightSetting
        {
            get { return Get(ref _row3Height, "Row3HeightSetting"); }
            set { Set(ref _row3Height, value, "Row3HeightSetting"); }
        }

        private static GridLength Get(ref double propertyValue, string settingName)
        {
            if (double.IsNaN(propertyValue))
                propertyValue = int.Parse(ConfigurationManager.AppSettings[settingName]);
            return new GridLength(propertyValue, GridUnitType.Pixel);
        }

        // ReSharper disable once RedundantAssignment
        private void Set(ref double propertyValue, GridLength value, string settingName)
        {
            propertyValue = value.Value;
            var config = ConfigurationManager.OpenExeConfiguration(Config.AppPath + "TicketWindow.exe");
            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
            config.AppSettings.Settings[settingName].Value = value.Value.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            OnPropertyChanged(settingName);
        }

        #region INotifyPropertyChanged Members

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}