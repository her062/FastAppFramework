using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FastAppFramework.Wpf
{
    public class SearchBar : Control
    {
#region Properties
        public string? Text
        {
            get => (string?)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(SearchBar),
            new PropertyMetadata(null)
        );

        public string? Hint
        {
            get => (string?)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }
        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
            nameof(Hint),
            typeof(string),
            typeof(SearchBar),
            new PropertyMetadata("Search")
        );

        public bool HasClearButton
        {
            get => (bool)GetValue(HasClearButtonProperty);
            set => SetValue(HasClearButtonProperty, value);
        }
        public static readonly DependencyProperty HasClearButtonProperty = DependencyProperty.Register(
            nameof(HasClearButton),
            typeof(bool),
            typeof(SearchBar),
            new PropertyMetadata(true)
        );

        public CornerRadius? CornerRadius
        {
            get => (CornerRadius?)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(SearchBar),
            new PropertyMetadata(new CornerRadius(16))
        );
#endregion

#region Constructor/Destructor
        public SearchBar()
        {
        }
#endregion
    }
}