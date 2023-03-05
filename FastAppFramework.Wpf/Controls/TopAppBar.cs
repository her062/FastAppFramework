using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FastAppFramework.Wpf
{
    public class TopAppBar : Control
    {
#region Constants
        public enum BarType
        {
            CenterAligned,
            Small,
            Medium,
            Large,
        }

        public enum LeadingNavigationType
        {
            None,
            Menu,
            Back,
        }

        private const string PART_Container = "PART_Container";
        private const string PART_LeadingNavigationIcon = "PART_LeadingNavigationIcon";
        private const string PART_TrailingIcon = "PART_TrailingIcon";
        private const string PART_MenuButton = "PART_MenuButton";
        private const string PART_BackButton = "PART_BackButton";
        private const string PART_Headline = "PART_Headline";
#endregion

#region Events
        public event RoutedEventHandler Back
        {
            add => AddHandler(BackEvent, value);
            remove => RemoveHandler(BackEvent, value);
        }
        public static readonly RoutedEvent BackEvent = EventManager.RegisterRoutedEvent(
            nameof(Back),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TopAppBar)
        );
#endregion

#region Commands
        public ICommand? BackCommand
        {
            get => (ICommand?)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }
        public static readonly DependencyProperty BackCommandProperty = DependencyProperty.Register(
            nameof(BackCommand),
            typeof(ICommand),
            typeof(TopAppBar),
            new PropertyMetadata(null, BackCommandPropertyChanged)
        );
#endregion

#region Properties
        public BarType Type
        {
            get => (BarType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type),
            typeof(BarType),
            typeof(TopAppBar),
            new PropertyMetadata(BarType.Small)
        );

        public LeadingNavigationType LeadingNavigation
        {
            get => (LeadingNavigationType)GetValue(LeadingNavigationProperty);
            set => SetValue(LeadingNavigationProperty, value);
        }
        public static readonly DependencyProperty LeadingNavigationProperty = DependencyProperty.Register(
            nameof(LeadingNavigation),
            typeof(LeadingNavigationType),
            typeof(TopAppBar),
            new PropertyMetadata(LeadingNavigationType.None)
        );

        public string? Headline
        {
            get => (string?)GetValue(HeadlineProperty);
            set => SetValue(HeadlineProperty, value);
        }
        public static readonly DependencyProperty HeadlineProperty = DependencyProperty.Register(
            nameof(Headline),
            typeof(string),
            typeof(TopAppBar),
            new PropertyMetadata()
        );

        public bool IsShowingNavigation
        {
            get => (bool)GetValue(IsShowingNavigationProperty);
            set => SetValue(IsShowingNavigationProperty, value);
        }
        public static readonly DependencyProperty IsShowingNavigationProperty = DependencyProperty.Register(
            nameof(IsShowingNavigation),
            typeof(bool),
            typeof(TopAppBar),
            new PropertyMetadata(false)
        );

        public StackPanel? TrailingIcon
        {
            get => (StackPanel?)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }
        public static readonly DependencyProperty TrailingIconProperty = DependencyProperty.Register(
            nameof(TrailingIcon),
            typeof(StackPanel),
            typeof(TopAppBar),
            new FrameworkPropertyMetadata(null, TrailingIconPropertyChanged)
        );
#endregion

#region Fields
        private Button? _backButton;
#endregion

#region Constructor/Destructor
        public TopAppBar()
        {
        }
#endregion

#region Public Functions
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._backButton = Template.FindName(PART_BackButton, this) as Button;
            if (this._backButton != null)
            {
                this._backButton.Command = this.BackCommand;
            }
        }
#endregion

#region Private Functions
        private static void BackCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TopAppBar;
            if ((obj != null) && (obj._backButton != null))
                obj._backButton.Command = obj.BackCommand;
        }

        private static void TrailingIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as StackPanel;
            if (item == null)
                return;

            item.Orientation = Orientation.Horizontal;
        }
#endregion
    }
}