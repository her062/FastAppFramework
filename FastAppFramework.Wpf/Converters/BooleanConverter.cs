using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FastAppFramework.Wpf.Converters
{
    public class BooleanConverter : IValueConverter
    {
#region Properties
        public object? TrueValue
        {
            get { return this._trueValue; }
            set { this._trueValue = value; }
        }
        public object? FalseValue
        {
            get { return this._falseValue; }
            set { this._falseValue = value; }
        }
#endregion

#region Fields
        private object? _trueValue;
        private object? _falseValue;
#endregion

#region Constructor/Destructor
        public BooleanConverter() : this(true, false)
        {
        }
        public BooleanConverter(object? trueValue, object? falseValue)
        {
            // Setup Fields.
            {
                this._trueValue = trueValue;
                this._falseValue = falseValue;
            }
        }
#endregion

#region Public Functions
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var b = (bool)value;
                return (b) ? this._trueValue : this._falseValue;
            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsEqual(value, this._trueValue))
                return true;
            if (IsEqual(value, this._falseValue))
                return false;
            return DependencyProperty.UnsetValue;
        }
#endregion

#region Private Functions
        private static bool IsEqual(object? objA, object? objB)
        {
            if (Equals(objA, objB))
                return true;

            var c = objA as IComparable;
            if (c != null)
                return (c.CompareTo(objB) == 0);

            return false;
        }
#endregion
    }

    public class BooleanConverter<T> : BooleanConverter
    {
#region Properties
        public new T? TrueValue
        {
            get { return (T?)base.TrueValue; }
            set { base.TrueValue = value; }
        }
        public new T? FalseValue
        {
            get { return (T?)base.FalseValue; }
            set { base.FalseValue = value; }
        }
#endregion

#region Constructor/Destructor
        public BooleanConverter(T? trueValue, T? falseValue) : base(trueValue, falseValue)
        {
        }
#endregion
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : BooleanConverter<bool>
    {
        public InverseBooleanConverter() : base(false, true)
        {
        }
    }
}