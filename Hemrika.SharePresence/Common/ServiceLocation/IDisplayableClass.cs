using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Reflection;

namespace Hemrika.SharePresence.Common.ServiceLocation
{
    public interface IDisplayableClass
    {
        bool IsAppliable(Type propertyType);

        bool IsControlInHeaderSection();

        Control CreateControl();
        
        void SetControlValue(Control control, object value);
        object GetControlValue(Control control);
    }
}
