// <copyright file="SchemaOrgFieldType.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-26 15:23:29Z</date>
namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.IO;

    /// <summary>
    /// Custom field type SchemaOrgFieldType
    /// DisplayName: SchemaOrgFieldType
    /// Description: SchemaOrgFieldType
    /// </summary>
    public class SchemaType : SPFieldText
    {
        public SchemaType(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public SchemaType(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new SchemaTypeField(value);
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl fieldControl = new SchemaTypeControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }
        public override string GetFieldValueAsHtml(object value)
        {
            SchemaTypeControl control = new SchemaTypeControl();
            control.ControlMode = SPControlMode.Display;
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            control.RenderControl(writer2);
            return writer.ToString();
        }

        public override string GetFieldValueAsText(object value)
        {
            SchemaTypeField field = (SchemaTypeField)value;
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