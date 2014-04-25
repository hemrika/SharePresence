using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a control for setting the element’s value to a string representing a time 
    /// (with no timezone information).
    /// </summary>
    public class TimeInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.TimeInput" />
        /// </summary>
        public TimeInput() : base(InputType.Time) { }

        /// <summary>
        /// Gets or sets the selected Time
        /// </summary>
        [Themeable(false)]
        [Category("Behavior")]
        [Description("Selected Time")]
        public TimeOfDay Value
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return TimeOfDay.Empty;
                return TimeOfDay.Parse(Text);
            }
            set
            {
                if (TimeOfDay.IsEmpty(value))
                    Text = string.Empty;
                else
                    Text = value.ToString();
            }
        }
    }
}
