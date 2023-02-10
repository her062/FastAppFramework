using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;

namespace FastAppFramework.Wpf
{
    public class SideNavigationBar : Control
    {
#region Constants
        private const string PART_SearchBar = "PART_SearchBar";
        private const string PART_ListBox = "PART_ListBox";
#endregion

#region Properties
        public string? Filter
        {
            get => (string?)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            nameof(Filter),
            typeof(string),
            typeof(SideNavigationBar),
            new FrameworkPropertyMetadata(null, FilterPropertyChanged){ BindsTwoWayByDefault = true }
        );

        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(SideNavigationBar),
            new PropertyMetadata(null)
        );

        public object? SelectedItem
        {
            get => (object?)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(SideNavigationBar),
            new FrameworkPropertyMetadata(null){ BindsTwoWayByDefault = true }
        );

        public DataTemplate? ItemTemplate
        {
            get => (DataTemplate?)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(SideNavigationBar),
            new PropertyMetadata(null)
        );
#endregion

#region Fields
        private ListBox? _listBox;
#endregion

#region Constructor/Destructor
        public SideNavigationBar()
        {
        }
#endregion

#region Public Functions
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._listBox = Template.FindName(PART_ListBox, this) as ListBox;
        }

        public void Validate()
        {
            var value = this.Filter;
            if (this._listBox != null)
            {
                Predicate<object> filter = (v) => (string.IsNullOrEmpty(value) || ((SideNavigationBarItem)v).Title.Contains(value, StringComparison.OrdinalIgnoreCase));
                if (this.SelectedItem != null && !filter(this.SelectedItem))
                    this.SelectedItem = null;

                this._listBox.Items.Filter = filter;
                this._listBox.Items.Refresh();
            }
        }
#endregion

#region Private Functions
        private static void FilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SideNavigationBar;
            obj?.Validate();
        }
#endregion
    }
}