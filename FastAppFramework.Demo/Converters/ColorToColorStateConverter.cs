using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ColorPicker.Models;

namespace FastAppFramework.Demo
{
    public class ColorToColorStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is Color c) && (targetType == typeof(ColorState)))
            {
                var obj = new ColorState();
                obj.SetARGB(c.A, c.R, c.G, c.B);
                return obj;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is ColorState c) && (targetType == typeof(Color)))
                return Color.FromArgb((byte)c.A, (byte)c.RGB_R, (byte)c.RGB_G, (byte)c.RGB_G);

            return DependencyProperty.UnsetValue;
        }
    }
}