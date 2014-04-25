using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    internal class Helper
    {
        public static void AddStringAttributeIfNotEmpty(HtmlTextWriter writer, string attribName, string value)
        {
            if (!string.IsNullOrEmpty(value))
                writer.AddAttribute(attribName, value);
        }
        public static void AddBooleanAttribute(HtmlTextWriter writer, string attribName, bool value)
        {
            if (value)
            {
                writer.AddAttribute(attribName, Boolean.TrueString.ToLower());
            }
            else
            {
                writer.AddAttribute(attribName, Boolean.FalseString.ToLower());
            }
        }
        public static void AddLowerCaseEnumIfNotZero<T>(HtmlTextWriter writer, string attribName, T value) where T: struct
        {
            var ignoreValue = (T)Enum.ToObject(typeof(T), 0);
            AddLowerCaseEnumIfNotValue<T>(writer, attribName, value, ignoreValue);
        }

        public static void AddLowerCaseEnumIfNotValue<T>(HtmlTextWriter writer, string attribName, T value, T ignoreValue) where T : struct
        {
            if (value.Equals(ignoreValue))
                return;
            writer.AddAttribute(attribName, value.ToString().ToLower());
        }
        public static void AddFloatAttributeIfNotDefault(HtmlTextWriter writer, string attribName, float value, float defaultValue)
        {
            if (value != defaultValue)
                writer.AddAttribute(attribName, value.ToString());
        }
        public static void AddUrlAttributeIfNotEmpty(HtmlTextWriter writer, string attribName, string value, Control control)
        {
            if (string.IsNullOrEmpty(value))
                return;

            string resolved = control.ResolveClientUrl(value);
            writer.AddAttribute(attribName, resolved);
        }
    }
}
