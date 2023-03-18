using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using FastAppFramework.ModularityDemo.ViewModels;
using FastAppFramework.ModularityDemo.Views;
using FastAppFramework.Wpf;
using Prism.Ioc;

namespace FastAppFramework.ModularityDemo
{
    public class Module : IFastWpfAppModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
        public void RegisterNavigationTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ModulePage, ModulePageViewModel>();
        }
        public void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
        }
        public void RegisterContextMenuItems(ContextMenuContainer container)
        {
        }
    }
}