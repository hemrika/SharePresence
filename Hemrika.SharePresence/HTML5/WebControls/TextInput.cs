using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a one-line plain text edit control for the input element’s value.
    /// </summary>
    public class TextInput : InputControl
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.TextInput" />
        /// </summary>
        public TextInput() : base(InputType.Text) { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddStringAttributeIfNotEmpty(writer, "dirname", DirName);
            Helper.AddStringAttributeIfNotEmpty(writer, "class", Class);
            Helper.AddStringAttributeIfNotEmpty(writer, "style", Style);
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
        /// Gets or sets the For
        /// </summary>
        [DefaultValue(""), Description("The For for the label"), Themeable(false), Category("Behavior")]
        public string For { get { return GetViewState("For", string.Empty); } set { SetViewState("For", value); } }


        /// <summary>
        /// Gets or sets the selected text
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Selected text")]
        public string Value { get { return Text; } set { Text = value; } }

        /// <summary>
        /// Gets or sets a value that enables submission of a rtl/ltr for of the element (If switched by user), and gives the name of the field that contains that value.
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Enables submission of a value for the directionality of the element, and gives the name of the field that contains that value.")]
        public string DirName
        { get { return GetViewState("DirName", string.Empty); } set { SetViewState("DirName", value); } }

    }
}
