using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
#region Properties
        public ReactivePropertySlim<string?> Headline
        {
            get; private set;
        }
#endregion

#region Constructor/Destructor
        public MainPageViewModel()
        {
            // Setup Properties.
            {
                this.Headline = new ReactivePropertySlim<string?>("Welcome").AddTo(this);
            }
        }
#endregion
    }
}