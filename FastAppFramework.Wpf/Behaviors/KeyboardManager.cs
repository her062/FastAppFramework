using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FastAppFramework.Wpf
{
    public static class KeyboardManager
    {
#region Properties
        public static readonly DependencyProperty FocusedElementProperty = DependencyProperty.RegisterAttached(
            "FocusedElement",
            typeof(FrameworkElement),
            typeof(KeyboardManager),
            new PropertyMetadata(FocusedElementPropertyChanged)
        );
        public static FrameworkElement? GetFocusedElement(DependencyObject d)
        {
            return (FrameworkElement?)d.GetValue(FocusedElementProperty);
        }
        public static void SetFocusedElement(DependencyObject d, FrameworkElement? value)
        {
            d.SetValue(FocusedElementProperty, value);
        }
#endregion

#region Private Functions
        private static void FocusedElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = e.NewValue as FrameworkElement;
            if (obj != null)
                Keyboard.Focus(obj);
        }
#endregion
    }
}