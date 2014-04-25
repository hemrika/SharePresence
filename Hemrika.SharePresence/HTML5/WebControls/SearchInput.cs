using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a one-line plain-text edit control for entering one or more search terms.
    /// </summary>
    public class SearchInput : InputControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.SearchInput" />
        /// </summary>
        public SearchInput() : base(InputType.Search) { }

        /// <summary>
        /// Gets or sets the selected value to search
        /// </summary>
        [Themeable(false), DefaultValue(""), Category("Behavior"), Description("Selected value to search")]
        public string Value { get { return Text; } set { Text = value; } }
    }
}
