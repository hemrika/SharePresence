using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5DetailsControl runat=server></{0}:HTML5DetailsControl>")]
    public class HTML5DetailsControl : BaseFieldControl
    {
        protected HiddenField html_details_hidden;
        protected Details html_details;
        private HTML5DetailsField detailsField;

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
                return "HTML5Details";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5DetailsDisplay";
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
            detailsField = (HTML5DetailsField)ItemFieldValue;

            if (detailsField == null)
            {
                detailsField = new HTML5DetailsField();
            }
            
            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                //html_summery = (InputFormTextBox)this.TemplateContainer.FindControl("html_summery");
                //html_summery.Text = detailsField.Summery;

                //html_text = (InputFormTextBox)this.TemplateContainer.FindControl("html_text");
                //html_text.Text = detailsField.Text;

                html_details_hidden = (HiddenField)this.TemplateContainer.FindControl("html_header_hidden");
                html_details_hidden.Value = detailsField.Text;

                html_details = (Details)this.TemplateContainer.FindControl("html_details");
                html_details.Controls.AddAt(0,new LiteralControl { Text = detailsField.Text });

            }
            else
            {
                html_details = (Details)this.TemplateContainer.FindControl("html_details");
                html_details.Controls.AddAt(0, new LiteralControl { Text = detailsField.Text });

                //html_details = (Details)this.TemplateContainer.FindControl("html_details");
            }
        }

        /*
        private void CreateDetails(WebControl parent)
        {
            if (!string.IsNullOrEmpty(detailsField.Summery))
            {
                Summary summery = new Summary();
                HtmlGenericControl text = new HtmlGenericControl("p");
                text.InnerText = detailsField.Text;
                text.Attributes.Add("class", "text");
                summery.Controls.Add(text);
                parent.Controls.Add(summery);
            }

            if (!string.IsNullOrEmpty(detailsField.Text))
            {
                HtmlGenericControl text = new HtmlGenericControl("p");
                text.InnerText = detailsField.Text;
                text.Attributes.Add("class", "text");
                parent.Controls.Add(text);
            }
        }
        */

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    //this.EnsureChildControls();
                    /*
                    detailsField.Summery = html_summery.Text;
                    detailsField.Text = html_text.Text;
                    */

                    detailsField.Text = html_details_hidden.Value;
                    ((html_details.Controls[0]) as LiteralControl).Text = detailsField.Text;
                    return detailsField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                detailsField = (HTML5DetailsField)value;
                /*
                html_summery.Text = detailsField.Summery;
                html_text.Text = detailsField.Text;
                */
                html_details_hidden.Value = detailsField.Text;
                ((html_details.Controls[0]) as LiteralControl).Text = detailsField.Text;
            }
        }

        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }
    }
}
