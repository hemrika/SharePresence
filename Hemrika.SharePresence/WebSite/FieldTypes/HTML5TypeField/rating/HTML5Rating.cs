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
    using System.Globalization;
    using System.Web.UI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HTML5Rating : SPFieldText
    {
        public HTML5Rating(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public HTML5Rating(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new HTML5RatingField(value);
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new HTML5RatingControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetFieldValueAsHtml(object value)
        {
            HTML5RatingControl control = new HTML5RatingControl();
            control.ControlMode = SPControlMode.Display;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            control.RenderControl(writer2);
            return writer.ToString();
        }

        public override string GetFieldValueAsText(object value)
        {
            HTML5RatingField field = (HTML5RatingField)value;
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
