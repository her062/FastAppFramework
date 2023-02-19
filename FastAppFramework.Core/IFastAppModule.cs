using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Modularity;

namespace FastAppFramework.Core
{
    public interface IFastAppModule : IModule
    {
        void RegisterSettingTypes(IApplicationSettingRegistry settingRegistry)
        {
        }
    }
}