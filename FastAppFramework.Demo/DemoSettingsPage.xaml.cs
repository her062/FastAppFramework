using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo
{
    [SideNavigationBar("Demo", Region = SideNavigationBarAttribute.RegionType.Preference)]
    public partial class DemoSettingsPage : UserControl
    {
        public DemoSettingsPage()
        {
            InitializeComponent();
        }
    }
}