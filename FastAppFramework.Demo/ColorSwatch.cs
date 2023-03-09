using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using MaterialDesignColors;

namespace FastAppFramework.Demo
{
    public class ColorSwatch
    {
#region Properties
        public string Name
        {
            get; set;
        }
        public ColorPair Pair
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public ColorSwatch(string name, ColorPair pair)
        {
            // Setup Properties.
            {
                this.Name = name;
                this.Pair = pair;
            }
        }
#endregion
    }
}