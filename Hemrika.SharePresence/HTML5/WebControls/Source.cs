using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Enables multiple media sources to be specified for audio and video elements.
    /// </summary>
    /// <remarks>This control can only be nested inside an <see cref="Hemrika.SharePresence.Html5.WebControls.Audio" /> or <see cref="Hemrika.SharePresence.Html5.WebControls.Video" /> Control</remarks>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Audio"/>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Video"/>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Track"/>
    public class Source : Html5Control
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Source" />
        /// </summary>
        public Source() : base("source") { }
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter object that receives the control content.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when nested inside a type other than <see cref="Hemrika.SharePresence.Html5.WebControls.Video" /> or <see cref="Hemrika.SharePresence.Html5.WebControls.Audio" /></exception>
        protected override void Render(HtmlTextWriter writer)
        {
            var parent = this.Parent;
            if (parent == null || (!(parent is Video) && !(parent is Audio)))
                throw new InvalidOperationException("A source element can only nest inside a video or audio element");
            base.Render(writer);
        }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddUrlAttributeIfNotEmpty(writer, "src", Url, this);
            Helper.AddStringAttributeIfNotEmpty(writer, "type", Type);
            Helper.AddStringAttributeIfNotEmpty(writer, "media", Media);
        }

        /// <summary>
        /// Gets or sets the address of the media source
        /// </summary>
        [DefaultValue(""), Description("The address of the media source"), UrlProperty, Themeable(false), Category("Behavior")]
        public string Url
        { get { return GetViewState("Url", string.Empty); } set { SetViewState("Url", value); } }

        /// <summary>
        /// Gets or sets the type of the media source (used for helping the user agent determine, before fetching this media source, if it can play it).
        /// A string that identifies a valid MIME media type, as defined in [RFC2046].
        /// </summary>
        [DefaultValue(""), Description("The type of the media source"), Themeable(false)]
        public string Type
        { get { return GetViewState("TypeAttr", string.Empty); } set { SetViewState("TypeAttr", value); } }

        /// <summary>
        /// Gets or sets the intended media type of the media source (used for helping the user agent determine, before fetching this media source, if it is useful to the user).
        /// A valid media query list, as defined in [MediaQueries]
        /// </summary>
        [DefaultValue(""), Description("The intended media type of the media source"), Themeable(false), Category("Behavior")]
        public string Media
        { get { return GetViewState("Media", string.Empty); } set { SetViewState("Media", value); } }


    }
}
