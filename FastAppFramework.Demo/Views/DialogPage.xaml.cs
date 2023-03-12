using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("Dialogs", Group = "Controls", Region = NavigationPageAttribute.RegionType.Main)]
    public partial class DialogPage : NavigationPage
    {
        public DialogPage()
        {
            InitializeComponent();
        }
    }
}