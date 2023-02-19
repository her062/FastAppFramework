using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FastAppFramework.Wpf
{
    public interface INotifyIconService : IDisposable
    {
#region Commands
        ICommand? DoubleClickCommand { get; set; }
#endregion

#region Properties
        string? ToolTip { get; set; }
        Icon Icon { get; set; }
#endregion
    }
}