using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core
{
    public interface IApplicationEnvironment
    {
        string ApplicationDataFolder { get; }
        string TempFolder { get; }
        string LogFolder { get; }
    }
}