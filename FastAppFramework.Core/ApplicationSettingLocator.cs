using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FastAppFramework.Core
{
    public static class ApplicationSettingLocator
    {
        public static string GetKey(Type type)
        {
            string? key = null;

            var attr = ApplicationSettingAttribute.Get(type);
            if (attr != null)
                key = attr.Key;

            if (key == null)
            {
                var name = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(type.Name);
                key = char.ToLowerInvariant(name[0]) + name.Substring(1);
            }
            return key;
        }

        public static Version? GetVersion(Type type)
        {
            var value = ApplicationSettingAttribute.Get(type)?.Version;
            if (value == null)
                return null;
            return new Version(value);
        }

        public static object? Resolve(ApplicationSettingInfo type)
        {
            if (type.DefaultValue != null)
                return type.DefaultValue;

            try { return Activator.CreateInstance(type.Type); }
            catch (Exception ex) { FastApplication.Current.Logger.LogWarning(ex, ""); }

            return null;
        }
    }
}