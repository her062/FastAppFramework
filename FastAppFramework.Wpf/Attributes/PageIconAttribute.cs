using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace FastAppFramework.Wpf
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageIconAttribute : Attribute
    {
#region Fields
        [TypeConverter(typeof(GeometryConverter))]
        public string Data
        {
            get; set;
        }
        public bool ShowInSideNavigationBar
        {
            get; set;
        }
        public bool ShowInTopAppBar
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public PageIconAttribute(string data)
        {
            // Setup Fields.
            {
                this.Data = data;
                this.ShowInSideNavigationBar = true;
                this.ShowInTopAppBar = true;
            }
        }
#endregion

#region Public Functions
        public static PageIconAttribute? Get(Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(PageIconAttribute)) as PageIconAttribute;
        }
#endregion
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MaterialDesignPageIconAttribute : PageIconAttribute
    {
#region Fields
        public PackIconKind Kind
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public MaterialDesignPageIconAttribute(PackIconKind kind) : base((new PackIcon(){ Kind = kind }).Data!)
        {
            // Setup Fields.
            {
                this.Kind = kind;
            }
        }
#endregion
    }
}