using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Newtonsoft.Json;

namespace FastAppFramework.Demo
{
    [ApplicationSetting(Version = "1.0"), JsonObject(MemberSerialization.OptIn)]
    public class DemoSettings : CloneableModelBase
    {
#region Properties
        public string? Nonvolatile
        {
            get => this._nonvolatile;
            set => SetValue(ref this._nonvolatile, value);
        }
        public string? Volatile
        {
            get => this._volatile;
            set => SetValue(ref this._volatile, value);
        }
#endregion

#region Fields
        [JsonProperty("nonvolatile")]
        private string? _nonvolatile;
        private string? _volatile;
#endregion

#region Constructor/Destructor
        public DemoSettings()
        {
        }
#endregion

#region Public Functions
        public override int CompareTo(object? obj)
        {
            var item = obj as DemoSettings;
            if (item == null)
                throw new ArgumentException($"{obj?.GetType().Name} cannot convert to {typeof(DemoSettings).Name}");

            int res = 0;
            if ((res = string.Compare(this.Nonvolatile, item.Nonvolatile)) != 0)
                return res;
            if ((res = string.Compare(this.Volatile, item.Volatile)) != 0)
                return res;

            return 0;
        }
        public override void CopyTo(object obj)
        {
            var item = obj as DemoSettings;
            if (item == null)
                throw new ArgumentException($"{obj.GetType().Name} cannot convert to {typeof(DemoSettings).Name}");

            item.Nonvolatile = this.Nonvolatile;
            item.Volatile = this.Volatile;
        }
#endregion

#region Protected Functions
        protected override object CreateInstance()
        {
            return new DemoSettings();
        }
#endregion
    }
}