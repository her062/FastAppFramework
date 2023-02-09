using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Core;
using FastAppFramework.Wpf.ViewModels;
using FastAppFramework.Wpf.Views;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Regions;

namespace FastAppFramework.Wpf
{
    public abstract class FastWpfApplication : FastApplication
    {
#region Constants
        public const string RootRegionName = "RootRegion";
        public const string MainRegionName = "MainRegion";
        public const string PreferenceRegionName = "PreferenceRegion";
        public const string MainPageName = "_main";
        public const string PreferencePageName = "_preference";

        public const string HomePageSetting = "home";
#endregion

#region Properties
        public static new FastWpfApplication Current => (FastWpfApplication)FastApplication.Current;
#endregion

#region Fields
        public ApplicationConfiguration Config
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        protected FastWpfApplication()
        {
            // Setup Fields.
            {
                this.Config = new ApplicationConfiguration();
            }
        }
#endregion

#region Protected Functions
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Navigate to RootPage in RootRegion.
            var regionManager = this.Container.Resolve<IRegionManager>();
            regionManager.Regions.First(v => (v.Name == RootRegionName)).RequestNavigate(this.Config.RootPage);

            this.Logger.LogDebug("");
        }
        protected override Window CreateShell()
        {
            var obj = new MainWindow()
            {
                Title = this.Config.WindowTitle,
                Width = this.Config.WindowSize.Width,
                Height = this.Config.WindowSize.Height,
            };
            this.Logger.LogDebug("");
            return obj;
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            // Register types for navigation.
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>(MainPageName);
            containerRegistry.RegisterForNavigation<PreferencePage, PreferencePageViewModel>(PreferencePageName);

            this.Logger.LogDebug("");
        }
        protected override void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
            base.RegisterSettingTypes(settingRegistry);

            settingRegistry.Register(new ApplicationSettingInfo(typeof(string), HomePageSetting){ DefaultValue = Config.HomePage, Variability = Variability.Volatile });

            this.Logger.LogDebug("");
        }
#endregion
    }
}