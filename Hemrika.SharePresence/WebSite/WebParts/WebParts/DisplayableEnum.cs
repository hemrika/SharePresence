using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Hemrika.SharePresence.Common.ServiceLocation;

namespace Hemrika.SharePresence.WebSite.WebParts
{
    [Export(typeof(IDisplayableClass))]
    public class DisplayableEnum : IDisplayableClass
    {
        private Type enumType;

        public bool IsAppliable(Type propertyType)
        {
            if (propertyType.IsEnum)
            {
                enumType = propertyType;
            }

            return propertyType.IsEnum;
        }

        public bool IsControlInHeaderSection()
        {
            return false;
        }

        public Control CreateControl()
        {
            return new DropDownList();
        }

        public void SetControlValue(Control control, object value)
        {
            DropDownList ddl = (control as DropDownList);

            ddl.Items.Clear();
            int i = 0;
            foreach (MemberInfo member in Enum.GetNames(value.GetType()).Select(name => value.GetType().GetMember(name).Single()))
            {
                DescriptionAttribute attribute = (DescriptionAttribute)member.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    ddl.Items.Add(attribute.Description);
                }
                else
                {
                    ddl.Items.Add(value.ToString());
                }

                if (value.ToString() == member.Name)
                {
                    ddl.SelectedIndex = i;
                }
                i++;
            }

        }

        public object GetControlValue(Control control)
        {
            DropDownList ddl = (control as DropDownList);
            string enumValue = Enum.GetNames(enumType).Skip(ddl.SelectedIndex).First();
            return Enum.Parse(enumType, enumValue);
        }
    }
}
