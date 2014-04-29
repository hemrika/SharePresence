using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using Hemrika.SharePresence.Common.ServiceLocation;

namespace Hemrika.SharePresence.WebSite.WebParts
{
    [Export(typeof(IDisplayableClass))]
    public class DisplayableSPUser : IDisplayableClass
    {

        public bool IsAppliable(Type propertyType)
        {
            return propertyType == typeof(SPUser);
        }

        public bool IsControlInHeaderSection()
        {
            return false;
        }

        public Control CreateControl()
        {
            PeopleEditor peopleEditor = new PeopleEditor();
            peopleEditor.AllowEmpty = true;
            peopleEditor.AllowTypeIn = true;
            peopleEditor.MaximumEntities = 1;
            peopleEditor.SelectionSet = "User";

            return peopleEditor;
        }

        public void SetControlValue(Control control, object value)
        {
            if (value != null)
                (control as PeopleEditor).CommaSeparatedAccounts = (value as SPUser).LoginName;
            else
                (control as PeopleEditor).CommaSeparatedAccounts = "";
        }

        public object GetControlValue(Control control)
        {
            string loginName = (control as PeopleEditor).CommaSeparatedAccounts.TrimEnd(';');
            return SPContext.Current.Web.EnsureUser(loginName);
        }
    }
}
