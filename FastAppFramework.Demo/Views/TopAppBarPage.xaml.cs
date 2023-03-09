using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("TopAppBars", Group = "Controls", Region = NavigationPageAttribute.RegionType.Main)]
    public partial class TopAppBarPage : NavigationPage
    {
        public TopAppBarPage()
        {
            InitializeComponent();
        }
    }
}