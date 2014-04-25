using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Hemrika.SharePresence.Common.Localization
{
    public class LocalizedWebDisplayNameAttribute : WebDisplayNameAttribute
    {
        public LocalizedWebDisplayNameAttribute(string resourceName, string resourceFileName)
            : base(LocalizationHelper.Localize(resourceName, resourceFileName))
        {

        }

    }
}
