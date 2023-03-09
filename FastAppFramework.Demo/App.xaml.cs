using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Core;
using FastAppFramework.Demo.Models;
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
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var theme = this.Settings.GetValue<ThemeSettings>("theme");
            if (theme != null)
            {
                if (theme.LoadFromFile)
                    theme.Apply();
                else
                    theme.Load();
            }
        }
        protected override void RegisterNavigationTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterNavigationTypes(containerRegistry);

            containerRegistry.RegisterForNavigation<FirstWizardFrame, FirstWizardFrameViewModel>();
            containerRegistry.RegisterForNavigation<LicenseAgreementPage, LicenseAgreementPageViewModel>();
            containerRegistry.RegisterForNavigation<OverviewPage, OverviewPageViewModel>();
            containerRegistry.RegisterForNavigation<ThemePage, ThemePageViewModel>();
            containerRegistry.RegisterForNavigation<TopAppBarPage>();
            containerRegistry.RegisterForNavigation<SearchableComboBoxPage>();
            containerRegistry.RegisterForNavigation<DialogPage, DialogPageViewModel>();
            containerRegistry.RegisterForNavigation<ThemeSettingsPage, ThemeSettingsPageViewModel>();
        }
        protected override void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
            base.RegisterSettingTypes(settingRegistry);

            settingRegistry.Register<ThemeSettings>("theme", new ThemeSettings());
        }
#endregion
    }
}
