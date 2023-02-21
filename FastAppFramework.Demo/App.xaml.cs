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
using Reactive.Bindings;

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
            containerRegistry.RegisterForNavigation<ControlPage>();
            containerRegistry.RegisterForNavigation<DemoSettingsPage, DemoSettingsPageViewModel>();
        }
        protected override void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
            base.RegisterSettingTypes(settingRegistry);

            settingRegistry.Register(new ApplicationSettingInfo<DemoSettings>("demo"){ Variability = Variability.Normal });
        }

        protected override void RegisterNotifyIconContextMenuItems(ContextMenuContainer container)
        {
            base.RegisterNotifyIconContextMenuItems(container);

            container.Add("Clickable Item", new ReactiveCommand().WithSubscribe(() => { MessageBox.Show("'Clickable Item' is clicked!"); }));
            container.Add("Checkable Item", false);
            container.Add();

            var subMenu = new ContextMenuContainer();
            subMenu.Add("Sub Clickable Item", new ReactiveCommand().WithSubscribe(() => { MessageBox.Show("'Sub Clickable Item' is clicked!"); }));
            subMenu.Add("Sub Checkable Item", false);
            subMenu.Add("Sub Sub Menu", new ContextMenuItem[]{ new ContextMenuClickItem(){ Title = "Sub Sub Clickable Item", Command = new ReactiveCommand().WithSubscribe(() => { MessageBox.Show("'Sub Sub Clickable Item' is clicked!"); }) }});

            container.Add("Sub Menu", subMenu);
        }
    }
}
