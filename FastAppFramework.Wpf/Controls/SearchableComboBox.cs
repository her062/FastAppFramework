using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FastAppFramework.Wpf
{
    public class SearchableComboBox : CustomPopupComboBox
    {
#region Properties
        public string? FilterText
        {
            get => (string?)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }
        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register(
            nameof(FilterText),
            typeof(string),
            typeof(SearchableComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FilterTextPropertyChanged)
        );

        public Predicate<(object? Item, string Filter)>? Filter
        {
            get => (Predicate<(object?, string)>?)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            nameof(Filter),
            typeof(Predicate<(object?, string)>),
            typeof(SearchableComboBox),
            new FrameworkPropertyMetadata(null, FilterPropertyChanged)
        );

        public string? FilteringPropertyName
        {
            get => (string?)GetValue(FilteringPropertyNameProperty);
            set => SetValue(FilteringPropertyNameProperty, value);
        }
        public static readonly DependencyProperty FilteringPropertyNameProperty = DependencyProperty.Register(
            nameof(FilteringPropertyName),
            typeof(string),
            typeof(SearchableComboBox),
            new FrameworkPropertyMetadata(null, FilteringPropertyNamePropertyChanged)
        );
#endregion

#region Constructor/Destructor
        public SearchableComboBox()
        {
            // Setup Properties.
            {
                this.Filter = (v) => {
                    var value = v.Item?.ToString();
                    return (value?.IndexOf(v.Filter, StringComparison.OrdinalIgnoreCase) >= 0);
                };
            }
        }
#endregion

#region Private Functions
        private static void FilterTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SearchableComboBox;
            if (obj == null)
                throw new InvalidOperationException();

            obj.UpdateFilter();
        }
        private static void FilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SearchableComboBox;
            if (obj == null)
                throw new InvalidOperationException();

            obj.UpdateFilter();
        }
        private static void FilteringPropertyNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SearchableComboBox;
            if (obj == null)
                throw new InvalidOperationException();

            obj.UpdateFilter();
        }

        private void UpdateFilter()
        {
            Predicate<object>? filter = null;
            if ((this.Filter != null) && !string.IsNullOrEmpty(this.FilterText))
                filter = (v) => {
                    object? value = v;
                    if (!string.IsNullOrEmpty(this.FilteringPropertyName))
                        value = v.GetType().GetProperty(this.FilteringPropertyName)?.GetValue(v);
                    return this.Filter((value, this.FilterText));
                };

            if ((this.SelectedIndex >= 0) && (filter?.Invoke(this.SelectedItem) == false))
                this.SelectedIndex = -1;

            this.Items.Filter = filter;
            this.Items.Refresh();
        }
#endregion
    }
}