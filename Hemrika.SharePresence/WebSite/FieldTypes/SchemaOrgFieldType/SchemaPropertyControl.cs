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
    [Guid("BC9977EA-5B75-437F-B8E3-640434EEDAE3")]
    public class SchemaPropertyControl : BaseFieldControl
    {
        protected TextBox tbx_type;
        protected TextBox tbx_property;
        protected System.Web.UI.HtmlControls.HtmlGenericControl scope;

        private SchemaPropertyField propertyField;

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
                return "SchemaOrg_ItemProperty";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "SchemaOrg_ItemPropertyDisplay";
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
            propertyField = (SchemaPropertyField)ItemFieldValue;

            if (propertyField == null)
            {
                propertyField = new SchemaPropertyField();
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                tbx_type = (TextBox)this.TemplateContainer.FindControl("tbx_type");
                tbx_property = (TextBox)this.TemplateContainer.FindControl("tbx_property");

                if (tbx_type != null)
                {
                    tbx_type.Text = propertyField.Type;
                }

                if (tbx_property != null)
                {
                    tbx_property.Text = propertyField.Property;
                }
            }
            else
            {
                scope = (HtmlGenericControl)this.TemplateContainer.FindControl("scope");
                scope.Attributes["itemtype"] = propertyField.Type;
                scope.Attributes["itemprop"] = propertyField.Property;
            }
        }

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    this.EnsureChildControls();
                    propertyField.Property = tbx_property.Text;
                    propertyField.Type = tbx_type.Text;
                    return propertyField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                propertyField = (SchemaPropertyField)value;
                tbx_property.Text = propertyField.Property;
                tbx_type.Text = propertyField.Type;
            }
        }
    }
}

