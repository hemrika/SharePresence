using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Common.UI
{
    public class EnhancedWebPartPropertyAttribute : Attribute
    {
        public bool IsEnhanced { get; private set; }

        public EnhancedWebPartPropertyAttribute(bool isEnhanced)
        {
            IsEnhanced = isEnhanced;
        }
    }
}
