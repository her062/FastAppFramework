using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FastAppFramework.Core;
using Hardcodet.Wpf.TaskbarNotification;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf
{
    public class NotifyIconService : BindableBase, INotifyIconService
    {
#region Constants
        private const string PART_ToolTip = "PART_ToolTip";
        private const string PART_ContextMenu = "PART_ContextMenu";
#endregion

#region Properties
        public string? ToolTip
        {
            get => (string?)this._tooltip.Content;
            set => this._tooltip.Content = value;
        }
        public ICommand? DoubleClickCommand
        {
            get => this._data.DoubleClickCommand;
            set => this._data.DoubleClickCommand = value;
        }
#endregion

#region Fields
        private TaskbarIcon _data;
        private ToolTip _tooltip;

        private bool _disposed;
#endregion

#region Constructor/Destructor
        public NotifyIconService(string initialName, ContextMenuContainer container)
        {
            // Setup Fields.
            {
                var resource = new ResourceDictionary(){ Source = new Uri("pack://application:,,,/FastAppFramework.Wpf;component/Themes/NotifyIcon.xaml") };
                this._data = (TaskbarIcon)resource["NotifyIcon"];
                {
                    this._data.Icon = System.Drawing.Icon.ExtractAssociatedIcon(ApplicationEnvironment.AssemblyFile);
                }
                this._tooltip = (ToolTip)this._data.TrayToolTip;
                {
                    this._tooltip.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
                    this._tooltip.HasDropShadow = false;
                    this._tooltip.Content = initialName;
                }
            }

            // Setup Properties.
            {
                this._data.ContextMenu.ItemsSource = container;
                this._data.ContextMenu.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(ContextMenuItem.Order), System.ComponentModel.ListSortDirection.Ascending));
                this._data.ContextMenu.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(ContextMenuItem.Index), System.ComponentModel.ListSortDirection.Ascending));
            }

            // Setup Commands.
            {
                this.DoubleClickCommand = new ReactiveCommand()
                    .WithSubscribe(() => {
                        FastWpfApplication.Current.Activate();
                    }).AddTo(this);
            }
        }
        ~NotifyIconService()
        {
            Dispose(false);
        }
#endregion

#region Protected Functions
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this._data.Dispose();
                }

                _disposed = true;
            }
        }
#endregion
    }
}