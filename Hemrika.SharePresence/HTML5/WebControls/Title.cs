
using System.ComponentModel;
using System.Web.UI;
using System.Web;
namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// The header of a section
    /// </summary>
    public class Title : ContainerControl
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Header" />
        /// The <see cref="Hemrika.SharePresence.Html5.WebControls.Header" /> tag specifies an introduction, or a group of navigation elements for a document.
        /// The <see cref="Hemrika.SharePresence.Html5.WebControls.Header" /> element can contain headings, subheadings, version information, navigational controls, etc.
        /// </summary>
        public Title() : base(ContainerType.Span) { }

        // Properties
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
    }
}
