using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.Common.ServiceLocation;

namespace Hemrika.SharePresence.WebSite.WebParts
{
    [Export(typeof(IDisplayableClass))]
    public class DisplayableInt : IDisplayableClass
    {
        public bool IsAppliable(Type propertyType)
        {
            return propertyType == typeof(Int16)
                || propertyType == typeof(Int32)
                || propertyType == typeof(Int64)
                || propertyType == typeof(UInt16)
                || propertyType == typeof(UInt32)
                || propertyType == typeof(UInt64);
        }

        public bool IsControlInHeaderSection()
        {
            return false;
        }

        public System.Web.UI.Control CreateControl()
        {
            TextBox txt = new TextBox();
            //txt.Style.Add("width", "176px");

            return txt;
        }

        public void SetControlValue(System.Web.UI.Control control, object value)
        {
            (control as TextBox).Text = Convert.ToString(value);
        }

        public object GetControlValue(System.Web.UI.Control control)
        {
            int result;
            if (Int32.TryParse((control as TextBox).Text, out result))
                return result;
            else
                return 0;
        }
    }
}
