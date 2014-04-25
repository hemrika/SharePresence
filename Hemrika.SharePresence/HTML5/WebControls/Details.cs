using System.ComponentModel;
using System.Web.UI;
using System.Web;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a control from which the user can obtain additional information or controls on-demand.
    /// </summary>
    public class Details : ContainerControl
    {
        /// <summary>
        /// Creates new instance of type <see cref="Hemrika.SharePresence.Html5.WebControls.Details" />
        /// </summary>
        public Details() : base(ContainerType.Details) { }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddBooleanAttribute(writer, "open", IsOpen);
        }

        /// <summary>
        /// Gets or sets a value which specifies that the contents of the details element should be shown to the user.
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Layout"), Description("Specifies that the contents of the details element should be shown to the user")]
        public bool IsOpen 
        { get { return GetViewState("IsOpen", false); } set { SetViewState("IsOpen", value); } }

        /// <summary>
        /// Gets or sets the Style
        /// </summary>
        [DefaultValue(""), Description("The Style"), Themeable(false), Category("Behavior")]
        public new string Style { get { return GetViewState("Style", string.Empty); } set { SetViewState("Style", value); } }

    }
}
