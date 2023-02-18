using System.Linq.Expressions;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using FastAppFramework.Core.Internals;

namespace FastAppFramework.Core
{
    public static class IApplicationSettingProviderExtensions
    {
        public static T? GetValue<T>(this IApplicationSettingProvider self)
            => (T?)self.GetValue(ApplicationSettingLocator.GetKey(typeof(T)));
        public static T? GetValue<T>(this IApplicationSettingProvider self, string key)
            => (T?)self.GetValue(key);
        public static void SetValue<T>(this IApplicationSettingProvider self, T value)
            => self.SetValue(ApplicationSettingLocator.GetKey(typeof(T)), value);
        public static void ClearValue<T>(this IApplicationSettingProvider self)
            => self.ClearValue(ApplicationSettingLocator.GetKey(typeof(T)));

        public static IObservable<T?> Observe<T>(this IApplicationSettingProvider self, string key, bool subscribeOnInitialized = true)
            => new PropertyChangedObservable<IApplicationSettingProvider, T>(self, key, v => v.GetValue<T>(key), subscribeOnInitialized);
        public static IObservable<T?> Observe<T>(this IApplicationSettingProvider self, bool subscribeOnInitialized = true)
            => Observe<T>(self, ApplicationSettingLocator.GetKey(typeof(T)), subscribeOnInitialized);
    }

    public interface IApplicationSettingProvider : INotifyPropertyChanged
    {
#region Properties
        object? this[string key] { get; }
#endregion

#region Public Functions
        void Load(string? path = null);
        void Save(string? path = null);

        object? GetValue(string key);
        void SetValue(string key, object? value);
        void ClearValue(string? key = null);
#endregion
    }
}