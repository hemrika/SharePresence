using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hemrika.SharePresence.Common.Localization
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public LocalizedDescriptionAttribute(string resourceName, string resourceFileName)
            : base(LocalizationHelper.Localize(resourceName, resourceFileName))
        {

        }

    }
}
