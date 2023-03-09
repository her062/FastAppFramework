using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("Color Theme", Region = NavigationPageAttribute.RegionType.Preference), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.PaletteAdvanced)]
    public partial class ThemeSettingsPage : PreferencePage
    {
        public ThemeSettingsPage()
        {
            InitializeComponent();
        }
    }
}