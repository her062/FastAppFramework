using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace FastAppFramework.Core
{
    public static class IApplicationSettingProviderExtensions
    {
        public static T? GetValue<T>(this IApplicationSettingProvider self) => (T?)self.GetValue(ApplicationSettingLocator.GetKey(typeof(T)));
        public static T? GetValue<T>(this IApplicationSettingProvider self, string key) => (T?)self.GetValue(key);
        public static void SetValue<T>(this IApplicationSettingProvider self, T value) => self.SetValue(ApplicationSettingLocator.GetKey(typeof(T)), value);
        public static void ClearValue<T>(this IApplicationSettingProvider self) => self.ClearValue(ApplicationSettingLocator.GetKey(typeof(T)));

        public static IObservable<T?> Observe<T>(this IApplicationSettingProvider self, string key, bool subscribeOnInitialized = true) => new IApplicationSettingProvider.ApplicationSettingObservable<T>(self, key, subscribeOnInitialized);
    }

    public interface IApplicationSettingProvider : INotifyPropertyChanged
    {
#region Internal Classes
        public class ApplicationSettingObservable<T> : IObservable<T?>
        {
#region Internal Classes
            private class Subscriber : IDisposable
            {
#region Fields
                private IApplicationSettingProvider _settingProvider;
                private string _key;
                private IObserver<T?> _observer;
#endregion

#region Constructor/Destructor
                public Subscriber(IApplicationSettingProvider settingProvider, string key, IObserver<T?> observer)
                {
                    // Setup Fields.
                    {
                        this._settingProvider = settingProvider;
                        this._key = key;
                        this._observer = observer;
                    }

                    // Subscribes.
                    {
                        this._settingProvider.PropertyChanged += SettingProvider_PropertyChanged;
                    }
                }
#endregion

#region Public Functions
                public void Dispose()
                {
                    this._settingProvider.PropertyChanged -= SettingProvider_PropertyChanged;
                }

                public void OnNext()
                {
                    SettingProvider_PropertyChanged(this._settingProvider, new PropertyChangedEventArgs(this._key));
                }
#endregion

#region Private Functions
                private void SettingProvider_PropertyChanged(object? sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == this._key)
                        this._observer.OnNext(this._settingProvider.GetValue<T>(this._key));
                }
#endregion
            }
#endregion

#region Fields
            private IApplicationSettingProvider _settingProvider;
            private string _key;
            private bool _subscribeOnInitialized;
#endregion

#region Constructor/Destructor
            public ApplicationSettingObservable(IApplicationSettingProvider settingProvider, string key, bool subscribeOnInitialized = true)
            {
                // Setup Fields.
                {
                    this._settingProvider = settingProvider;
                    this._key = key;
                    this._subscribeOnInitialized = subscribeOnInitialized;
                }
            }
#endregion

#region Public Functions
            public IDisposable Subscribe(IObserver<T?> observer)
            {
                var obj = new Subscriber(this._settingProvider, this._key, observer);
                if (this._subscribeOnInitialized)
                    obj.OnNext();
                return obj;
            }
#endregion
        }
#endregion

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