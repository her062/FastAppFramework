using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Prism.Ioc;

namespace FastAppFramework.Wpf
{
    public interface IFastWpfAppModule : IFastAppModule
    {
#region Public Functions
        void RegisterNavigationTypes(IContainerRegistry containerRegistry);
#endregion
    }
}