    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    //using Microsoft.SharePoint.MobileControls;
    using Microsoft.SharePoint.Mobile.WebControls;
    using System.IO;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{

    /// <summary>
    /// Custom field type HTML5PublishingDate
    /// DisplayName: HTML5PublishingDate
    /// Description: HTML5PublishingDate
    /// </summary>
    public class HTML5PublishingDate : SPFieldText
    {
        public HTML5PublishingDate(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public HTML5PublishingDate(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new HTML5PublishingDateField(value);
        }

        public new Microsoft.SharePoint.Mobile.WebControls.SPMobileBaseFieldControl FieldRenderingMobileControl
        {
            get
            {
                SPMobileBaseFieldControl fieldControl = new HTML5PublishingDateMobileControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new HTML5PublishingDateControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetFieldValueAsHtml(object value)
        {
            HTML5PublishingDateControl control = new HTML5PublishingDateControl();
            control.ControlMode = SPControlMode.Display;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            control.RenderControl(writer2);
            return writer.ToString();
        }

        public override string GetFieldValueAsText(object value)
        {
            HTML5PublishingDateField field = (HTML5PublishingDateField)value;
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
