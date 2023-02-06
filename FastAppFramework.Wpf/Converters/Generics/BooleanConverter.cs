using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FastAppFramework.Wpf.Converters.Generics
{
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
}