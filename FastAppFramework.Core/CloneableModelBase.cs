using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core
{
    public abstract class CloneableModelBase : ModelBase, ICloneable, IComparable
    {
#region Public Functions
        public object Clone()
        {
            var obj = CreateInstance();
            CopyTo(obj);
            return obj;
        }

        public abstract int CompareTo(object? obj);
        public abstract void CopyTo(object obj);
#endregion

#region Protected Functions
        protected abstract object CreateInstance();
#endregion
    }
}