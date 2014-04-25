
namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents to a track kind in track element.
    /// possible values: "subtitles" or "captions" or "descriptions" or "chapters" or "metadata"
    /// </summary>
    public enum TrackKind
    {
        /// <summary>
        /// track kind: "subtitles"
        /// </summary>
        Subtitles = 0,
        /// <summary>
        /// track kind: "captions"
        /// </summary>
        Captions = 1,
        /// <summary>
        /// track kind: "descriptions"
        /// </summary>
        Descriptions = 2,
        /// <summary>
        /// track kind: "chapters"
        /// </summary>
        Chapters = 3,
        /// <summary>
        /// track kind: "metadata"
        /// </summary>
        Metadata = 4
    }
}
