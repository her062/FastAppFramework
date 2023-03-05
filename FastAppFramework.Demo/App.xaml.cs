using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Demo.ViewModels;
using FastAppFramework.Demo.Views;
using FastAppFramework.Wpf;
using Prism.Ioc;

namespace FastAppFramework.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : FastWpfApplication
    {
#region Protected Functions
        protected override void RegisterNavigationTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterNavigationTypes(containerRegistry);

            containerRegistry.RegisterForNavigation<FirstWizardFrame, FirstWizardFrameViewModel>();
            containerRegistry.RegisterForNavigation<LicenseAgreementPage, LicenseAgreementPageViewModel>();
            containerRegistry.RegisterForNavigation<OverviewPage, OverviewPageViewModel>();
            containerRegistry.RegisterForNavigation<ThemePage, ThemePageViewModel>();
        }
#endregion
    }
}
