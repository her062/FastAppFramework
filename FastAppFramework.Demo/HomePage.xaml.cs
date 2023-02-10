using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo
{
    [SideNavigationBar("Home"), MaterialDesignPageIcon(MaterialDesignThemes.Wpf.PackIconKind.Home)]
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }
    }
}