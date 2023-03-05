using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FastAppFramework.Core.Comparers
{
    public class NaturalStringComparer : IComparer<string>
    {
#region Properties
        public static NaturalStringComparer Instance = new NaturalStringComparer();
#endregion

#region Public Functions
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

        public int Compare(string? x, string? y)
        {
            if (x == y)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            return StrCmpLogicalW(x, y);
        }
#endregion
    }
}