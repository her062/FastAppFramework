using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo
{
    [NavigationPage("Sample", Group = "Sample"), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.CircleOutline)]
    public partial class SamplePage : NavigationPage
    {
        public SamplePage()
        {
            InitializeComponent();
        }
    }
}