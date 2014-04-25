using System.ComponentModel;
using System.Web.UI;
using System.Globalization;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    ///  Represents a date and/or time.
    /// </summary>
    [ToolboxData("<{0}:Time runat=\"server\" />")]
    public class Time : ContainerControl
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Time" />
        /// </summary>
        public Time() : base(ContainerType.Time) { }


        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (!string.IsNullOrEmpty(DateTime))
                writer.AddAttribute("datetime", DateTime);

            if (IsPubDate)
                writer.AddAttribute("pubdate", "pubdate");
        }

        /// <summary>
        /// Gets or sets the value that Specifies the date or time that the element represents
        /// </summary>
        /// <remarks>The value can be a time, a date or a date and time</remarks>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Specifies the date or time that the element represents")]
        public string DateTime//TODO:DateTime
        { get { return GetViewState("DateTime", string.Empty); } set { SetViewState("DateTime", value); } }

        /// <summary>
        /// Gets or sets the value which indicates that the date and time given by the element is the publication date and time of the nearest ancestor article element — or, if the element has no ancestor article element, of the document as a whole.
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Indicates that the date and time given by the element is the publication date and time of the nearest ancestor article element — or, if the element has no ancestor article element, of the document as a whole")]
        public bool IsPubDate
        { get { return GetViewState("IsPubDate", false); } set { SetViewState("IsPubDate", value); } }

        /// <summary>
        /// Gets or sets the value which indicates that the date and time given by the element is the publication date and time of the nearest ancestor article element — or, if the element has no ancestor article element, of the document as a whole.
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Specifies the date or time display pattern")]
        public string Pattern
        { get { return GetViewState("Pattern", string.Empty); } set { SetViewState("Pattern", value); } }

    }
}
