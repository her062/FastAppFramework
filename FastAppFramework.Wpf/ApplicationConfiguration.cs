using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Core;

namespace FastAppFramework.Wpf
{
    public class ApplicationConfiguration
    {
#region Fields
        public string WindowTitle
        {
            get; set;
        }
        public Size WindowSize
        {
            get; set;
        }
        public string RootPage
        {
            get; set;
        }
        public string? HomePage
        {
            get; set;
        }
        public bool HasNotifyIcon
        {
            get; set;
        }
        public bool ExitConfirmation
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public ApplicationConfiguration()
        {
            // Setup Fields.
            {
                this.WindowTitle = ApplicationEnvironment.AssemblyName;
                this.WindowSize = new Size(800, 450);
                this.RootPage = FastWpfApplication.MainFrameName;
                this.HasNotifyIcon = false;
                this.ExitConfirmation = false;
            }
        }
#endregion
    }
}