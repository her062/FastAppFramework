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
using Prism.Modularity;
using Prism.Regions;

namespace FastAppFramework.Wpf
{
    public abstract class FastWpfApplication : FastApplication
    {
#region Internal Classes
        private class ContainerRegistryWrapper : IContainerRegistry
        {
#region Properties
            public IEnumerable<(Type Type, string? Name)> Types => this._types;
#endregion

#region Fields
            private IContainerRegistry _source;
            private List<(Type Type, string? Name)> _types;
#endregion

#region Constructor/Destructor
            public ContainerRegistryWrapper(IContainerRegistry source)
            {
                // Setup Fields.
                {
                    this._source = source;
                    this._types = new List<(Type Type, string? name)>();
                }
            }
#endregion

#region Public Functions
            public bool IsRegistered(Type type)
            {
                return _source.IsRegistered(type);
            }
            public bool IsRegistered(Type type, string name)
            {
                return _source.IsRegistered(type, name);
            }
            public IContainerRegistry Register(Type from, Type to)
            {
                return _source.Register(from, to);
            }
            public IContainerRegistry Register(Type from, Type to, string name)
            {
                this._types.Add((to, name));
                return _source.Register(from, to, name);
            }
            public IContainerRegistry Register(Type type, Func<object> factoryMethod)
            {
                return _source.Register(type, factoryMethod);
            }
            public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
            {
                return _source.Register(type, factoryMethod);
            }
            public IContainerRegistry RegisterInstance(Type type, object instance)
            {
                return _source.RegisterInstance(type, instance);
            }
            public IContainerRegistry RegisterInstance(Type type, object instance, string name)
            {
                return _source.RegisterInstance(type, instance, name);
            }
            public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
            {
                return _source.RegisterMany(type, serviceTypes);
            }
            public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
            {
                return _source.RegisterManySingleton(type, serviceTypes);
            }
            public IContainerRegistry RegisterScoped(Type from, Type to)
            {
                return _source.RegisterScoped(from, to);
            }
            public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
            {
                return _source.RegisterScoped(type, factoryMethod);
            }
            public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
            {
                return _source.RegisterScoped(type, factoryMethod);
            }
            public IContainerRegistry RegisterSingleton(Type from, Type to)
            {
                return _source.RegisterSingleton(from, to);
            }
            public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
            {
                return _source.RegisterSingleton(from, to, name);
            }
            public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
            {
                return _source.RegisterSingleton(type, factoryMethod);
            }
            public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
            {
                return _source.RegisterSingleton(type, factoryMethod);
            }
#endregion
        }
#endregion

#region Constants
        public const string RootRegionName = "RootRegion";
        public const string MainRegionName = "MainRegion";
        public const string PreferenceRegionName = "PreferenceRegion";
        public const string MainPageName = "_main";
        public const string MainNavigationContainerName = "_mainNavigationContainer";
        public const string PreferencePageName = "_preference";
        public const string PreferenceNavigationContainerName = "_preferenceNavigationContainer";

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

        private ContainerRegistryWrapper? _containerRegistry;
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

            if (this._containerRegistry != null)
            {
                // Register navigation types in modules.
                var catalog = this.Container.Resolve<IModuleCatalog>();
                {
                    RegisterNavigationTypes(this._containerRegistry!);
                    foreach (var item in catalog.Modules)
                    {
                        var module = item as IFastWpfAppModule;
                        module?.RegisterNavigationTypes(this._containerRegistry!);
                    }
                }

                // Register SideNavigationBarItems.
                var mainNavigationContainer = this.Container.Resolve<SideNavigationBarContainer>(MainNavigationContainerName);
                var preferenceNavigationContainer = this.Container.Resolve<SideNavigationBarContainer>(PreferenceNavigationContainerName);
                foreach (var item in this._containerRegistry.Types)
                {
                    SideNavigationBarAttribute.RegionType region;
                    var navigationItem = SideNavigationBarAttribute.GetItem(item.Type, out region);
                    if (navigationItem != null)
                    {
                        switch (region)
                        {
                            case SideNavigationBarAttribute.RegionType.Main:
                                mainNavigationContainer?.Add(navigationItem);
                                break;
                            case SideNavigationBarAttribute.RegionType.Preference:
                                preferenceNavigationContainer?.Add(navigationItem);
                                break;
                            case SideNavigationBarAttribute.RegionType.Custom:
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }

            // Navigate to RootPage in RootRegion.
            var regionManager = this.Container.Resolve<IRegionManager>();
            regionManager.Regions[RootRegionName].RequestNavigate(this.Config.RootPage);

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
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            // Register instance for Main SideBar Navigation.
            containerRegistry.RegisterInstance<SideNavigationBarContainer>(new MainPageViewModel.NavigationContainer(), MainNavigationContainerName);
            // Register instance for Preference SideBar Navigation.
            containerRegistry.RegisterInstance<SideNavigationBarContainer>(new PreferencePageViewModel.NavigationContainer(), PreferenceNavigationContainerName);
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            this._containerRegistry = new ContainerRegistryWrapper(containerRegistry);

            this.Logger.LogDebug("");
        }
        protected virtual void RegisterNavigationTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>(MainPageName);
            containerRegistry.RegisterForNavigation<PreferencePage, PreferencePageViewModel>(PreferencePageName);
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