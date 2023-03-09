using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using FastAppFramework.Core;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace FastAppFramework.Demo.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ThemeSettings : CloneableModelBase
    {
#region Properties
        public bool LoadFromFile
        {
            get => this._loadFromFile;
            set => SetValue(ref this._loadFromFile, value);
        }
        public BaseTheme BaseTheme
        {
            get => this._baseTheme;
            set => SetValue(ref this._baseTheme, value);
        }
        public Color PrimaryColor
        {
            get => this._primaryColor;
            set => SetValue(ref this._primaryColor, value);
        }
        public Color SecondaryColor
        {
            get => this._secondaryColor;
            set => SetValue(ref this._secondaryColor, value);
        }
#endregion

#region Fields
        [JsonProperty("loadFromFile")]
        private bool _loadFromFile;
        [JsonProperty("baseTheme")]
        private BaseTheme _baseTheme;
        [JsonProperty("primaryColor")]
        private Color _primaryColor;
        [JsonProperty("secondaryColor")]
        private Color _secondaryColor;
#endregion

#region Constructor/Destructor
        public ThemeSettings()
        {
            // Setup Fields.
            {
                this._loadFromFile = false;
                Load();
            }
        }
#endregion

#region Public Functions
        public override int CompareTo(object? obj)
        {
            var o = obj as ThemeSettings;
            if (o == null)
                throw new InvalidOperationException($"{obj?.GetType()} cannot convert to {this.GetType()}");

            int ret = 0;
            if ((ret = this.LoadFromFile.CompareTo(o.LoadFromFile)) != 0)
                return ret;
            if ((ret = this.BaseTheme.CompareTo(o.BaseTheme)) != 0)
                return ret;
            if ((ret = string.Compare(this.PrimaryColor.ToString(), o.PrimaryColor.ToString())) != 0)
                return ret;
            if ((ret = string.Compare(this.SecondaryColor.ToString(), o.SecondaryColor.ToString())) != 0)
                return ret;

            return 0;
        }
        public override void CopyTo(object obj)
        {
            var o = obj as ThemeSettings;
            if (o == null)
                throw new InvalidOperationException($"{obj.GetType()} cannot convert to {this.GetType()}");

            o.LoadFromFile = this.LoadFromFile;
            o.BaseTheme = this.BaseTheme;
            o.PrimaryColor = this.PrimaryColor;
            o.SecondaryColor = this.SecondaryColor;
        }

        public void Load()
        {
            var theme = (new PaletteHelper()).GetTheme();

            this.BaseTheme = theme.GetBaseTheme();
            this.PrimaryColor = theme.PrimaryMid.Color;
            this.SecondaryColor = theme.SecondaryMid.Color;
        }
        public void Apply()
        {
            var palette = new PaletteHelper();
            var theme = palette.GetTheme();

            theme.SetBaseTheme(this.BaseTheme.GetBaseTheme());
            theme.SetPrimaryColor(this.PrimaryColor);
            theme.SetSecondaryColor(this.SecondaryColor);

            palette.SetTheme(theme);
        }
#endregion

#region Protected Functions
        protected override object CreateInstance()
        {
            return new ThemeSettings();
        }
#endregion
    }
}