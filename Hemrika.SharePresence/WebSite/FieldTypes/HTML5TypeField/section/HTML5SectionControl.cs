using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5SectionControl runat=server></{0}:HTML5SectionControl>")]
    public class HTML5SectionControl : BaseFieldControl
    {
        protected HiddenField html_section_hidden;
        protected Section html_section;
        private HTML5SectionField sectionField;

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Section";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5SectionDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();
            sectionField = (HTML5SectionField)ItemFieldValue;

            if (sectionField == null)
            {
                sectionField = new HTML5SectionField();
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                html_section_hidden = (HiddenField)this.TemplateContainer.FindControl("html_section_hidden");
                html_section_hidden.Value = sectionField.Text;

                html_section = (Section)this.TemplateContainer.FindControl("html_section");
                html_section.Controls.AddAt(0, new LiteralControl { Text = sectionField.Text });

            }
            else
            {
                html_section = (Section)this.TemplateContainer.FindControl("html_section");
                html_section.Controls.AddAt(0, new LiteralControl { Text = sectionField.Text });
            }
        }

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    //this.EnsureChildControls();
                    sectionField.Text = html_section_hidden.Value;
                    ((html_section.Controls[0]) as LiteralControl).Text = sectionField.Text;

                    return sectionField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                sectionField = (HTML5SectionField)value;
                html_section_hidden.Value = sectionField.Text;
                ((html_section.Controls[0]) as LiteralControl).Text = sectionField.Text;

            }
        }

        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }
    }
}
