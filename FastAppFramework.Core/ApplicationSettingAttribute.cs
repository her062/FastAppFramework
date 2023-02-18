using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core
{
    public class ApplicationSettingAttribute : Attribute
    {
#region Fields
        public string? Key { get; set; }
        public string? Version { get; set; }
#endregion

#region Constructor/Destructor
        public ApplicationSettingAttribute()
        {
        }
#endregion

#region Public Functions
        public static ApplicationSettingAttribute? Get(Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(ApplicationSettingAttribute)) as ApplicationSettingAttribute;
        }
#endregion
    }
}