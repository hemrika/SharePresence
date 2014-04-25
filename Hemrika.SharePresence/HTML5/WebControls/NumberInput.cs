using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a precise control for setting the element’s value to a string representing a number.
    /// </summary>
    public class NumberInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.NumberInput" />
        /// </summary>
        public NumberInput() : base(InputType.Number) { }
        internal NumberInput(InputType type) : base(type) { }

        /// <summary>
        /// Gets or sets the expected lower bound for the element’s value
        /// </summary>
        [Themeable(false), DefaultValue(float.MinValue), Category("Behavior"), Description("The expected lower bound for the element’s value")]
        public float Minimum
        { get { return GetViewState("Minimum", float.MinValue); } set { SetViewState("Minimum", value); } }

        /// <summary>
        /// Gets or sets the expected upper bound for the element’s value
        /// </summary>
        [Themeable(false), DefaultValue(float.MinValue), Category("Behavior"), Description("The expected upper bound for the element’s value")]
        public float Maximum
        { get { return GetViewState("Maximum", float.MaxValue); } set { SetViewState("Maximum", value); } }

        /// <summary>
        /// Gets or sets the increment or decrement step
        /// </summary>
        [Themeable(false), DefaultValue(1f), Category("Behavior"), Description("The increment or decrement step")]
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
            Helper.AddFloatAttributeIfNotDefault(writer, "min", Minimum, float.MinValue);
            Helper.AddFloatAttributeIfNotDefault(writer, "max", Maximum, float.MaxValue);
            Helper.AddFloatAttributeIfNotDefault(writer, "step", Step, 1f);
        }


        /// <summary>
        /// Gets or sets the selected value
        /// </summary>
        [Themeable(false), DefaultValue(0), Category("Behavior"), Description("Selected value")]
        public float Value
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return 0;
                return float.Parse(Text);
            }
            set
            {
                Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets a value showing if the 'Value' property has been set
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Determines if Value has been set")]
        public bool HasValue
        {
            get
            {
                return !string.IsNullOrEmpty(Text);
            }
        }
    }
}
