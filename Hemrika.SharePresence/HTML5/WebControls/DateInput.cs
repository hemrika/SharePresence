using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a control for setting the element’s value to a string representing a date.
    /// </summary>
    public class DateInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.DateInput" />
        /// </summary>
        public DateInput() : base(InputType.Date) { }

        /// <summary>
        /// Gets or sets the selected DateTime
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Selected DateTime")]
        public string Value //TODO change to DateTime
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the expected lower bound for the element’s value.
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("The expected lower bound for the element’s value.")]
        public string Minimum
        { get { return GetViewState("Minimum", string.Empty); } set { SetViewState("Minimum", value); } }

        /// <summary>
        /// Gets or sets the expected upper bound for the element’s value.
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("The expected upper bound for the element’s value.")]
        public string Maximum
        { get { return GetViewState("Maximum", string.Empty); } set { SetViewState("Maximum", value); } }

        /// <summary>
        /// Gets or sets specifies the value granularity of the element’s value.
        /// </summary>
        [Themeable(false), DefaultValue(1f), Category("Behavior"), Description("Specifies the value granularity of the element’s value")]
        public float Step
        { get { return GetViewState("Step", 1f); } set { SetViewState("Step", value); } }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddStringAttributeIfNotEmpty(writer, "min", Minimum);
            Helper.AddStringAttributeIfNotEmpty(writer, "max", Maximum);
            Helper.AddFloatAttributeIfNotDefault(writer, "step", Step, 1);
        }
    }
}
