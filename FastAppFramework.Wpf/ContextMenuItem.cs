using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FastAppFramework.Core;

namespace FastAppFramework.Wpf
{
    public class ContextMenuItem : ModelBase
    {
#region Constants
        public const int DefaultOrder = 0;

        public enum ContextMenuItemType
        {
            Separator,
            Clickable,
            Checkable,
            SubMenu,
        }
#endregion

#region Properties
        public IList<ContextMenuItem>? Owner
        {
            get => this._owner;
            internal set => this._owner = value;
        }

        public ContextMenuItemType Type => this._type;
        public int Order
        {
            get => this._order;
            set => SetValue(ref this._order, value);
        }

        public int Index => this.Owner?.IndexOf(this) ?? -1;
#endregion

#region Fields
        private IList<ContextMenuItem>? _owner;
        private ContextMenuItemType _type;
        private int _order;
#endregion

#region Constructor/Destructor
        public ContextMenuItem(ContextMenuItemType type)
        {
            // Setup Fields.
            {
                this._type = type;
                this._order = DefaultOrder;
            }
        }
#endregion
    }

    public class ContextMenuSeparator : ContextMenuItem
    {
#region Constructor/Destructor
        public ContextMenuSeparator() : base(ContextMenuItemType.Separator)
        {
        }
#endregion
    }

    public class ContextMenuClickItem : ContextMenuItem
    {
#region Properties
        public string? Title
        {
            get => this._title;
            set => SetValue(ref this._title, value);
        }
        public ICommand? Command
        {
            get => this._command;
            set => SetValue(ref this._command, value);
        }
#endregion

#region Fields
        private string? _title;
        private ICommand? _command;
#endregion

#region Constructor/Destructor
        public ContextMenuClickItem() : base(ContextMenuItemType.Clickable)
        {
        }
#endregion
    }

    public class ContextMenuCheckItem : ContextMenuItem
    {
#region Properties
        public string? Title
        {
            get => this._title;
            set => SetValue(ref this._title, value);
        }
        public bool IsEnabled
        {
            get => this._isEnabled;
            set => SetValue(ref this._isEnabled, value);
        }
        public bool IsChecked
        {
            get => this._isChecked;
            set => SetValue(ref this._isChecked, value);
        }
#endregion

#region Fields
        private string? _title;
        private bool _isEnabled;
        private bool _isChecked;
#endregion

#region Constructor/Destructor
        public ContextMenuCheckItem() : base(ContextMenuItemType.Checkable)
        {
            // Setup Fields.
            {
                this._isEnabled = true;
                this._isChecked = false;
            }
        }
#endregion
    }

    public class ContextMenuSubMenu : ContextMenuItem
    {
#region Properties
        public string? Title
        {
            get => this._title;
            set => SetValue(ref this._title, value);
        }
        public bool IsEnabled
        {
            get => this._isEnabled;
            set => SetValue(ref this._isEnabled, value);
        }

        public ContextMenuContainer Items => this._items;
#endregion

#region Fields
        private string? _title;
        private bool _isEnabled;
        private ContextMenuContainer _items;
#endregion

#region Constructor/Destructor
        public ContextMenuSubMenu() : base(ContextMenuItemType.SubMenu)
        {
            // Setup Fields.
            {
                this._isEnabled = true;
                this._items = new ContextMenuContainer();
            }
        }
#endregion
    }
}