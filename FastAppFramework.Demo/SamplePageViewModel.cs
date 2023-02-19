using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using FastAppFramework.Core;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;

namespace FastAppFramework.Demo
{
    public class SamplePageViewModel : BindableBase
    {
#region Commands
        public AsyncReactiveCommand ShowInputDialogCommand
        {
            get; private set;
        }
//        public AsyncReactiveCommand ShowLoginDialogCommand
//        {
//            get; private set;
//        }
        public AsyncReactiveCommand ShowMessageDialogCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand ShowProgressDialogCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public ReadOnlyReactivePropertySlim<DemoSettings?> Settings
        {
            get; private set;
        }
        public ReadOnlyReactivePropertySlim<string?> Nonvolatile
        {
            get; private set;
        }
#endregion

#region Fields
        private IMetroDialogService _dialogService;
#endregion

#region Constructor/Destructor
        public SamplePageViewModel(IMetroDialogService dialogService)
        {
            // Setup Fields.
            {
                this._dialogService = dialogService;
            }

            // Setup Properties.
            {
                this.Settings = FastWpfApplication.Current.Settings.Observe<DemoSettings>("demo").ToReadOnlyReactivePropertySlim().AddTo(this);
                this.Nonvolatile = this.Settings.ObserveProperty(o => o.Value!.Nonvolatile).ToReadOnlyReactivePropertySlim().AddTo(this);
            }

            // Setup Commands.
            {
                this.ShowInputDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var response = await this._dialogService.ShowInputAsync("Sample Input Dialog", "Please input any characters");
                    }).AddTo(this);

//                this.ShowLoginDialogCommand = new AsyncReactiveCommand()
//                    .WithSubscribe(async () => {
//                        var response = await this._dialogService.ShowLoginAsync("Sample Login Dialog", "Please input any Username/Password");
//                    }).AddTo(this);

                this.ShowMessageDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var response = await this._dialogService.ShowMessageAsync("Sample Message Dialog", "Please close it", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);
                    }).AddTo(this);

                this.ShowProgressDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var response = await this._dialogService.ShowProgressAsync("Sample Progress Dialog", "Please cancel it", true);
                        await Task.Run(() => {
                            response.Minimum = 0; response.Maximum = 100;
                            for (double i = response.Minimum; i <= response.Maximum; i += 1.0)
                            {
                                if (response.IsCanceled)
                                    break;
                                response.SetProgress(i);
                                Thread.Sleep(100);
                            }
                        });
                        await response.CloseAsync();
                    }).AddTo(this);
            }

            // Subscribes.
            {
                this.Nonvolatile.Subscribe(v => {
                    FastWpfApplication.Current.Logger.LogInformation($"Nonvolatile value is changed: {v}");
                }).AddTo(this);
            }
        }
#endregion
    }
}