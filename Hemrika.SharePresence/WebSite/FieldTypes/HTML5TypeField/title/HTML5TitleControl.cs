    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{

    /// <summary>
    /// Control for field type HTML5Title
    /// </summary>  
    [ToolboxData("<{0}:HTML5TitleControl runat=server></{0}:HTML5TitleControl>")]
    public class HTML5TitleControl : BaseFieldControl
    {
        protected HiddenField html_title_hidden;
        protected Title html_title;
        private HTML5TitleField titleField;
        private SPField _title;

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Title";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5TitleDisplay";
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

        internal virtual bool WpcmEnabled
        {
            get
            {
                return true;
            }
        }

        protected override void CreateChildControls()
        {
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();
            try
            {
                titleField = (HTML5TitleField)ItemFieldValue;
            }
            catch (Exception)
            {
            }

            _title = ListItem.Fields[SPBuiltInFieldId.Title];

            if (titleField == null)
            {
                titleField = new HTML5TitleField();
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                //this.CssClass = "HTML5-editable";
                html_title_hidden = (HiddenField)this.TemplateContainer.FindControl("html_title_hidden");
                html_title = (Title)this.TemplateContainer.FindControl("html_title");

                if (string.IsNullOrEmpty(titleField.Title))
                {
                    html_title_hidden.Value = _title.GetFieldValueAsHtml(ListItem[SPBuiltInFieldId.Title]);
                    html_title.Controls.AddAt(0, new LiteralControl { Text = _title.GetFieldValueAsHtml(ListItem[SPBuiltInFieldId.Title]) });
                    //html_title.Text = _title.GetFieldValueAsHtml(ListItem[SPBuiltInFieldId.Title]);
                }
                else
                {
                    html_title_hidden.Value = titleField.Title;
                    html_title.Controls.AddAt(0, new LiteralControl { Text = titleField.Title });
                    //html_title.Text = titleField.Title;
                }
            }
            else
            {
                html_title = (Title)this.TemplateContainer.FindControl("html_title");
                html_title.Controls.AddAt(0, new LiteralControl { Text = _title.GetFieldValueAsHtml(ListItem[SPBuiltInFieldId.Title]) });
                //html_title.InnerHtml = _title.GetFieldValueAsHtml(ListItem[SPBuiltInFieldId.Title]);
            }
        }

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    this.EnsureChildControls();
                    String clean = Regex.Replace(html_title_hidden.Value, @"<[^>]*>", String.Empty);
                    clean = Regex.Replace(clean, @"[\r\n\t]*", string.Empty);
                    titleField.Title = clean;// html_title_hidden.Value;
                    _title.ParseAndSetValue(ListItem, titleField.Title);
                    ((html_title.Controls[0]) as LiteralControl).Text = titleField.Title;

                    //html_title.Text = titleField.Title;
                    return titleField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                try
                {
                    titleField = (HTML5TitleField)value;
                }
                catch (Exception)
                {
                    titleField = new HTML5TitleField();
                }

                //html_title.Text = titleField.Title;
                String clean = Regex.Replace(titleField.Title, @"<[^>]*>", String.Empty);
                clean = Regex.Replace(clean, @"[\r\n\t]*", string.Empty);
                html_title_hidden.Value = clean;// titleField.Title;
                ((html_title.Controls[0]) as LiteralControl).Text = titleField.Title;

            }
        }
    }
}