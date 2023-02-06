using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FastAppFramework.Core;
using FastAppFramework.Wpf.ViewModels;
using FastAppFramework.Wpf.Views;
using Prism.Ioc;

namespace FastAppFramework.Wpf
{
    public abstract class FastWpfApplication : FastApplication
    {
#region Properties
        public static new FastWpfApplication Current => (FastWpfApplication)FastApplication.Current;
#endregion

#region Fields
        public ApplicationConfiguration Config
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        protected FastWpfApplication()
        {
            // Setup Fields.
            {
                this.Config = new ApplicationConfiguration();
            }
        }
#endregion

#region Protected Functions
        protected override Window CreateShell()
        {
            var obj = new MainWindow()
            {
                Title = this.Config.WindowTitle,
                Width = this.Config.WindowSize.Width,
                Height = this.Config.WindowSize.Height,
            };
            return obj;
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
        }
#endregion
    }
}