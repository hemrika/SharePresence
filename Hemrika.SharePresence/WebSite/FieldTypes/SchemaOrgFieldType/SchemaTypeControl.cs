// <copyright file="SchemaOrgFieldTypeControl.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-04-26 15:23:31Z</date>
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
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Control for field type SchemaOrgFieldType
    /// </summary>  
    [Guid("62e8c523-313c-4e7d-95b7-a55e8e4e6dab")]
    public class SchemaTypeControl : BaseFieldControl
    {
        protected TextBox tbx_type;
        private SchemaTypeField typeField = null;
        protected System.Web.UI.HtmlControls.HtmlGenericControl scope;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "SchemaOrg_ItemType";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "SchemaOrg_ItemTypeDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();

            typeField = (SchemaTypeField)ItemFieldValue;

            if (typeField == null)
            {
                typeField = new SchemaTypeField { Type = "http://Schema.Org/WebPage" };
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                tbx_type = (TextBox)this.TemplateContainer.FindControl("tbx_type");

                if (tbx_type != null)
                {
                    tbx_type.Text = typeField.Type;
                }
            }
            else
            {
                scope = (HtmlGenericControl)this.TemplateContainer.FindControl("scope");
                scope.Attributes["itemtype"] = typeField.Type;
            }
        }

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    this.EnsureChildControls();
                    typeField.Type = tbx_type.Text;
                    return typeField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                typeField = (SchemaTypeField)value;
                tbx_type.Text = typeField.Type;
            }
        }
    }
}

