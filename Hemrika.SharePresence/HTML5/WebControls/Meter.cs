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
    /// Represents a scalar gauge providing a measurement within a known range, or a fractional value
    /// </summary>
    [Description("represents a scalar gauge providing a measurement within a known range, or a fractional value")]
    public class Meter : Html5Control
    {

        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Meter" />
        /// </summary>
        public Meter() : base("meter") { }

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            Helper.AddFloatAttributeIfNotDefault(writer, "min", Minimum, 0);
            Helper.AddFloatAttributeIfNotDefault(writer, "max", Maximum, 1);
            Helper.AddFloatAttributeIfNotDefault(writer, "value", Value, 0);
            Helper.AddFloatAttributeIfNotDefault(writer, "low", Low, 0);
            Helper.AddFloatAttributeIfNotDefault(writer, "high", High, 1);
            Helper.AddFloatAttributeIfNotDefault(writer, "optimum", Optimum, 0);
        }

        /// <summary>
        /// Gets or sets lower bound of the range for the meter
        /// </summary>
        [Themeable(false), DefaultValue(0f), Category("Behavior"), Description("The lower bound of the range for the meter")]
        public float Minimum
        { get { return GetViewState("Minimum", 0f); } set { SetViewState("Minimum", value); } }

        /// <summary>
        /// Gets or sets the upper bound of the range for the meter
        /// </summary>
        [Themeable(false), DefaultValue(1f), Category("Behavior"), Description("The upper bound of the range for the meter")]
        public float Maximum
        { get { return GetViewState("Maximum", 1f); } set { SetViewState("Maximum", value); } }

        /// <summary>
        /// Gets or sets the “measured” value shown by meter
        /// </summary>
        [Themeable(false), DefaultValue(0f), Category("Behavior"), Description("The “measured” value shown by meter")]
        public float Value
        { get { return GetViewState("Value", 0f); } set { SetViewState("Value", value); } }


        /// <summary>
        /// Gets or sets the point that marks the upper boundary of the “low” segment of the meter
        /// </summary>
        [Themeable(false), DefaultValue(0f), Category("Behavior"), Description("The point that marks the upper boundary of the “low” segment of the meter")]
        public float Low
        { get { return GetViewState("Low", 0f); } set { SetViewState("Low", value); } }


        /// <summary>
        /// Gets or sets the point that marks the lower boundary of the “high” segment of the meter.
        /// </summary>
        [Themeable(false), DefaultValue(1f), Category("Behavior"), Description("The point that marks the lower boundary of the “high” segment of the meter")]
        public float High
        { get { return GetViewState("High", 1f); } set { SetViewState("High", value); } }


        /// <summary>
        /// Gets or sets the point that marks the “optimum” position for the meter
        /// </summary>
        [Themeable(false), DefaultValue(0f), Category("Behavior"), Description("The point that marks the “optimum” position for the meter")]
        public float Optimum
        { get { return GetViewState("Optimum", 0f); } set { SetViewState("Optimum", value); } }

    }
}
