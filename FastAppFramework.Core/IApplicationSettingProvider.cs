using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core
{
    public static class IApplicationSettingProviderExtensions
    {
        public static T? GetValue<T>(this IApplicationSettingProvider self) => (T?)self.GetValue(ApplicationSettingLocator.GetKey(typeof(T)));
        public static void SetValue<T>(this IApplicationSettingProvider self, T value) => self.SetValue(ApplicationSettingLocator.GetKey(typeof(T)), value);
        public static void ClearValue<T>(this IApplicationSettingProvider self) => self.ClearValue(ApplicationSettingLocator.GetKey(typeof(T)));
    }

    public interface IApplicationSettingProvider
    {
#region Properties
        object? this[string key] { get; }
#endregion

#region Public Functions
        void Load(string? path = null);
        void Save(string? path = null);

        object? GetValue(string key);
        void SetValue(string key, object? value);
        void ClearValue(string key);
#endregion
    }
}