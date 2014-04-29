using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel.Composition;
using Hemrika.SharePresence.Common.ServiceLocation;

namespace Hemrika.SharePresence.WebSite.WebParts
{
    [Export(typeof(IDisplayableClass))]
    public class DisplayableString : IDisplayableClass
    {

        public bool IsAppliable(Type propertyType)
        {
            return propertyType == typeof(String);
        }

        public bool IsControlInHeaderSection()
        {
            return false;
        }

        public Control CreateControl()
        {
            TextBox txt = new TextBox();
            //txt.Style.Add("width", "176px");

            return txt;
        }


        public void SetControlValue(Control control, object value)
        {
            (control as TextBox).Text = Convert.ToString(value);
        }

        public object GetControlValue(Control control)
        {
            return (control as TextBox).Text;
        }
    }
}
