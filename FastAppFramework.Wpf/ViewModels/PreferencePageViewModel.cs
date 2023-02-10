using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Prism.Ioc;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf.ViewModels
{
    public class PreferencePageViewModel : BindableBase, INavigationAware, IConfirmNavigationRequest
    {
#region Internal Classes
        public class NavigationContainer : SideNavigationBarContainer
        {
        }
#endregion

#region Commands
        public ReactiveCommand LoadCommand
        {
            get; private set;
        }

        public ReactiveCommand BackCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand ApplyCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand RevertCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand ResetCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public ReactivePropertySlim<string?> Headline
        {
            get; private set;
        }

        public ReadOnlyReactiveCollection<SideNavigationBarItem> NavigationItems
        {
            get; private set;
        }
        public ReactivePropertySlim<SideNavigationBarItem?> SelectedNavigationItem
        {
            get; private set;
        }

        public ReactivePropertySlim<bool> CanGoBack
        {
            get; private set;
        }
#endregion

#region Fields
        private IRegionManager _regionManager;
        private IApplicationSettingProvider _settingProvider;
        private IRegion? _region;
        private IRegion? _rootRegion;
        private ReactivePropertySlim<bool> _hasChanges; 
        private ReactivePropertySlim<bool> _hasErrors;
#endregion

#region Constructor/Destructor
        public PreferencePageViewModel(IRegionManager regionManager, IApplicationSettingProvider settingProvider)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
                this._settingProvider = settingProvider;
                this._rootRegion = this._regionManager.Regions[FastWpfApplication.RootRegionName];
                this._hasChanges = new ReactivePropertySlim<bool>().AddTo(this);
                this._hasErrors = new ReactivePropertySlim<bool>().AddTo(this);
            }

            // Setup Properties.
            {
                this.Headline = new ReactivePropertySlim<string?>("Preferences").AddTo(this);
                this.NavigationItems = FastWpfApplication.Current.Container.Resolve<SideNavigationBarContainer>(FastWpfApplication.PreferenceNavigationContainerName).ToReadOnlyReactiveCollection().AddTo(this);
                this.SelectedNavigationItem = new ReactivePropertySlim<SideNavigationBarItem?>(null, ReactivePropertyMode.RaiseLatestValueOnSubscribe).AddTo(this);
                this.CanGoBack = new ReactivePropertySlim<bool>(false).AddTo(this);
            }

            // Setup Commands.
            {
                this.LoadCommand = new ReactiveCommand()
                    .WithSubscribe(() => {
                        this.CanGoBack.Value = this._rootRegion.NavigationService.Journal.CanGoBack;
                    }).AddTo(this);
                this.BackCommand = this.CanGoBack.ToReactiveCommand()
                    .WithSubscribe(() => {
                        this._rootRegion.NavigationService.Journal.GoBack();
                    }).AddTo(this);
                this.ApplyCommand = Observable.CombineLatest(
                        this._hasChanges, this._hasErrors,
                        (c, e) => (c && !e)
                    ).ToAsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        await Task.Run(() => {
                            // TODO: Apply Changes Behavior is not declared.
                        });
                    }).AddTo(this);
                this.RevertCommand = this._hasChanges.ToAsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        await Task.Run(() => {
                            // TODO: Revert Changes Behavior is not declared.
                        });
                    }).AddTo(this);
                this.ResetCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        await Task.Run(() => {
                            // TODO: Restore Defaults Behavior is not declared.
                        });
                    }).AddTo(this);
            }

            // Subscribes.
            {
                this._regionManager.Regions.CollectionChanged += Regions_CollectionChanged;
                this.SelectedNavigationItem.Subscribe(v => {
                    if (v == null)
                        return;

                    this._region?.RequestNavigate(v.View);
                }).AddTo(this);
            }
        }
#endregion

#region Public Functions
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(!navigationContext.Uri.Equals(navigationContext.NavigationService.Journal.CurrentEntry.Uri));
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
                    if (region?.Name == FastWpfApplication.PreferenceRegionName)
                    {
                        this._region = region;
                        this._region.NavigationService.Navigated += PreferenceRegion_Navigated;
                        if (this.SelectedNavigationItem.Value != null)
                            this._region.RequestNavigate(this.SelectedNavigationItem.Value.View);
                    }
                }
            }
        }
        private void PreferenceRegion_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            var view = e.Uri.OriginalString;
            if (view != this.SelectedNavigationItem.Value?.View)
                this.SelectedNavigationItem.Value = this.NavigationItems.FirstOrDefault(v => (v.View == view));
        }
#endregion
    }
}