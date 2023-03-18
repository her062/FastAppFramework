using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.ModularityDemo.ViewModels
{
    public class ModulePageViewModel : BindableBase
    {
#region Properties
        public ReactivePropertySlim<string> Body
        {
            get; private set;
        }
#endregion

#region Constructor/Destructor
        public ModulePageViewModel()
        {
            // Setup Properties.
            {
                this.Body = new ReactivePropertySlim<string>("This is modularity demo page").AddTo(this);
            }
        }
#endregion
    }
}