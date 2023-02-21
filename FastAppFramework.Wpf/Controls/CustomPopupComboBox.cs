using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace FastAppFramework.Wpf
{
    public class CustomPopupComboBox : ComboBox
    {
#region Constants
        private const string PART_Popup = "PART_Popup";
#endregion

#region Properties
        public ControlTemplate PopupTemplate
        {
            get => (ControlTemplate)GetValue(PopupTemplateProperty);
            set => SetValue(PopupTemplateProperty, value);
        }
        public static readonly DependencyProperty PopupTemplateProperty = DependencyProperty.Register(
            nameof(PopupTemplate),
            typeof(ControlTemplate),
            typeof(CustomPopupComboBox),
            new FrameworkPropertyMetadata(null, PopupTemplatePropertyChanged)
        );

        public ControlTemplate? PopupHeaderTemplate
        {
            get => (ControlTemplate?)GetValue(PopupHeaderTemplateProperty);
            set => SetValue(PopupHeaderTemplateProperty, value);
        }
        public static readonly DependencyProperty PopupHeaderTemplateProperty = DependencyProperty.Register(
            nameof(PopupHeaderTemplate),
            typeof(ControlTemplate),
            typeof(CustomPopupComboBox),
            new FrameworkPropertyMetadata(null, PopupTemplatePropertyChanged)
        );

        public ControlTemplate? PopupFooterTemplate
        {
            get => (ControlTemplate?)GetValue(PopupFooterTemplateProperty);
            set => SetValue(PopupFooterTemplateProperty, value);
        }
        public static readonly DependencyProperty PopupFooterTemplateProperty = DependencyProperty.Register(
            nameof(PopupFooterTemplate),
            typeof(ControlTemplate),
            typeof(CustomPopupComboBox),
            new FrameworkPropertyMetadata(null, PopupTemplatePropertyChanged)
        );
#endregion

#region Fields
        private ComboBoxPopup? _popup;
#endregion

#region Constructor/Destructor
        public CustomPopupComboBox()
        {
        }
#endregion

#region Protected Functions
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._popup = Template.FindName(PART_Popup, this) as ComboBoxPopup;
            if (this._popup != null)
                this._popup.ClassicContentTemplate = ((ContentControl)this._popup.Child).Template = this.PopupTemplate;
        }
#endregion

#region Private Functions
        private static void PopupTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as CustomPopupComboBox;
            if (obj != null && obj._popup != null)
                obj._popup.ClassicContentTemplate = ((ContentControl)obj._popup.Child).Template = e.NewValue as ControlTemplate;
        }
#endregion
    }
}