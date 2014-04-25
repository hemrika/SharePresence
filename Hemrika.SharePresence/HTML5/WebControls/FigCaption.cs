using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a caption or legend for a figure.
    /// </summary>
    /// <remarks>This control can only be nested inside a <see cref="Hemrika.SharePresence.Html5.WebControls.Figure" /> Control</remarks>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.Figure"/>
    public class FigCaption : ContainerControl
    {
        /// <summary>
        /// Creates a new instace of <see cref="Hemrika.SharePresence.Html5.WebControls.FigCaption" />
        /// </summary>
        public FigCaption() : base(ContainerType.FigCaption)
        {

        }
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter object that receives the control content.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when nested inside a type other than <see cref="Hemrika.SharePresence.Html5.WebControls.Figure" /></exception>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            var parent = this.Parent as Figure;
            if (parent == null)
                throw new InvalidOperationException("A FigCaption element can only nest inside a figure element");
            base.Render(writer);
        }
    }
}
