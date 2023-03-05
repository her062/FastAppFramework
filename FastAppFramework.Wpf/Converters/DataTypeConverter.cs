using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FastAppFramework.Wpf.Converters
{
    public class DataTypeConverter : IValueConverter
    {
#region Public Functions
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.GetType();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
#endregion
    }
}