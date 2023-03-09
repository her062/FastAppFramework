using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.Views
{
    [NavigationPage("SearchableComboBoxes", Group = "Controls", Region = NavigationPageAttribute.RegionType.Main), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.TextBoxSearchOutline)]
    public partial class SearchableComboBoxPage : NavigationPage
    {
        public SearchableComboBoxPage()
        {
            InitializeComponent();
        }
    }
}