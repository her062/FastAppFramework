using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Prism.Regions;

namespace FastAppFramework.Wpf
{
    public abstract class NavigationPageBase : UserControl, INotifyPropertyChanged
    {
#region Events
        public event PropertyChangedEventHandler? PropertyChanged;
#endregion

#region Properties
        public string Title
        {
            get;
            set;
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(NavigationPageBase),
            new PropertyMetadata(string.Empty, OnPropertyChanged)
        );
#endregion

#region Constructor/Destructor
        public NavigationPageBase()
        {
            // Setup Properties.
            {
                var type = this.GetType();
                this.Title = NavigationPageAttribute.Get(type)?.Title ?? type.Name;
            }
        }
#endregion

#region Protected Functions
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion

#region Private Functions
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as NavigationPageBase;
            obj?.OnPropertyChanged(e.Property.Name);
        }
#endregion
    }
}