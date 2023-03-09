using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Ioc;

namespace FastAppFramework.Core
{
    public class ApplicationSettingContainer : ModelBase, IApplicationSettingRegistry, IApplicationSettingProvider
    {
#region Internal Classes
        [JsonObject(MemberSerialization.OptIn)]
        protected class VersioningProperty
        {
#region Properties
            public Version Version
            {
                get => this._version;
                set => this._version = value;
            }
            public object? Value
            {
                get => this._value;
                set => this._value = value;
            }
#endregion

#region Fields
            [JsonProperty("_version")]
            private Version _version;
            [JsonProperty("_value")]
            private object? _value;
#endregion

#region Constructor/Destructor
            public VersioningProperty() : this(new Version(1, 0), null)
            {
            }
            public VersioningProperty(string version, object? value) : this(new Version(version), value)
            {
            }
            public VersioningProperty(Version version, object? value)
            {
                // Setup Fields.
                {
                    this._version = version;
                    this._value = value;
                }
            }
#endregion
        }
        protected class ApplicationSettingValues : VersioningProperty
        {
#region Properties
            public new Dictionary<string, object?> Value
            {
                get => (Dictionary<string, object?>)base.Value!;
            }
#endregion

#region Constructor/Destructor
            public ApplicationSettingValues() : base(FastApplication.Current.SettingVersion, new Dictionary<string, object?>())
            {
            }
#endregion
        }
#endregion

#region Constants
        public const string DefaultFileName = "app.settings";
#endregion

#region Properties
        public string Path
        {
            get => this._path;
            private set => SetValue(ref this._path, value);
        }

        public object? this[string key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }
#endregion

#region Fields
        private string _path;
        private IContainerRegistry _containerRegistry;
        private List<ApplicationSettingInfo> _types;
        private ApplicationSettingValues _values;
#endregion

#region Constructor/Destructor
        public ApplicationSettingContainer(IContainerRegistry containerRegistry)
        {
            // Setup Fields.
            {
                this._path = System.IO.Path.Combine(FastApplication.Current.Environment.ApplicationDataFolder, DefaultFileName);
                this._containerRegistry = containerRegistry;
                this._types = new List<ApplicationSettingInfo>();
                this._values = new ApplicationSettingValues();
            }
        }
#endregion

#region Public Functions
        public void Register(ApplicationSettingInfo type)
        {
            if (this._types.FirstOrDefault(v => (v.Key == type.Key)) != null)
                throw new ArgumentException($"'{type.Key}' is already registered");
            this._types.Add(type);

            var obj = ApplicationSettingLocator.Resolve(type);
            if (type.Version != null)
                obj = new VersioningProperty(type.Version, obj);

            this._values.Value.Add(type.Key, obj);
            if (!type.Type.IsPrimitive && type.Type != typeof(string) && type.Type != typeof(decimal))
                this._containerRegistry.Register(type.Type, () => this.GetValue(type.Key));
            OnPropertyChanged(type.Key);
        }

        public void Load(string? path = null)
        {
            path ??= this.Path;

            // Deserialize a json file.
            JObject? root = null;
            try {
                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        var json = reader.ReadToEnd();
                        root = JsonConvert.DeserializeObject<JObject>(json);
                    }
                }
            } catch (Exception ex) {
                FastApplication.Current.Logger.LogWarning(ex, $"Cannot deserialize {path}");
            }

