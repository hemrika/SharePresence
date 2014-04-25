using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a one-line plain text edit control for the input element’s value.
    /// </summary>
    public class HiddenInput : InputControl
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.TextInput" />
        /// </summary>
        public HiddenInput() : base(InputType.Hidden) { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddStringAttributeIfNotEmpty(writer, "value", Value);
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Hidden Value")]
        public string Value
        { get { return GetViewState("Value", string.Empty); } set { SetViewState("Value", value); } }

    }
}
