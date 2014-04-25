// -----------------------------------------------------------------------
// <copyright file="Image.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Web.UI;
using System.Web;

namespace Hemrika.SharePresence.Html5.WebControls
{
    public class Label : ContainerControl
    {
        public Label() : base(ContainerType.Label) { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddStringAttributeIfNotEmpty(writer, "class", Class);
            Helper.AddStringAttributeIfNotEmpty(writer, "style", Style);
            Helper.AddStringAttributeIfNotEmpty(writer, "title", Title);
            Helper.AddStringAttributeIfNotEmpty(writer, "for", For);

        }

        /// <summary>
        /// Gets or sets the Class
        /// </summary>
        [DefaultValue(""), Description("The Class for the label"), Themeable(false), Category("Behavior")]
        public string Class { get { return GetViewState("Class", string.Empty); } set { SetViewState("Class", value); } }

        /// <summary>
        /// Gets or sets the Style
        /// </summary>
        [DefaultValue(""), Description("The Style for the label"), Themeable(false), Category("Behavior")]
        public new string Style { get { return GetViewState("Style", string.Empty); } set { SetViewState("Style", value); } }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        [DefaultValue(""), Description("The Title for the label"), Themeable(false), Category("Behavior")]
        public string Title { get { return GetViewState("Title", string.Empty); } set { SetViewState("Title", value); } }

        /// <summary>
        /// Gets or sets the For
        /// </summary>
        [DefaultValue(""), Description("The For for the label"), Themeable(false), Category("Behavior")]
        public string For { get { return GetViewState("For", string.Empty); } set { SetViewState("For", value); } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),DefaultValue(""), Description("The inner HTML for the label"), Themeable(false), Category("Behavior")]
        public virtual string InnerHtml
        {
            get
            {
                if (base.IsLiteralContent())
                {
                    return ((LiteralControl)this.Controls[0]).Text;
                }
                if ((this.HasControls() && (this.Controls.Count == 1)) && (this.Controls[0] is DataBoundLiteralControl))
                {
                    return ((DataBoundLiteralControl)this.Controls[0]).Text;
                }
                /*
                if (this.Controls.Count != 0)
                {
                    throw new HttpException(SR.GetString("Inner_Content_not_literal", new object[] { this.ID }));
                }
                */
                return string.Empty;
            }
            set
            {
                this.Controls.Clear();
                this.Controls.Add(new LiteralControl(value));
                this.ViewState["innerhtml"] = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(""), Description("The Text for the label"), Themeable(false), Category("Behavior")]
        public virtual string InnerText
        {
            get
            {
                return HttpUtility.HtmlDecode(this.InnerHtml);
            }
            set
            {
                this.InnerHtml = HttpUtility.HtmlEncode(value);
            }
        }

    }
}