            if (root != null)
            {
                // Check setting schema version.
                var ver = root["_version"]?.ToObject<Version>();
                if (ver == null)
                    throw new ApplicationException("Cannot deserialize the version of settings");

                if (this._values.Version.Major != ver.Major)
                {
                    // Major version is changed.
                    // The value on assembly will be adopted.
                    FastApplication.Current.Logger.LogWarning($"The major version of settings is changed: {ver.Major} -> {this._values.Version}");
                }
                else
                {
                    // The deserialized value will be adopted.
                    var items = root["_value"] as JObject;
                    if (items == null)
                        throw new ApplicationException("Cannot deserialize settings");

                    foreach (var item in items)
                    {
                        var token = item.Value;
                        ApplicationSettingInfo? type = this._types.FirstOrDefault(v => (v.Key == item.Key));
                        if (type == null)
                        {
                            // The value of unknown key is kept as is.
                            FastApplication.Current.Logger.LogWarning($"A setting('{item.Key}') is not registered");
                            if (!this._values.Value.ContainsKey(item.Key))
                                this._values.Value.Add(item.Key, token);
                            SetValueInternal(item.Key, token);
                            continue;
                        }

                        var obj = token as JObject;
                        if (obj != null)
                        {
                            // The deserialized type is VersioningProperty.
                            var keys = obj.Properties().Select(v => v.Name).ToList();
                            if (keys.Count == 2 && keys.Contains("_version") && keys.Contains("_value"))
                            {
                                // Get the version of deserialized value.
                                ver = obj.GetValue("_version")?.ToObject<Version>();
                                // Get the version on assembly.
                                if (type.Version != null && ver != null && type.Version.Major != ver.Major)
                                {
                                    // The value on assembly will be adopted because the following reasons:
                                    // The assembly type has version attribute.
                                    // & Success to obtain the deserialized version.
                                    // & The major version is changed.
                                    FastApplication.Current.Logger.LogInformation($"The major version of a setting('{item.Key}') is changed: {ver.Major} -> {type.Version}");
                                    continue;
                                }

                                // Get the deserialized value.
                                token = obj.GetValue("_value");
                            }
                        }

                        // Set the value of a setting.
                        SetValueInternal(item.Key, token?.ToObject(type.Type));
                    }
                }
            }

            this.Path = path;
            FastApplication.Current.Logger.LogInformation($"Application Settings({this.Path}) is loaded");
        }
        public void Save(string? path = null)
        {
            path ??= this.Path;

            // Create folder.
            {
                var folder = System.IO.Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(folder))
                    System.IO.Directory.CreateDirectory(folder);
            }

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None))
            {
                using (var writer = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    ApplicationSettingValues values = new ApplicationSettingValues();
                    foreach (var value in this._values.Value)
                    {
                        ApplicationSettingInfo? type = this._types.FirstOrDefault(v => (v.Key == value.Key));
                        if ((type != null) && type.Variability.HasFlag(Variability.Volatile))
                            continue;
                        values.Value.Add(value.Key, value.Value);
                    }

                    var json = JsonConvert.SerializeObject(values, Formatting.Indented);
                    writer.Write(json);
                }
            }

            this.Path = path;
            FastApplication.Current.Logger.LogInformation($"Application Settings is saved to a file({this.Path})");
        }

        public object? GetValue(string key)
        {
            var obj = this._values.Value[key];
            if (obj is VersioningProperty)
                obj = ((VersioningProperty)obj).Value;

            return obj;
        }
        public void SetValue(string key, object? value)
        {
            var type = this._types.First(v => (v.Key == key));
            if (type.Variability.HasFlag(Variability.Immutable))
                throw new InvalidOperationException($"Cannot set the value of an immutable setting('{key}')");

            SetValueInternal(key, value);
        }
        public void ClearValue(string? key = null)
        {
            if (key == null)
            {
                ClearAllValue();
                return;
            }

            var type = this._types.First(v => (v.Key == key));
            SetValue(key, ApplicationSettingLocator.Resolve(type));
        }
#endregion

#region Private Functions
        private void SetValueInternal(string key, object? value)
        {
            var obj = this._values.Value[key] as VersioningProperty;
            if (obj != null) obj.Value = value;
            else this._values.Value[key] = value;

            OnPropertyChanged(key);
        }
        private void ClearAllValue()
        {
            foreach (var type in this._types)
                ClearValue(type.Key);
        }
#endregion
    }
}