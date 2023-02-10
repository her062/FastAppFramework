using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Prism.Ioc;

namespace FastAppFramework.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
#region Commands
        public ReactiveCommand HomeNavigationCommand
        {
            get; private set;
        }
        public ReactiveCommand PreferenceNavigationCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public ReadOnlyReactivePropertySlim<bool> HasPreferences
        {
            get; private set;
        }

        public ReadOnlyReactivePropertySlim<bool> ShowHomeNavigationButton
        {
            get; private set;
        }
#endregion

#region Fields
        private IRegionManager _regionManager;
        private IApplicationSettingProvider _settingProvider;
        private ReactivePropertySlim<IRegion?> _mainRegion;
#endregion

#region Constructor/Destructor
        public MainWindowViewModel(IRegionManager regionManager, IApplicationSettingProvider settingProvider)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
                this._settingProvider = settingProvider;
                this._mainRegion = new ReactivePropertySlim<IRegion?>().AddTo(this);
            }

            // Setup Properties.
            {
                this.ShowHomeNavigationButton = this._settingProvider.Observe<string>(FastWpfApplication.HomePageSetting).Select(v => !string.IsNullOrEmpty(v)).ToReadOnlyReactivePropertySlim().AddTo(this);
                this.HasPreferences = FastWpfApplication.Current.Container.Resolve<SideNavigationBarContainer>(FastWpfApplication.PreferenceNavigationContainerName).ObserveProperty(o => o.Count).Select(v => (v != 0)).ToReadOnlyReactivePropertySlim().AddTo(this);
            }

            // Setup Commands.
            {
                this.HomeNavigationCommand = Observable.CombineLatest(
                        this.ShowHomeNavigationButton, this._mainRegion,
                        (s, r) => (s && (r != null))
                    ).ToReactiveCommand()
                    .WithSubscribe(() => {
                        this._regionManager.Regions[FastWpfApplication.RootRegionName]?.RequestNavigate(FastWpfApplication.MainPageName);
                        var home = this._settingProvider.GetValue<string>(FastWpfApplication.HomePageSetting);
                        this._mainRegion.Value?.RequestNavigate(home);
                    }).AddTo(this);
                this.PreferenceNavigationCommand = this.HasPreferences.ToReactiveCommand()
                    .WithSubscribe(() => {
                        this._regionManager.Regions[FastWpfApplication.RootRegionName]?.RequestNavigate(FastWpfApplication.PreferencePageName);
                    }).AddTo(this);
            }

            // Subscribes.
            {
                this._regionManager.Regions.CollectionChanged += Regions_CollectionChanged;
            }
        }
#endregion

#region Private Functions
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
                        this._mainRegion.Value = region;
                }
            }
        }
#endregion
    }
}