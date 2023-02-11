using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo
{
    [NavigationPage("Home", Region = NavigationPageAttribute.RegionType.Custom), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.Home)]
    public partial class HomePage : NavigationPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }
}