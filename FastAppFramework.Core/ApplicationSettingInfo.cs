using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core
{
    [Flags]
    public enum Variability
    {
        Normal = 0x00,
        Volatile = 0x01,
        Immutable = 0x02,
    }

    public class ApplicationSettingInfo
    {
#region Fields
        public Type Type { get; set; }
        public string Key { get; set; }
        public Version? Version { get; set; }
        public object? DefaultValue { get; set; }
        public Variability Variability { get; set; }
#endregion

#region Constructor/Destructor
        public ApplicationSettingInfo(Type type) : this(type, ApplicationSettingLocator.GetKey(type))
        {
        }
        public ApplicationSettingInfo(Type type, string key) : this(type, key, ApplicationSettingLocator.GetVersion(type))
        {
        }
        public ApplicationSettingInfo(Type type, string key, Version? version)
        {
            // Setup Fields.
            {
                this.Type = type;
                this.Key = key;
                this.Version = version;
                this.DefaultValue = null;
                this.Variability = Variability.Normal;
            }
        }
#endregion
    }

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