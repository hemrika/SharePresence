    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{

    /// <summary>
    /// Control for field type HTML5PublishingDate
    /// </summary>
    [ToolboxData("<{0}:HTML5PublishingDateControl runat=server></{0}:HTML5PublishingDateControl>")]
    public class HTML5PublishingDateControl : BaseFieldControl
    {
        protected Time html_date;
        private HTML5PublishingDateField dateField;

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5PublishingDate";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5PublishingDateDisplay";
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

            dateField = (HTML5PublishingDateField)ItemFieldValue;

            if (dateField == null)
            {
                dateField = new HTML5PublishingDateField();
            }
            DateTime pubTime = DateTime.Now;
            //DateTime.TryParse(ListItem[SPBuiltInFieldId.PublishedDate].ToString(), out pubTime);
            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                html_date = (Time)this.TemplateContainer.FindControl("html_date");
                string pattern = html_date.Pattern;
                html_date.IsPubDate = true;

                html_date.DateTime = String.Format("{0:dddd MMMM d, yyyy}", pubTime);
            }
            else
            {
                html_date = (Time)this.TemplateContainer.FindControl("html_date");
                string pattern = html_date.Pattern;
                html_date.IsPubDate = true;
                html_date.DateTime = String.Format("{0:dddd, MMMM d, yyyy}", pubTime);
            }
        }

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    this.EnsureChildControls();
                    return dateField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                dateField = (HTML5PublishingDateField)value;
            }
        }
    }
}