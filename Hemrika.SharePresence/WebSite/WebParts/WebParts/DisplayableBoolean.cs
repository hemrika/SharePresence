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
    public class DisplayableBoolean : IDisplayableClass
    {

        public bool IsAppliable(Type propertyType)
        {
            return propertyType == typeof(bool);
        }

        public bool IsControlInHeaderSection()
        {
            return true;
        }

        public Control CreateControl()
        {
            return new CheckBox();
        }

        public void SetControlValue(Control control, object value)
        {
            (control as CheckBox).Checked = Convert.ToBoolean(value);
        }

        public object GetControlValue(Control control)
        {
            return (control as CheckBox).Checked;
        }
    }
}
