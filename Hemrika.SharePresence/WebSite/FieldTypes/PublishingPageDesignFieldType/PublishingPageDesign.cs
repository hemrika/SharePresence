// <copyright file="PublishingPageDesign.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-02-15 11:56:31Z</date>
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
    /// Custom field type PublishingPageDesign
    /// DisplayName: Publishing Page Design
    /// Description: Publishing PageDesign Field
    /// </summary>
    public class PublishingPageDesign : SPFieldMultiColumn
    {
        public PublishingPageDesign(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public PublishingPageDesign(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new PublishingPageDesignFieldValue(value);
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl control = new PublishingPageDesignControl();
                control.FieldName = base.InternalName;
                return control;
            }
        }
    }
}

