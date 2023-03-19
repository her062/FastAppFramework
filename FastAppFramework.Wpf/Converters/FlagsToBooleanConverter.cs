using System.ComponentModel;
using System.Xml.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FastAppFramework.Wpf
{
    public class FlagsToBooleanConverter<T> : IValueConverter where T : struct, Enum
    {
#region Properties
        public T? TrueValue
        {
            get => this._trueValue;
            set => this._trueValue = value;
        }
#endregion

#region Fields
        private T? _latestValue;
        private T? _trueValue;
#endregion

#region Constructor/Destructor
        public FlagsToBooleanConverter()
        {
        }
#endregion

#region Public Functions
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is T v)
            {
                this._latestValue = v;
                T? mask = this._trueValue;
                if (mask == null)
                {
                    if (parameter is T)
                        mask = (T)parameter;
                    else if (parameter is string)
                        mask = (T)Enum.Parse(typeof(T), (string)parameter);
                }

                if (mask != null)
                    return this._latestValue.Value.HasFlag(mask.Value);
            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._latestValue == null)
                throw new InvalidOperationException();

            if ((value is bool v) && (targetType == typeof(T)))
            {
                T? mask = this._trueValue;
                if (mask == null)
                {
                    if (parameter is T)
                        mask = (T)parameter;
                    else if (parameter is string)
                        mask = (T)Enum.Parse(typeof(T), (string)parameter);
                }

                if (mask != null)
                {
                    if (v && !this._latestValue.Value.HasFlag(mask.Value))
                        this._latestValue = (T)Enum.ToObject(typeof(T), System.Convert.ToUInt64(this._latestValue.Value) + System.Convert.ToUInt64(mask.Value));
                    else if (!v && this._latestValue.Value.HasFlag(mask.Value))
                        this._latestValue = (T)Enum.ToObject(typeof(T), System.Convert.ToUInt64(this._latestValue.Value) - System.Convert.ToUInt64(mask.Value));
                    return this._latestValue;
                }
            }
            return DependencyProperty.UnsetValue;
        }
#endregion
    }
}