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
    //using Microsoft.SharePoint.MobileControls;
    using Microsoft.SharePoint.Mobile.WebControls;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HTML5Section : SPFieldText
    {
        public HTML5Section(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public HTML5Section(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new HTML5SectionField(value);
        }

        public new Microsoft.SharePoint.Mobile.WebControls.SPMobileBaseFieldControl FieldRenderingMobileControl
        {
            get
            {
                SPMobileBaseFieldControl fieldControl = new HTML5SectionMobileControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new HTML5SectionControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetFieldValueAsHtml(object value)
        {
            HTML5SectionControl control = new HTML5SectionControl();
            control.ControlMode = SPControlMode.Display;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            control.RenderControl(writer2);
            return writer.ToString();
        }

        public override string GetFieldValueAsText(object value)
        {
            HTML5SectionField field = (HTML5SectionField)value;
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
