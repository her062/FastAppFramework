using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FastAppFramework.Wpf.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color)
                return new SolidColorBrush((Color)value);
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Color))
            {
                if (value is SolidColorBrush)
                    return ((SolidColorBrush)value).Color;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}