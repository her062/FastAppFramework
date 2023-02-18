using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo
{
    [NavigationPage("Demo", Region = NavigationPageAttribute.RegionType.Preference)]
    public partial class DemoSettingsPage : PreferencePage
    {
        public DemoSettingsPage()
        {
            InitializeComponent();
        }
    }
}