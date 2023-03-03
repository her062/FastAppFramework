using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using FastAppFramework.Core;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace FastAppFramework.Demo
{
    public class ColorSet : ModelBase
    {
#region Constants
        public enum Order
        {
            Primary,
            Secondary,
        }
#endregion

#region Properties
        public Color Color
        {
            get => this._color;
            set => SetValue(ref this._color, value);
        }

        public ColorPair Light
        {
            get => this._light;
            set => SetValue(ref this._light, value);
        }
        public ColorPair Mid
        {
            get => this._mid;
            set => SetValue(ref this._mid, value);
        }
        public ColorPair Dark
        {
            get => this._dark;
            set => SetValue(ref this._dark, value);
        }
#endregion

#region Fields
        private Color _color;
        private ColorPair _light;
        private ColorPair _mid;
        private ColorPair _dark;
#endregion

#region Constructor/Destructor
        public ColorSet()
        {
        }
#endregion

#region Public Functions
        public static ColorSet FromTheme(ITheme theme, Order order)
        {
            var obj = new ColorSet();
            obj.Apply(theme, order);
            return obj;
        }

        public void Apply(ITheme theme, Order order)
        {
            switch (order)
            {
                case Order.Primary:
                    Color = theme.PrimaryMid.Color;
                    Light = theme.PrimaryLight;
                    Mid = theme.PrimaryMid;
                    Dark = theme.PrimaryDark;
                    break;
                case Order.Secondary:
                    Color = theme.SecondaryMid.Color;
                    Light = theme.SecondaryLight;
                    Mid = theme.SecondaryMid;
                    Dark = theme.SecondaryDark;
                    break;
                default:
                    break;
            }
        }
#endregion
    }
}