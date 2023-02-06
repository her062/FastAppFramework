using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Core;
using Prism.Ioc;

namespace FastAppFramework.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : FastApplication
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var settings = this.Container.Resolve<DemoSettings>();
            settings.Nonvolatile = "Nonvolatile changed from OnInitialized";
            settings.Volatile = "Volatile changed from OnInitialized";
            this.Settings.Save();
        }
        protected override Window CreateShell()
        {
            return new MainWindow();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
        }
        protected override void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
            base.RegisterSettingTypes(settingRegistry);

            settingRegistry.Register<DemoSettings>();
            var settings = this.Container.Resolve<DemoSettings>();
            settings.Nonvolatile = "Nonvolatile changed from RegisterSettingTypes";
            settings.Volatile = "Volatile changed from RegisterSettingTypes";
        }
    }
}
