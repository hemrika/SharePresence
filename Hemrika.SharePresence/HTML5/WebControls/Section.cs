using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a section of a document, typically with a title or heading.
    /// The <section> tag defines sections in a document. Such as chapters, headers, footers, or any other sections of the document.
    /// </summary>
    public class Section : ContainerControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Section" />
        /// </summary>
        public Section() : base(ContainerType.Section) { }

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
        /// Gets or sets the Style
        /// </summary>
        [DefaultValue(""), Description("The Style"), Themeable(false), Category("Behavior")]
        public new string Style { get { return GetViewState("Style", string.Empty); } set { SetViewState("Style", value); } }

    }
}
