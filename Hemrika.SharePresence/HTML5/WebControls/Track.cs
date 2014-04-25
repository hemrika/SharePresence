using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// enables supplementary media tracks such as subtitle tracks and caption tracks
    /// to be specified for audio and video elements.
    /// </summary>
    /// <remarks>This control can only be nested inside an <see cref="Hemrika.SharePresence.Html5.WebControls.Audio" /> or <see cref="Hemrika.SharePresence.Html5.WebControls.Video" /> Control</remarks>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Audio"/>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Video"/>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Source"/>
    public class Track : Html5Control
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Track" />
        /// </summary>
        public Track() : base("track") { }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter object that receives the control content.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when nested inside a type other than <see cref="Hemrika.SharePresence.Html5.WebControls.Video" /> or <see cref="Hemrika.SharePresence.Html5.WebControls.Audio" /></exception>
        protected override void Render(HtmlTextWriter writer)
        {
            var parent = this.Parent;
            if (parent == null || (!(parent is Video) && !(parent is Audio)))
                throw new InvalidOperationException("A track element can only nest inside a video or audio element");
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
            writer.AddAttribute("kind", Kind.ToString().ToLower());
            Helper.AddUrlAttributeIfNotEmpty(writer, "src", Url, this);
            Helper.AddStringAttributeIfNotEmpty(writer, "srclang", Language);
            Helper.AddStringAttributeIfNotEmpty(writer, "label", Label);
            Helper.AddBooleanAttribute(writer, "default", IsDefault);
        }

        /// <summary>
        /// Gets or sets the kind of timed track
        /// </summary>
        [DefaultValue(TrackKind.Subtitles), Description("The kind of timed track"), Themeable(false), Category("Behavior")]
        public TrackKind Kind
        { get { return GetViewState("Kind", TrackKind.Subtitles); } set { SetViewState("Kind", value); } }

        /// <summary>
        /// Gets or sets the address of the timed track
        /// </summary>
        [DefaultValue(""), Description("The address of the timed track"), UrlProperty, Themeable(false), Category("Behavior")]
        public string Url
        { get { return GetViewState("Url", string.Empty); } set { SetViewState("Url", value); } }

        /// <summary>
        /// Gets or sets the language of the timed track
        /// </summary>
        [DefaultValue(""), Description("The language of the timed track"), Themeable(false), Category("Behavior")]
        public string Language
        { get { return GetViewState("Language", string.Empty); } set { SetViewState("Language", value); } }


        /// <summary>
        /// Gets or sets the user-readable title for the timed track
        /// </summary>
        [DefaultValue(""), Description("A user-readable title for the timed track"), Themeable(false), Category("Behavior")]
        public string Label
        { get { return GetViewState("Label", string.Empty); } set { SetViewState("Label", value); } }

        /// <summary>
        /// Gets or sets a value that instructs the user agent that the track is to be enabled if the user’s preferences do not indicate that another track would be more appropriate
        /// </summary>
        [DefaultValue(false), Description("Is Default among tracks"), Themeable(false), Category("Behavior")]
        public bool IsDefault
        { get { return GetViewState("IsDefault", false); } set { SetViewState("IsDefault", value); } }
    }
}
