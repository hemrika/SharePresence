using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a one-line plain-text edit control for entering a telephone number.
    /// </summary>
    public class TelInput : InputControl
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.TelInput" />
        /// </summary>
        public TelInput() : base(InputType.Tel) { }

        /// <summary>
        /// Gets or sets the selected Tel
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Selected Tel")]
        public string Value { get { return Text; } set { Text = value; } }
    }
}
