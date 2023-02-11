using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using FastAppFramework.Core;

namespace FastAppFramework.Wpf
{
    public class SideNavigationBarItem : ModelBase
    {
#region Fields
        public string View
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        [TypeConverter(typeof(GeometryConverter))]
        public string Icon
        {
            get; set;
        }

        public string? Group
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public SideNavigationBarItem(string view, string title)
        {
            // Setup Fields.
            {
                this.View = view;
                this.Title = title;
                this.Icon = string.Empty;
            }
        }
#endregion
    }

    public class SideNavigationBarItem<T> : SideNavigationBarItem
    {
        public SideNavigationBarItem(string title) : base(typeof(T).Name, title)
        {
        }
    }
}