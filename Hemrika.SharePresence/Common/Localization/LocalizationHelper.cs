using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.SharePoint.Utilities;
using System.Threading;

namespace Hemrika.SharePresence.Common.Localization
{
    public class LocalizationHelper
    {
        public static string Localize(string resourceName, string resourceFile)
        {
            // Replace this line with your localization code
            return SPUtility.GetLocalizedString("$Resources:" + resourceName, resourceFile, (uint)Thread.CurrentThread.CurrentUICulture.LCID);
        }
    }
}
