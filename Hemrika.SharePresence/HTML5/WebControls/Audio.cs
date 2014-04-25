using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    ///  Represents an audio stream.
    /// </summary>
    public class Audio : ContainerControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Audio" />
        /// </summary>
        public Audio() : base(ContainerType.Audio) { }


        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddBooleanAttribute(writer, "autoplay", AutoPlay);
            Helper.AddLowerCaseEnumIfNotZero(writer, "preload", PreLoad);
            Helper.AddBooleanAttribute(writer, "controls", DisplayControls);
            Helper.AddBooleanAttribute(writer, "loop", Loop);
            Helper.AddStringAttributeIfNotEmpty(writer, "mediagroup", MediaGroup);
            Helper.AddUrlAttributeIfNotEmpty(writer, "src", Url, this);
        }

        /// <summary>
        /// Gets or sets a value that instructs the user agent to automatically begin playback of the audio stream as soon as it can do so without stopping
        /// </summary>
        [DefaultValue(false), Description("Instructs the user agent to automatically begin playback of the audio stream as soon as it can do so without stopping"), Themeable(false), Category("Behavior")]
        public bool AutoPlay { get { return GetViewState<bool>("AutoPlay"); } set { SetViewState("AutoPlay", value); } }
        
        /// <summary>
        /// Gets or sets a value that represents a hint to the user agent about whether optimistic downloading of the audio stream itself or its metadata is considered worthwhile
        /// </summary>
        [DefaultValue(PreLoadMode.NotSet), Description("Represents a hint to the user agent about whether optimistic downloading of the audio stream itself or its metadata is considered worthwhile"), Themeable(false), Category("Behavior")]
        public PreLoadMode PreLoad { get { return GetViewState<PreLoadMode>("PreLoad", PreLoadMode.NotSet); } set { SetViewState("PreLoad", value); } }

        /// <summary>
        /// Gets or sets a value that instructs the user agent to expose a user interface for controlling playback of the audio stream
        /// </summary>
        [DefaultValue(false), Description("Instructs the user agent to expose a user interface for controlling playback of the audio stream"), Themeable(false), Category("Behavior")]
        public bool DisplayControls { get { return GetViewState<bool>("DisplayControls"); } set { SetViewState("DisplayControls", value); } }

        /// <summary>
        /// Gets or sets a value that instructs the user agent to seek back to the start of the audio stream upon reaching the end
        /// </summary>
        [DefaultValue(false), Description("Instructs the user agent to seek back to the start of the audio stream upon reaching the end"), Themeable(false), Category("Behavior")]
        public bool Loop { get { return GetViewState<bool>("Loop"); } set { SetViewState("Loop", value); } }
        
        /// <summary>
        /// Gets or sets a value that instructs the user agent to link multiple videos and/or audio streams together
        /// </summary>
        [DefaultValue(""), Description("Instructs the user agent to link multiple videos and/or audio streams together"), Themeable(false), Category("Behavior")]
        public string MediaGroup{ get { return GetViewState("MediaGroup", string.Empty); } set { SetViewState("MediaGroup", value); } }

        /// <summary>
        /// Gets or sets the URL for the audio stream
        /// </summary>
        [DefaultValue(""), Description("The URL for the audio stream"), Themeable(false), Category("Behavior"), UrlProperty]
        public string Url { get { return GetViewState("Url", string.Empty); } set { SetViewState("Url", value); } }
    }
}
