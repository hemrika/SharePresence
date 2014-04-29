// -----------------------------------------------------------------------
// <copyright file="HTML5Image.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.IO;
    using System.Web.UI;
    using System.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HTML5Image : SPFieldMultiColumn
    {
        public HTML5Image(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public HTML5Image(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new HTML5ImageField(value);
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new HTML5ImageControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetFieldValueAsHtml(object value)
        {
            HTML5ImageControl control = new HTML5ImageControl();
            control.ControlMode = SPControlMode.Display;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            control.RenderControl(writer2);
            return writer.ToString();
        }

        public override string GetFieldValueAsText(object value)
        {
            HTML5ImageField field = (HTML5ImageField)value;
            if (field != null)
            {
                return field.ToString();
            }
            return string.Empty;
        }

        public override string GetFieldValueForEdit(object value)
        {
            return base.GetFieldValueForEdit(value);
        }

    }
}
