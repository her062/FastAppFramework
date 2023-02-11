using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;

namespace FastAppFramework.Wpf
{
    public abstract class ModelBindingBase : BindableBase
    {
#region Properties
        public ModelBase Model => this._model;
#endregion

#region Fields
        private ModelBase _model;
#endregion

#region Constructor/Destructor
        protected ModelBindingBase(ModelBase model)
        {
            // Setup Fields.
            {
                this._model = model;
            }
        }
#endregion
    }

    public abstract class ModelBindingBase<T> : ModelBindingBase where T : ModelBase
    {
#region Properties
        public new T Model => (T)base.Model;
#endregion

#region Constructor/Destructor
        protected ModelBindingBase(T model) : base(model)
        {
        }
#endregion
    }
}