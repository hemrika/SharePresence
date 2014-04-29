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
    public class HTML5Footer : SPFieldText
    {
        public HTML5Footer(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public HTML5Footer(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new HTML5FooterField(value);
        }

        public new Microsoft.SharePoint.Mobile.WebControls.SPMobileBaseFieldControl FieldRenderingMobileControl
        {
            get
            {
                SPMobileBaseFieldControl fieldControl = new HTML5FooterMobileControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new HTML5FooterControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetFieldValueAsHtml(object value)
        {
            HTML5FooterControl control = new HTML5FooterControl();
            control.ControlMode = SPControlMode.Display;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            control.RenderControl(writer2);
            return writer.ToString();
        }

        public override string GetFieldValueAsText(object value)
        {
            try
            {

                HTML5FooterField field = (HTML5FooterField)value;
                if (field != null)
                {
                    return field.Text.ToString();
                }
            }
            catch { }

                return string.Empty;
        }

        public override string GetFieldValueForEdit(object value)
        {
            return base.GetFieldValueForEdit(value);

        }

    }
}
