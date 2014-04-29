// <copyright file="ContentTypeIdFieldType.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-11 16:41:31Z</date>
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

    /// <summary>
    /// Custom field type ContentTypeIdFieldType
    /// DisplayName: ContentTypeIdFieldType
    /// Description: ContentTypeIdFieldType
    /// </summary>
    public class ContentTypeIdFieldType : SPFieldMultiColumn
    {
        public ContentTypeIdFieldType(SPFieldCollection fields, string fieldName) : base(fields, fieldName)
        {
        }

        public ContentTypeIdFieldType(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new ContentTypeIdFieldValue(value);
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl control = new ContentTypeIdFieldTypeControl();
                control.FieldName = base.InternalName;
                return control;
            }
        }
    }
}

