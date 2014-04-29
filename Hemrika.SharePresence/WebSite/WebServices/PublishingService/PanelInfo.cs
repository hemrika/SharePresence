using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.WebSite.WebServices.PublishingService
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PanelInfo
    {
        public string displayName;
        public string controlListId;
        public int panelTypeIdentifier;
    }
}
