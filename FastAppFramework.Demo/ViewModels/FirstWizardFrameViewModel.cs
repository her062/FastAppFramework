using System.Text;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using FastAppFramework.Wpf;
using Reactive.Bindings;
using System.Windows;
using Reactive.Bindings.Extensions;
using MaterialDesignExtensions.Controls;
using Prism.Regions;
using System.Windows.Controls;
using System.Reactive.Linq;
using FastAppFramework.Demo.Views;
using Prism.Ioc;
using MaterialDesignExtensions.Model;

namespace FastAppFramework.Demo.ViewModels
{
    public class FirstWizardFrameViewModel : BindableBase
    {
#region Commands
        public AsyncReactiveCommand<StepperNavigationEventArgs> ContinueNavigationCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand<StepperNavigationEventArgs> CancelNavigationCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public IRegion RootRegion => this._regionManager.Regions[FastWpfApplication.RootRegionName];

        public ReactiveCollection<IStep> Steps
        {
            get; private set;
        }
        public ReactivePropertySlim<IStep?> ActiveStep
        {
            get; private set;
        }
#endregion

#region Fields
        private IRegionManager _regionManager;
        private IMetroDialogService _dialogService;
#endregion

#region Constructor/Destructor
        public FirstWizardFrameViewModel(IRegionManager regionManager, IMetroDialogService dialogService)
        {
            // Setup Fields.
            {
                this._regionManager = regionManager;
                this._dialogService = dialogService;
            }

            // Setup Properties.
            {
                this.Steps = new ReactiveCollection<IStep>(){
                    new Step(){ Header = new StepTitleHeader(){ FirstLevelTitle = "License Agreement" }, Content = new LicenseAgreementPageViewModel() },
                    new Step(){ Header = new StepTitleHeader(){ FirstLevelTitle = "Overview" }, Content = new OverviewPageViewModel() },
                    new Step(){ Header = new StepTitleHeader(){ FirstLevelTitle = "Color Theme" }, Content = new ThemePageViewModel() },
                }.AddTo(this);
                this.ActiveStep = new ReactivePropertySlim<IStep?>().AddTo(this);
            }

            // Setup Commands.
            {
                this.ContinueNavigationCommand = new AsyncReactiveCommand<StepperNavigationEventArgs>()
                    .WithSubscribe(async (e) => {
                        if (!(e.CurrentStep.Content is ThemePageViewModel))
                            return;

                        this.RootRegion.RequestNavigate(FastWpfApplication.MainFrameName);
                        await Task.Run(() => {});
                    }).AddTo(this);
                this.CancelNavigationCommand = this.ActiveStep.Select(v => (v?.Content is LicenseAgreementPageViewModel)).ToAsyncReactiveCommand<StepperNavigationEventArgs>()
                    .WithSubscribe(async (e) => {
                        var res = await this._dialogService.ShowMessageAsync("Warning", "You cannot use this application if you decline the license. Are you sure to quit this application?", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);
                        if (res == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
                            Application.Current.Shutdown();
                    }).AddTo(this);
            }

            // Subscribes.
            {
            }
        }
#endregion
    }
}