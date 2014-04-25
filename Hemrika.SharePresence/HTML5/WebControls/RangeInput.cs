using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    ///  Represents an imprecise control for setting the element’s value to a string representing a number.
    /// </summary>
    public class RangeInput : NumberInput
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.RangeInput" />
        /// </summary>
        public RangeInput() : base(InputType.Range) { }
    }
}
