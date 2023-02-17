using System.Windows.Input;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace FastAppFramework.Wpf
{
    public static class ContextMenuContainerExtensions
    {
        public static ContextMenuSeparator Add(this ContextMenuContainer self, int order = ContextMenuItem.DefaultOrder)
        {
            var obj = new ContextMenuSeparator(){ Order = order };
            self.Add(obj);
            return obj;
        }
        public static ContextMenuClickItem Add(this ContextMenuContainer self, string title, ICommand command, int order = ContextMenuItem.DefaultOrder)
        {
            var obj = new ContextMenuClickItem(){ Title = title, Command = command, Order = order };
            self.Add(obj);
            return obj;
        }
        public static ContextMenuCheckItem Add(this ContextMenuContainer self, string title, bool isChecked, int order = ContextMenuItem.DefaultOrder)
        {
            var obj = new ContextMenuCheckItem(){ Title = title, IsChecked = isChecked, Order = order };
            self.Add(obj);
            return obj;
        }
        public static ContextMenuSubMenu Add(this ContextMenuContainer self, string title, IEnumerable<ContextMenuItem>? items, int order = ContextMenuItem.DefaultOrder)
        {
            var obj = new ContextMenuSubMenu(){ Title = title };
            if (items != null)
                obj.Items.AddRange(items);
            self.Add(obj);
            return obj;
        }
    }
    public class ContextMenuContainer : ObservableCollection<ContextMenuItem>
    {
#region Constructor/Destructor
        public ContextMenuContainer()
        {
        }
        #endregion

#region Protected Functions
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems == null)
                        throw new ArgumentNullException(nameof(e.NewItems));
                    foreach (var item in e.NewItems)
                        ((ContextMenuItem)item).Owner = this;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems == null)
                        throw new ArgumentNullException(nameof(e.OldItems));
                    foreach (var item in e.OldItems)
                        ((ContextMenuItem)item).Owner = null;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems == null)
                        throw new ArgumentNullException(nameof(e.OldItems));
                    foreach (var item in e.OldItems)
                        ((ContextMenuItem)item).Owner = null;
                    if (e.NewItems == null)
                        throw new ArgumentNullException(nameof(e.NewItems));
                    foreach (var item in e.NewItems)
                        ((ContextMenuItem)item).Owner = this;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }

            base.OnCollectionChanged(e);
        }
#endregion
    }
}