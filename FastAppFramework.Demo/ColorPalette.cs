using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using MaterialDesignColors;

namespace FastAppFramework.Demo
{
    public class ColorPalette : ColorSwatch
    {
#region Properties
        public ColorPair SecondaryPair
        {
            get; set;
        }

        public IEnumerable<ColorSwatch> Swatches
        {
            get; set;
        }
        public IEnumerable<ColorSwatch> SecondarySwatches
        {
            get; set;
        }
#endregion

#region Constructor/Destructor
        public ColorPalette(string name, ColorPair pair, ColorPair secondaryPair, IEnumerable<ColorSwatch> swatches, IEnumerable<ColorSwatch> secondarySwatches) : base(name, pair)
        {
            // Set Properties.
            {
                this.SecondaryPair = secondaryPair;
                this.Swatches = swatches;
                this.SecondarySwatches = secondarySwatches;
            }
        }
#endregion
    }
}