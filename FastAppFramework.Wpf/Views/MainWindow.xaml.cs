using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace FastAppFramework.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
#region Properties
        public bool ShowPreferenceNavigationButton
        {
            get => (bool)GetValue(ShowPreferenceNavigationButtonProperty);
            set => SetValue(ShowPreferenceNavigationButtonProperty, value);
        }
        public static readonly DependencyProperty ShowPreferenceNavigationButtonProperty = DependencyProperty.Register(
            nameof(ShowPreferenceNavigationButton),
            typeof(bool),
            typeof(MainWindow),
            new PropertyMetadata(true)
        );
#endregion

#region Constructor/Destructor
        public MainWindow()
        {
            InitializeComponent();
        }
#endregion
    }
}
