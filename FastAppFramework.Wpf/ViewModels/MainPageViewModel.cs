using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using Reactive.Bindings.Helpers;
using Prism.Ioc;

namespace FastAppFramework.Wpf.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
#region Internal Classes
        public class NavigationContainer : SideNavigationBarContainer
        {
        }
#endregion

#region Properties
        public ReactivePropertySlim<string?> Headline
        {
            get; private set;
        }
        public ReactivePropertySlim<bool> ShowNavigationDrawer
        {
            get; private set;
        }

        public ReadOnlyReactiveCollection<SideNavigationBarItem> NavigationItems
        {
            get; private set;
        }
        public ReadOnlyReactivePropertySlim<bool> HasNavigationPages
        {
            get; private set;
        }
        public ReactivePropertySlim<SideNavigationBarItem?> SelectedNavigationItem
        {
            get; private set;
        }
#endregion

#region Fields
        private IRegionManager _regionManager;
        private IRegion? _region;
        private IApplicationSettingProvider _settingProvider;
#endregion

#region Constructor/Destructor
        public MainPageViewModel(IRegionManager regionManager, IApplicationSettingProvider settingProvider)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
                this._settingProvider = settingProvider;
            }

            // Setup Properties.
            {
                this.Headline = new ReactivePropertySlim<string?>("Welcome").AddTo(this);
                this.ShowNavigationDrawer = new ReactivePropertySlim<bool>(false).AddTo(this);
                this.NavigationItems = FastWpfApplication.Current.Container.Resolve<SideNavigationBarContainer>(FastWpfApplication.MainNavigationContainerName).ToReadOnlyReactiveCollection().AddTo(this);
                this.SelectedNavigationItem = new ReactivePropertySlim<SideNavigationBarItem?>(null, ReactivePropertyMode.RaiseLatestValueOnSubscribe).AddTo(this);
                this.HasNavigationPages = this.NavigationItems.ObserveProperty(o => o.Count).Select(v => (v != 0)).ToReadOnlyReactivePropertySlim().AddTo(this);
            }

            // Subscribes.
            {
                this._regionManager.Regions.CollectionChanged += Regions_CollectionChanged;
                this.SelectedNavigationItem.Subscribe(v => {
                    if (v != null)
                    {
                        this._region?.RequestNavigate(v.View);
                        this.ShowNavigationDrawer.Value = false;
                    }
                });
            }
        }

        private void Regions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    throw new ArgumentNullException(nameof(e.NewItems));
                foreach (var item in e.NewItems)
                {
                    var region = item as IRegion;
                    if (region?.Name == FastWpfApplication.MainRegionName)
                    {
                        this._region = region;
                        var home = this._settingProvider.GetValue<string>(FastWpfApplication.HomePageSetting);
                        if (!string.IsNullOrEmpty(home))
                            this._region.RequestNavigate(home);
                    }
                }
            }
        }
#endregion
    }
}