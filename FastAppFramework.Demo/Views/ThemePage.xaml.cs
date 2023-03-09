using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("Color Theme", Region = NavigationPageAttribute.RegionType.Custom), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.PaletteAdvanced)]
    public partial class ThemePage : NavigationPage
    {
        public ThemePage()
        {
            InitializeComponent();
        }
    }
}