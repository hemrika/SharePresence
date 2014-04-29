using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Hemrika.SharePresence.WebSite.Page
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct StateFlagToStateStringEntry
    {
        internal AuthoringStates stateFlag;
        internal string stateString;
        internal StateFlagToStateStringEntry(AuthoringStates flag, string s)
        {
            this.stateFlag = flag;
            this.stateString = s;
        }
    }
}
