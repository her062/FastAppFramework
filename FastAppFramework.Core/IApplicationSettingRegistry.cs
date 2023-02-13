using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core
{
    public static class IApplicationSettingRegistryExtensions
    {
        public static void Register<T>(this IApplicationSettingRegistry self) => self.Register(new ApplicationSettingInfo<T>());
        public static void Register<T>(this IApplicationSettingRegistry self, T? defaultValue) => self.Register(new ApplicationSettingInfo<T>(){ DefaultValue = defaultValue });
        public static void Register<T>(this IApplicationSettingRegistry self, string key, T? defaultValue) => self.Register(new ApplicationSettingInfo<T>(key){ DefaultValue = defaultValue });
        public static void Register<T>(this IApplicationSettingRegistry self, string key, T? defaultValue, Variability variability) => self.Register(new ApplicationSettingInfo<T>(key){ DefaultValue = defaultValue, Variability = variability });
    }

    public interface IApplicationSettingRegistry
    {
        object? this[string key] { set; }
#region Public Functions
        void Register(ApplicationSettingInfo type);
#endregion
    }
}