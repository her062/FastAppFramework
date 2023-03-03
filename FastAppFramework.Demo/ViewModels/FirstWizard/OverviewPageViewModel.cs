using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Demo.ViewModels
{
    public class OverviewPageViewModel : BindableBase
    {
#region Commands
        public ReactiveCommand<string?> OpenInBrowserCommand
        {
            get; private set;
        }
#endregion

#region Properties
#endregion

#region Constructor/Destructor
        public OverviewPageViewModel()
        {
            // Setup Commands.
            {
                this.OpenInBrowserCommand = new ReactiveCommand<string?>()
                    .WithSubscribe(v => {
                        if (!string.IsNullOrEmpty(v) && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Process.Start(new ProcessStartInfo(v){ UseShellExecute = true });
                    }).AddTo(this);
            }
        }
#endregion
    }
}