using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo
{
    [NavigationPage("Controls", Region = NavigationPageAttribute.RegionType.Main), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.ApplicationOutline)]
    public partial class ControlPage : NavigationPage
    {
        public ControlPage()
        {
            InitializeComponent();
        }
    }
}