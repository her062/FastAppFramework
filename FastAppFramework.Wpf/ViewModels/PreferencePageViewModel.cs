using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf.ViewModels
{
    public class PreferencePageViewModel : BindableBase, INavigationAware, IConfirmNavigationRequest
    {
#region Commands
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
#endregion

#region Fields
        private IApplicationSettingProvider _settingProvider;
        private ReactivePropertySlim<IRegionNavigationService?> _navigationService;
        private ReactivePropertySlim<bool> _hasChanges; 
        private ReactivePropertySlim<bool> _hasErrors;
#endregion

#region Constructor/Destructor
        public PreferencePageViewModel(IApplicationSettingProvider settingProvider)
        {
            // Setup Fields.
            {
                this._settingProvider = settingProvider;
                this._navigationService = new ReactivePropertySlim<IRegionNavigationService?>(null, ReactivePropertyMode.RaiseLatestValueOnSubscribe).AddTo(this);
                this._hasChanges = new ReactivePropertySlim<bool>().AddTo(this);
                this._hasErrors = new ReactivePropertySlim<bool>().AddTo(this);
            }

            // Setup Properties.
            {
                this.Headline = new ReactivePropertySlim<string?>("Preferences").AddTo(this);
            }

            // Setup Commands.
            {
                this.BackCommand = this._navigationService.Select(v => (v != null) && v.Journal.CanGoBack).ToReactiveCommand()
                    .WithSubscribe(() => {
                        this._navigationService.Value?.Journal.GoBack();
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
        }
#endregion

#region Public Functions
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this._navigationService.Value = navigationContext.NavigationService;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(!navigationContext.Uri.Equals(navigationContext.NavigationService.Journal.CurrentEntry.Uri));
        }
#endregion
    }
}