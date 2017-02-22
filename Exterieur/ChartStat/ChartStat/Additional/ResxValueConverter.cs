using System;
using System.Globalization;
using System.Resources;
using System.Windows.Data;
using System.Windows.Markup;

namespace ChartStat.Additional
{
    public class ResxValueConverter : MarkupExtension, IValueConverter
    {
        private ResourceManager _resourceManager;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = ReferenceEquals(null, parameter) ? 
                value.ToString() : 
                string.Concat(parameter.ToString(), value.ToString());

            return ResourceManager.GetObject(key, culture);
        }

        public Type Type { get; set; }

        private ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(null, _resourceManager))
                    _resourceManager = new ResourceManager(Type);
                return _resourceManager;
            }
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}