using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Demo
{
    public class SamplePageViewModel : BindableBase
    {
#region Commands
        public AsyncReactiveCommand ShowInputDialogCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand ShowLoginDialogCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand ShowMessageDialogCommand
        {
            get; private set;
        }
        public AsyncReactiveCommand ShowProgressDialogCommand
        {
            get; private set;
        }
#endregion

#region Fields
        private IDialogService _dialogService;
#endregion

#region Constructor/Destructor
        public SamplePageViewModel(IDialogService dialogService)
        {
            // Setup Fields.
            {
                this._dialogService = dialogService;
            }

            // Setup Commands.
            {
                this.ShowInputDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var response = await this._dialogService.ShowInputAsync("Sample Input Dialog", "Please input any characters");
                    }).AddTo(this);

                this.ShowLoginDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var response = await this._dialogService.ShowLoginAsync("Sample Login Dialog", "Please input any Username/Password");
                    }).AddTo(this);

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
        }
#endregion
    }
}