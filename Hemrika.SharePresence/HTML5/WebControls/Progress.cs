using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents the completion progress of a task
    /// </summary>
    [Description("Represents the completion progress of a task")]
    public class Progress : Html5Control
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Progress" />
        /// </summary>
        public Progress() : base("progress") { }


        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddFloatAttributeIfNotDefault(writer, "max", Maximum, 1f);
            Helper.AddFloatAttributeIfNotDefault(writer, "value", Value, 0f);
        }

        /// <summary>
        /// Gets or sets the value taht specifies how much work the task requires in total
        /// </summary>
        [Themeable(false), DefaultValue(1f), Category("Behavior"), Description("Specifies how much work the task requires in total")]
        public float Maximum
        { get { return GetViewState("Maximum", 1f); } set { SetViewState("Maximum", value); } }

        /// <summary>
        /// Gets or sets the value that specifies how much of the task has been completed. 
        /// </summary>
        [Themeable(false), DefaultValue(0f), Category("Behavior"), Description("Specifies how much of the task has been completed.")]
        public float Value
        { get { return GetViewState("Value", 0f); } set { SetViewState("Value", value); } }
    }
}
