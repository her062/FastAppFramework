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
    public class PreferenceFrameViewModel : BindableBase, INavigationAware, IConfirmNavigationRequest
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
        public IRegion RootRegion => this._regionManager.Regions[FastWpfApplication.RootRegionName];
        public IRegion Region => this._regionManager.Regions[FastWpfApplication.PreferenceRegionName];

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
        private ReactivePropertySlim<bool> _hasChanges; 
        private ReactivePropertySlim<bool> _hasErrors;
#endregion

#region Constructor/Destructor
        public PreferenceFrameViewModel(IRegionManager regionManager, IApplicationSettingProvider settingProvider)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
                this._settingProvider = settingProvider;
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
                        this.CanGoBack.Value = this.RootRegion.NavigationService.Journal.CanGoBack;
                    }).AddTo(this);
                this.BackCommand = this.CanGoBack.ToReactiveCommand()
                    .WithSubscribe(() => {
                        this.RootRegion.NavigationService.Journal.GoBack();
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
                this.SelectedNavigationItem.Subscribe(v => {
                    if (v == null)
                        return;

                    this.Region.RequestNavigate(v.View);
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
        private void PreferenceRegion_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            var view = e.Uri.OriginalString;
            if (view != this.SelectedNavigationItem.Value?.View)
                this.SelectedNavigationItem.Value = this.NavigationItems.FirstOrDefault(v => (v.View == view));
        }
#endregion
    }
}