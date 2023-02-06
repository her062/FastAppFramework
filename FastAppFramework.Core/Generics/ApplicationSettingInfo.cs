using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core.Generics
{
    public class ApplicationSettingInfo<T> : ApplicationSettingInfo
    {
#region Fields
#endregion

#region Constructor/Destructor
        public ApplicationSettingInfo() : base(typeof(T))
        {
        }
        public ApplicationSettingInfo(string key) : base(typeof(T), key)
        {
        }
        public ApplicationSettingInfo(string key, Version? version) : base(typeof(T), key, version)
        {
        }
#endregion
    }
}