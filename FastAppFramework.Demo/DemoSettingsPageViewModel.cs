using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Demo
{
    public class DemoSettingsPageViewModel : CloneableModelBindingBase<DemoSettings>
    {
#region Properties
        [Required(AllowEmptyStrings = false)]
        public ReactiveProperty<string?> Nonvolatile
        {
            get; private set;
        }
        [Required(AllowEmptyStrings = true)]
        public ReactiveProperty<string?> Volatile
        {
            get; private set;
        }
#endregion

#region Constructor/Destructor
        public DemoSettingsPageViewModel(DemoSettings model) : base(model, UpdateModelTrigger.Commit)
        {
            // Setup Properties.
            {
                this.Nonvolatile = this.Model.ToReactivePropertyAsSynchronized(o => o.Nonvolatile).SetValidateAttribute(() => this.Nonvolatile).Observe(this).AddTo(this);
                this.Volatile = this.Model.ToReactivePropertyAsSynchronized(o => o.Volatile).SetValidateAttribute(() => this.Volatile).Observe(this).AddTo(this);
            }
        }
#endregion
    }
}