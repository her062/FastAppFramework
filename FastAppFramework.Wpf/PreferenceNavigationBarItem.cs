using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Wpf
{
    public class PreferenceNavigationBarItem : SideNavigationBarItem
    {
#region Properties
        public bool IsDirty
        {
            get => this._isDirty;
            set => SetValue(ref this._isDirty, value);
        }
        public bool HasErrors
        {
            get => this._hasErrors;
            set => SetValue(ref this._hasErrors, value);
        }
#endregion

#region Fields
        public PreferencePage? Page
        {
            get; set;
        }

        private bool _isDirty;
        private bool _hasErrors;
#endregion

#region Constructor/Destructor
        public PreferenceNavigationBarItem(SideNavigationBarItem source) : this(source.View, source.Title)
        {
            // Setup Fields.
            {
                this.Icon = source.Icon;
                this.Group = source.Group;
            }
        }
        public PreferenceNavigationBarItem(string view, string title) : base(view, title)
        {
        }
#endregion
    }
}