using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Wpf
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SideNavigationBarAttribute : Attribute
    {
#region Constants
        public enum RegionType
        {
            Main,
            Preference,
            Custom,
        }
#endregion

#region Fields
        public RegionType Region
        {
            get; set;
        }
        public string Title
        {
            get; private set;
        }
        public string? View
        {
            get; set;
        }
        public string? Group
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public SideNavigationBarAttribute(string title)
        {
            // Setup Fields.
            {
                this.Region = RegionType.Main;
                this.Title = title;
            }
        }
#endregion

#region Public Functions
        public static SideNavigationBarAttribute? Get(Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(SideNavigationBarAttribute)) as SideNavigationBarAttribute;
        }
        public static SideNavigationBarItem? GetItem(Type type, out RegionType region)
        {
            region = RegionType.Custom;
            var attr = Get(type);
            if (attr == null)
                return null;

            var icon = PageIconAttribute.Get(type);
            if (icon?.ShowInSideNavigationBar != true)
                icon = null;

            region = attr.Region;
            return new SideNavigationBarItem(attr.View ?? type.Name, attr.Title)
            {
                Icon = icon?.Data ?? string.Empty,
                Group = attr.Group,
            };
        }
#endregion
    }
}