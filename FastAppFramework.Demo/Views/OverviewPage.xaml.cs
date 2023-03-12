using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("Overview", Region = NavigationPageAttribute.RegionType.Custom)]
    public partial class OverviewPage : NavigationPage
    {
        public OverviewPage()
        {
            InitializeComponent();
        }
    }
}