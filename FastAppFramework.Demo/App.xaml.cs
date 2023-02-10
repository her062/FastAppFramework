using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Core;
using FastAppFramework.Wpf;
using Prism.Ioc;

namespace FastAppFramework.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : FastWpfApplication
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
        }
        protected override void RegisterNavigationTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterNavigationTypes(containerRegistry);

            // Register types for navigation.
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<SamplePage, SamplePageViewModel>();
            containerRegistry.RegisterForNavigation<DemoSettingsPage>();
        }
        protected override void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
            base.RegisterSettingTypes(settingRegistry);
        }
    }
}
