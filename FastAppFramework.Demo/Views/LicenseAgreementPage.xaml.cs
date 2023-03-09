using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("License Agreement", Region = NavigationPageAttribute.RegionType.Custom), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.License)]
    public partial class LicenseAgreementPage : NavigationPage
    {
        public LicenseAgreementPage()
        {
            InitializeComponent();
        }
    }
}