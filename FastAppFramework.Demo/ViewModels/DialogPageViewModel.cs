using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;
using Microsoft.Extensions.Logging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Demo.ViewModels
{
    public class DialogPageViewModel : BindableBase
    {
#region Commands
        public AsyncReactiveCommand ShowInputDialogCommand
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
        private IMetroDialogService _dialogService;
#endregion

#region Constructor/Destructor
        public DialogPageViewModel(IMetroDialogService dialogService)
        {
            // Setup Fields.
            {
                this._dialogService = dialogService;
            }

            // Setup Properties.
            {
                this.ShowInputDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var ret = await this._dialogService.ShowInputAsync("Input Dialog", "Please input any text");
                        FastWpfApplication.Current.Logger.LogInformation($"Input Text: {ret}");
                    }).AddTo(this);
                this.ShowMessageDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var ret = await this._dialogService.ShowMessageAsync("Message Dialog", "This is sample message dialog", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);
                        FastWpfApplication.Current.Logger.LogInformation($"MessageDialog Result: {ret}");
                    }).AddTo(this);
                this.ShowProgressDialogCommand = new AsyncReactiveCommand()
                    .WithSubscribe(async () => {
                        var progress = await this._dialogService.ShowProgressAsync("Progress Dialog", "This is sample progress dialog", true);
                        await Task.Run(() => {
                            progress.Minimum = 0; progress.Maximum = 100;
                            for (double v = progress.Minimum; v < progress.Maximum; v += 1.0)
                            {
                                if (progress.IsCanceled)
                                    break;
                                progress.SetProgress(v);
                                Thread.Sleep(100);
                            }
                        });
                        await progress.CloseAsync();
                    }).AddTo(this);
            }
        }
#endregion
    }
}