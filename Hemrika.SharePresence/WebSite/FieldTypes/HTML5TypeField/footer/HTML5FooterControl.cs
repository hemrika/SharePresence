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
    [ToolboxData("<{0}:HTML5FooterControl runat=server></{0}:HTML5FooterControl>")]
    public class HTML5FooterControl : BaseFieldControl
    {
        protected HiddenField html_footer_hidden;
        protected Footer html_footer;
        private HTML5FooterField footerField;

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
                return "HTML5Footer";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5FooterDisplay";
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
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();
            footerField = (HTML5FooterField)ItemFieldValue;

            if (footerField == null)
            {
                footerField = new HTML5FooterField();
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                html_footer_hidden = (HiddenField)this.TemplateContainer.FindControl("html_footer_hidden");
                html_footer_hidden.Value = footerField.Text;

                html_footer = (Footer)this.TemplateContainer.FindControl("html_footer");
                html_footer.Controls.AddAt(0, new LiteralControl { Text = footerField.Text });

            }
            else
            {
                html_footer = (Footer)this.TemplateContainer.FindControl("html_footer");
                html_footer.Controls.AddAt(0, new LiteralControl { Text = footerField.Text });
            }
        }

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    //this.EnsureChildControls();
                    footerField.Text = html_footer_hidden.Value;
                    ((html_footer.Controls[0]) as LiteralControl).Text = footerField.Text;
                    return footerField;

                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                footerField = (HTML5FooterField)value;
                html_footer_hidden.Value = footerField.Text;
                ((html_footer.Controls[0]) as LiteralControl).Text = footerField.Text;

            }
        }

        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }
    }
}
