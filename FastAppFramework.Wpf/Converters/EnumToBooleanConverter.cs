using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FastAppFramework.Wpf.Converters
{
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class EnumToBooleanConverter : IValueConverter
    {
#region Properties
        public object? TrueValue
        {
            get => this._trueValue;
            set => this._trueValue = value;
        }
#endregion

#region Fields
        private object? _trueValue;
#endregion

#region Constructor/Destructor
        public EnumToBooleanConverter()
        {
        }
#endregion

#region Public Functions
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum v)
            {
                object? mask = this.TrueValue;
                if (mask == null)
                {
                    if (parameter.GetType() == value.GetType())
                        mask = parameter;
                    if (parameter is string str)
                        mask = Enum.Parse(value.GetType(), str);
                }

                if (mask != null)
                    return v.Equals(mask);
            }
            return DependencyProperty.UnsetValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is bool v) && targetType.IsSubclassOf(typeof(Enum)))
            {
                if (v)
                {
                    object? mask = this.TrueValue;
                    if (mask == null)
                    {
                        if (parameter.GetType() == targetType)
                            mask = parameter;
                        if (parameter is string str)
                            mask = Enum.Parse(targetType, str);
                    }

                    if (mask != null)
                        return mask;
                }
            }
            return DependencyProperty.UnsetValue;
        }
#endregion
    }

    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class EnumToBooleanConverter<T> : EnumToBooleanConverter where T : struct, Enum
    {
#region Properties
        public new T? TrueValue
        {
            get => (T?)base.TrueValue;
            set => base.TrueValue = value;
        }
#endregion

#region Constructor/Destructor
        public EnumToBooleanConverter()
        {
        }
#endregion

#region Public Functions
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is T))
                return DependencyProperty.UnsetValue;

            return base.Convert(value, targetType, parameter, culture);
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(T))
                return DependencyProperty.UnsetValue;

            return base.ConvertBack(value, targetType, parameter, culture);
        }
#endregion
    }
}