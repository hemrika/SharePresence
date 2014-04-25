using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a color-well control, for setting the element’s value to a string representing a simple color.
    /// </summary>
    public class ColorInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.ColorInput" />
        /// </summary>
        public ColorInput() : base(InputType.Color) { }

        /// <summary>
        /// Gets or sets the selected Color
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Selected Color")]
        public string Value//TODO
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
    }
}
