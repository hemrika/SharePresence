using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a summary, caption, or legend for a details element.
    /// </summary>
    /// <remarks>This control can only be nested inside a <see cref="Hemrika.SharePresence.Html5.WebControls.Details" /> Control</remarks>
    public class Summary : ContainerControl
    {
        /// <summary>
        /// Creates new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Summary" />
        /// </summary>
        public Summary() : base(ContainerType.Summary) { }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter object that receives the control content.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when nested inside a type other than <see cref="Hemrika.SharePresence.Html5.WebControls.Details" /></exception>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            var parent = this.Parent as Details;
            if (parent == null)
                throw new InvalidOperationException("A summary element can only nest inside a details element");
            base.Render(writer);
        }
    }
}
