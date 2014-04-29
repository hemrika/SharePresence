using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using Microsoft.SharePoint;
using System.Web.UI;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5HeaderControl runat=server></{0}:HTML5HeaderControl>")]
    public class HTML5HeaderControl : BaseFieldControl
    {
        protected HiddenField html_header_hidden;
        protected Header html_header;
        private HTML5HeaderField headerField;

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Header";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5HeaderDisplay";
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

            headerField = (HTML5HeaderField)ItemFieldValue;

            if (headerField == null)
            {
                headerField = new HTML5HeaderField();
            }
            bool design = base.DesignMode;

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                /*
                html_title = (CheckBox)this.TemplateContainer.FindControl("html_title");
                html_title.Checked = headerField.UseTitle;

                html_heading = (TextInput)this.TemplateContainer.FindControl("html_heading");
                html_heading.Text = headerField.Heading;
                html_heading.Enabled = !html_title.Checked;

                html_subheading = (TextInput)this.TemplateContainer.FindControl("html_subheading");
                html_subheading.Text = headerField.SubHeading;

                html_author = (CheckBox)this.TemplateContainer.FindControl("html_author");
                html_author.Checked = headerField.IncludeAuthor;

                html_date = (CheckBox)this.TemplateContainer.FindControl("html_date");
                html_date.Checked = headerField.IncludeDate;
                */
                html_header_hidden = (HiddenField)this.TemplateContainer.FindControl("html_header_hidden");
                html_header_hidden.Value = headerField.Text;

                html_header = (Header)this.TemplateContainer.FindControl("html_header");
                html_header.Controls.AddAt(0,new LiteralControl { Text = headerField.Text });

            }
            else
            {
                html_header = (Header)this.TemplateContainer.FindControl("html_header");
                html_header.Controls.AddAt(0, new LiteralControl { Text = headerField.Text });
                /*
                if (string.IsNullOrEmpty(headerField.SubHeading))
                {
                    CreateHeader(html_header);
                }
                else
                {
                    HGroup hgroup = new HGroup();
                    CreateGroup(hgroup);
                    html_header.Controls.Add(hgroup);
                    CreateMeta(html_header);
                }
                */
            }
        }

        /*
        private void CreateGroup(WebControl parent)
        {

            if (!string.IsNullOrEmpty(headerField.Heading) && !headerField.UseTitle)
            {
                HtmlGenericControl heading = new HtmlGenericControl("h1");
                heading.InnerHtml = headerField.Heading;
                heading.Attributes.Add("class", "heading");
                parent.Controls.Add(heading);
            }
            else
            {
                HtmlGenericControl heading = new HtmlGenericControl("h1");
                heading.InnerHtml = ListItem[SPBuiltInFieldId.Title].ToString();
                heading.Attributes.Add("class", "heading");
                parent.Controls.Add(heading);
            }

            if (!string.IsNullOrEmpty(headerField.SubHeading))
            {
                HtmlGenericControl subHeading = new HtmlGenericControl("h2");
                subHeading.InnerHtml = headerField.SubHeading;
                subHeading.Attributes.Add("class", "subheading");
                parent.Controls.Add(subHeading);
            }
        }
        
        private void CreateMeta(WebControl parent)
        {
            if (headerField.IncludeAuthor)
            {
                HtmlGenericControl author = new HtmlGenericControl("p");
                author.InnerHtml = ListItem[SPBuiltInFieldId.Created_x0020_By].ToString();
                author.Attributes.Add("class", "author");
                parent.Controls.Add(author);
            }
            if (headerField.IncludeDate)
            {
                Time time = new Time();
                time.DateTime = ListItem[SPBuiltInFieldId.Created_x0020_Date].ToString();
                time.IsPubDate = true;
                time.CssClass = "time";
                parent.Controls.Add(time);
            }

            if (!string.IsNullOrEmpty(headerField.Text))
            {
                HtmlGenericControl text = new HtmlGenericControl("p");
                text.InnerHtml = headerField.Text;
                text.Attributes.Add("class", "text");
                parent.Controls.Add(text);
            }

        }

        private void CreateHeader(WebControl parent)
        {
            if (!string.IsNullOrEmpty(headerField.Heading) && !headerField.UseTitle)
            {
                HtmlGenericControl heading = new HtmlGenericControl("h1");
                heading.InnerHtml = headerField.Heading;
                heading.Attributes.Add("class", "heading");
                parent.Controls.Add(heading);
            }
            else
            {
                HtmlGenericControl heading = new HtmlGenericControl("h1");
                heading.InnerHtml = ListItem[SPBuiltInFieldId.Title].ToString();
                heading.Attributes.Add("class", "heading");
                parent.Controls.Add(heading);
            }

            if (!string.IsNullOrEmpty(headerField.SubHeading))
            {
                HtmlGenericControl subHeading = new HtmlGenericControl("h2");
                subHeading.InnerHtml = headerField.SubHeading;
                subHeading.Attributes.Add("class", "subheading");
                parent.Controls.Add(subHeading);
            }
            
            if (headerField.IncludeAuthor)
            {
                HtmlGenericControl author = new HtmlGenericControl("p");
                author.InnerHtml = ListItem[SPBuiltInFieldId.Created_x0020_By].ToString();
                author.Attributes.Add("class", "author");
                parent.Controls.Add(author);
            }
            
            if (headerField.IncludeDate)
            {
                Time time = new Time();
                time.DateTime = ListItem[SPBuiltInFieldId.Created_x0020_Date].ToString();
                time.IsPubDate = true;
                time.CssClass = "time";
                parent.Controls.Add(time);
            }

            if (!string.IsNullOrEmpty(headerField.Text))
            {
                HtmlGenericControl text = new HtmlGenericControl("p");
                text.InnerHtml = headerField.Text;
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
                    headerField.UseTitle = html_title.Checked;
                    headerField.Heading = html_heading.Text;
                    headerField.SubHeading = html_subheading.Text;
                    headerField.IncludeAuthor = html_author.Checked;
                    headerField.IncludeDate = html_author.Checked;
                    */
                    headerField.Text = html_header_hidden.Value;
                    ((html_header.Controls[0]) as LiteralControl).Text = headerField.Text;
                    return headerField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                headerField = (HTML5HeaderField)value;
                /*
                html_title.Checked = headerField.UseTitle;
                html_heading.Text = headerField.Heading;
                html_subheading.Text = headerField.SubHeading;
                html_author.Checked = headerField.IncludeAuthor;
                html_date.Checked = headerField.IncludeDate;
                */
                html_header_hidden.Value = headerField.Text;
                ((html_header.Controls[0]) as LiteralControl).Text = headerField.Text;
            }
        }

        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }
    }
}
