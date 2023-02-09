using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
#region Properties
        public ReactivePropertySlim<string?> Headline
        {
            get; private set;
        }
        public ReactivePropertySlim<bool> ShowNavigationDrawer
        {
            get; private set;
        }

        public ReactivePropertySlim<bool> HasNavigationPages
        {
            get; private set;
        }
#endregion

#region Fields
        private IRegionManager _regionManager;
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
                // TODO: HasNavigationPages should be determined by navigation items.
                this.HasNavigationPages = new ReactivePropertySlim<bool>(true).AddTo(this);
            }

            // Subscribes.
            {
                this._regionManager.Regions.CollectionChanged += Regions_CollectionChanged;
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
                        var home = this._settingProvider.GetValue<string>(FastWpfApplication.HomePageSetting);
                        if (!string.IsNullOrEmpty(home))
                            region.RequestNavigate(home);
                    }
                }
            }
        }
#endregion
    }
}