using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Microsoft.Extensions.Logging;
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

        public ReadOnlyReactiveCollection<PreferenceNavigationBarItem> NavigationItems
        {
            get; private set;
        }
        public ReactivePropertySlim<PreferenceNavigationBarItem?> SelectedNavigationItem
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
        private ReactivePropertySlim<bool> _isDirty; 
        private ReactivePropertySlim<bool> _hasErrors;
#endregion

#region Constructor/Destructor
        public PreferenceFrameViewModel(IRegionManager regionManager, IApplicationSettingProvider settingProvider)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
                this._settingProvider = settingProvider;
                this._isDirty = new ReactivePropertySlim<bool>().AddTo(this);
                this._hasErrors = new ReactivePropertySlim<bool>().AddTo(this);
            }

            // Setup Properties.
            {
                this.Headline = new ReactivePropertySlim<string?>("Preferences").AddTo(this);
                this.NavigationItems = FastWpfApplication.Current.Container.Resolve<SideNavigationBarContainer>(FastWpfApplication.PreferenceNavigationContainerName)
                    .ToReadOnlyReactiveCollection(v => new PreferenceNavigationBarItem(v)).AddTo(this);
                this.SelectedNavigationItem = new ReactivePropertySlim<PreferenceNavigationBarItem?>(null, ReactivePropertyMode.RaiseLatestValueOnSubscribe).AddTo(this);
                this.CanGoBack = new ReactivePropertySlim<bool>(false).AddTo(this);
            }

            // Setup Commands.
            {
                this.LoadCommand = new ReactiveCommand()
                    .WithSubscribe(() => {
                        this.CanGoBack.Value = this.RootRegion.NavigationService.Journal.CanGoBack;

                        var loader = FastWpfApplication.Current.Container.Resolve<IRegionNavigationContentLoader>();
                        foreach (var item in this.NavigationItems)
                        {
                            if (item.Page != null)
                                continue;

                            var page = loader.LoadContent(this.Region, new NavigationContext(this.Region.NavigationService, new Uri(item.View, UriKind.RelativeOrAbsolute))) as PreferencePage;
                            if (page == null)
                                continue;

                            item.Page = page;
                            page.PropertyChanged += PreferencePage_PropertyChanged;
                            PreferencePage_PropertyChanged(page, null);
                        }
                    }).AddTo(this);
                this.BackCommand = this.CanGoBack.ToReactiveCommand()
                    .WithSubscribe(() => {
                        this.RootRegion.NavigationService.Journal.GoBack();
                    }).AddTo(this);
                this.ApplyCommand = Observable.CombineLatest(
                        this._isDirty, this._hasErrors,
                        (c, e) => (c && !e)
                    ).ToAsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        foreach (var item in this.NavigationItems)
                            item.Page?.Apply();
                        await Task.Run(() => {
                            this._settingProvider.Save();
                        });
                    }).AddTo(this);
                this.RevertCommand = this._isDirty.ToAsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        foreach (var item in this.NavigationItems)
                            item.Page?.Revert();
                        await Task.Run(() => {
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
        private void PreferencePage_PropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            var page = sender as PreferencePage;
            if (page == null)
                throw new ArgumentException(nameof(sender));

            var item = this.NavigationItems.First(v => (v.Page == page));
            if ((e == null) || (e.PropertyName == nameof(page.IsDirty)))
            {
                item.IsDirty = page.IsDirty;
                this._isDirty.Value = this.NavigationItems.Any(v => v.IsDirty);
                FastWpfApplication.Current.Logger.LogDebug($"{item.Title} IsDirty is Changed: {item.IsDirty}");
            }
            if ((e == null) || (e.PropertyName == nameof(page.HasErrors)))
            {
                item.HasErrors = page.HasErrors;
                this._hasErrors.Value = this.NavigationItems.Any(v => v.HasErrors);
                FastWpfApplication.Current.Logger.LogDebug($"{item.Title} HasErrors is Changed: {item.HasErrors}");
            }
        }
#endregion
    }
}