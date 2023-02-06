using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Demo
{
    public class MainWindowViewModel
    {
#region Fields
        private DemoSettings _settings;
#endregion

#region Constructor/Destructor
        public MainWindowViewModel(DemoSettings settings)
        {
            // Setup Fields.
            {
                this._settings = settings;
                this._settings.Volatile = "Changed from MainWindowViewModel";
            }
        }
#endregion
    }
}