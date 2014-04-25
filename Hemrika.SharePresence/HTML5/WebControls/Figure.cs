
namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a unit of content, optionally with a caption, that is self-contained, 
    /// that is typically referenced as a single unit from the main flow of the document, 
    /// and that can be moved away from the main flow of the document without affecting the document’s meaning.
    /// </summary>
    /// <seealso cref="Hemrika.SharePresence.Html5.WebControls.FigCaption"/>
    public class Figure : ContainerControl
    {
        /// <summary>
        /// Creates a new instance of <see cref="Hemrika.SharePresence.Html5.WebControls.Figure" />
        /// </summary>
        public Figure() : base(ContainerType.Figure) { }
    }
}
