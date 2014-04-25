
using System.ComponentModel;
using System.Web.UI;
namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a section of content that forms an independent part of a document or site; 
    /// for example, a magazine or newspaper article, or a blog entry.
    /// </summary>
    public class Article : ContainerControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Article" />
        /// </summary>
        public Article() : base(ContainerType.Article) { }

        /// <summary>
        /// Gets or sets the Style
        /// </summary>
        [DefaultValue(""), Description("The Style"), Themeable(false), Category("Behavior")]
        public new string Style { get { return GetViewState("Style", string.Empty); } set { SetViewState("Style", value); } }

    }
}
