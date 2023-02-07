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
            get;
            set;
        }
        public static readonly DependencyProperty HeadlineProperty = DependencyProperty.Register(
            nameof(Headline),
            typeof(string),
            typeof(TopAppBar),
            new PropertyMetadata()
        );

        public bool IsShowingNavigation
        {
            get;
            set;
        }
        public static readonly DependencyProperty IsShowingNavigationProperty = DependencyProperty.Register(
            nameof(IsShowingNavigation),
            typeof(bool),
            typeof(TopAppBar),
            new PropertyMetadata(false)
        );

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

            if (this._backButton != null)
                this._backButton.Click -= BackButton_Click;

            this._backButton = Template.FindName(PART_BackButton, this) as Button;
            if (this._backButton != null)
                this._backButton.Click += BackButton_Click;
        }
#endregion

#region Private Functions
        private static void BackCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as TopAppBar;
            if (obj != null)
            {
                if (e.OldValue != null)
                    ((ICommand)e.OldValue).CanExecuteChanged -= obj.BackCommand_CanExecuteChanged;
                if (e.NewValue != null)
                    ((ICommand)e.NewValue).CanExecuteChanged += obj.BackCommand_CanExecuteChanged;
            }
        }
        private void BackCommand_CanExecuteChanged(object? sender, EventArgs e)
        {
            var command = sender as ICommand;
            if ((this._backButton != null) && (command != null))
                this._backButton.IsEnabled = command.CanExecute(null);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BackEvent, this));
            if ((this.BackCommand != null) && this.BackCommand.CanExecute(null))
                this.BackCommand.Execute(null);
        }
#endregion
    }
}