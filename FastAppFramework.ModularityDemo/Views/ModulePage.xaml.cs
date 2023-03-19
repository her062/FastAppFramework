using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;

namespace FastAppFramework.ModularityDemo.Views
{
    [NavigationPage("Module", Region = NavigationPageAttribute.RegionType.Main, Group = "Extensions")]
    public partial class ModulePage : NavigationPage
    {
        public ModulePage()
        {
            InitializeComponent();
        }
    }
}