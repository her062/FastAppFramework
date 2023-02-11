using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FastAppFramework.Wpf
{
    public class PreferencePage : NavigationPageBase
    {
#region Commands
        public ICommand? ApplyCommand
        {
            get => (ICommand?)GetValue(ApplyCommandProperty);
            set => SetValue(ApplyCommandProperty, value);
        }
        public static readonly DependencyProperty ApplyCommandProperty = DependencyProperty.Register(
            nameof(ApplyCommand),
            typeof(ICommand),
            typeof(PreferencePage),
            new PropertyMetadata(null)
        );

        public ICommand? RevertCommand
        {
            get => (ICommand?)GetValue(RevertCommandProperty);
            set => SetValue(RevertCommandProperty, value);
        }
        public static readonly DependencyProperty RevertCommandProperty = DependencyProperty.Register(
            nameof(RevertCommand),
            typeof(ICommand),
            typeof(PreferencePage),
            new PropertyMetadata(null)
        );
#endregion

#region Properties
        public bool IsDirty
        {
            get => (bool)GetValue(IsDirtyProperty);
            set => SetValue(IsDirtyProperty, value);
        }
        public static readonly DependencyProperty IsDirtyProperty = DependencyProperty.Register(
            nameof(IsDirty),
            typeof(bool),
            typeof(PreferencePage),
            new PropertyMetadata(false, OnPropertyChanged)
        );

        public bool HasErrors
        {
            get => (bool)GetValue(HasErrorsProperty);
            set => SetValue(HasErrorsProperty, value);
        }
        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register(
            nameof(HasErrors),
            typeof(bool),
            typeof(PreferencePage),
            new PropertyMetadata(false, OnPropertyChanged)
        );
#endregion

#region Constructor/Destructor
        public PreferencePage()
        {
        }
#endregion

#region Public Functions
        public void Apply()
        {
            if (this.ApplyCommand?.CanExecute(null) == true)
                this.ApplyCommand.Execute(null);
        }
        public void Revert()
        {
            if (this.RevertCommand?.CanExecute(null) == true)
                this.RevertCommand.Execute(null);
        }
#endregion
    }
}