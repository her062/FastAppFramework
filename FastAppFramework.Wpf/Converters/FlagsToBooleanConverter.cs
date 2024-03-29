using System.ComponentModel;
using System.Xml.Schema;
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
    public class FlagsToBooleanConverter : IValueConverter
    {
#region Properties
        public object? TrueValue
        {
            get => this._trueValue;
            set => this._trueValue = value;
        }
#endregion

#region Fields
        private object? _latestValue;
        private object? _trueValue;
#endregion

#region Constructor/Destructor
        public FlagsToBooleanConverter()
        {
        }
#endregion

#region Public Functions
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                this._latestValue = value;
                object? mask = this.TrueValue;
                if (mask == null)
                {
                    if (parameter.GetType() == value.GetType())
                        mask = parameter;
                    else if (parameter is string str)
                        mask = Enum.Parse(value.GetType(), str);
                }

                if (mask != null)
                {
                    ulong v = System.Convert.ToUInt64(value);
                    ulong m = System.Convert.ToUInt64(mask);

                    return ((v & m) == m);
                }
            }
            return DependencyProperty.UnsetValue;
        }
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._latestValue == null)
                throw new InvalidOperationException();

            if ((value is bool v) && (targetType.IsSubclassOf(typeof(Enum))))
            {
                object? mask = this.TrueValue;
                if (mask == null)
                {
                    if (parameter.GetType() == targetType)
                        mask = parameter;
                    else if (parameter is string str)
                        mask = Enum.Parse(targetType, str);
                }

                if (mask != null)
                {
                    ulong l = System.Convert.ToUInt64(this._latestValue);
                    ulong m = System.Convert.ToUInt64(mask);

                    if (v)
                        this._latestValue = Enum.ToObject(targetType, (l | m));
                    else if (!v && ((l & m) == m))
                        this._latestValue = Enum.ToObject(targetType, (l - m));
                    return this._latestValue;
                }
            }
            return DependencyProperty.UnsetValue;
        }
#endregion
    }

    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class FlagsToBooleanConverter<T> : FlagsToBooleanConverter where T : struct, Enum
    {
#region Properties
        public new T? TrueValue
        {
            get => (T?)base.TrueValue;
            set => base.TrueValue = value;
        }
#endregion

#region Constructor/Destructor
        public FlagsToBooleanConverter()
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