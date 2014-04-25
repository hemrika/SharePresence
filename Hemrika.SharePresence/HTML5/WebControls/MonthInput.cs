using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a control for setting the element’s value to a string representing a month.
    /// </summary>
    public class MonthInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.MonthInput" />
        /// </summary>
        public MonthInput() : base(InputType.Month) { }

        /// <summary>
        /// Gets or sets the selected MonthOfYear
        /// </summary>
        [Themeable(false), DefaultValue(false), Category("Behavior"), Description("Selected Month-Of-Year")]
        public MonthOfYear Value
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                    return MonthOfYear.Empty;
                return MonthOfYear.Parse(Text);
            }
            set
            {
                if (MonthOfYear.IsEmpty(value))
                    Text = string.Empty;
                else

                    Text = value.ToString();
            }
        }
    }
}
