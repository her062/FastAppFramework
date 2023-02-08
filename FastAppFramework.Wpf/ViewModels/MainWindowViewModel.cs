using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
#region Commands
        public ReactiveCommand PreferenceNavigationCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public ReactivePropertySlim<bool> HasPreferences
        {
            get; private set;
        }
#endregion

#region Fields
        private IRegionManager _regionManager;
#endregion

#region Constructor/Destructor
        public MainWindowViewModel(IRegionManager regionManager)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
            }

            // Setup Properties.
            {
                // TODO: HasPreferences should be determined by navigation items.
                this.HasPreferences = new ReactivePropertySlim<bool>(true).AddTo(this);
            }

            // Setup Commands.
            {
                this.PreferenceNavigationCommand = this.HasPreferences.ToReactiveCommand()
                    .WithSubscribe(() => {
                        this._regionManager.Regions[FastWpfApplication.RootRegionName]?.RequestNavigate(FastWpfApplication.PreferencePageName);
                    }).AddTo(this);
            }
        }
#endregion
    }
}