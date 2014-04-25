
namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// Represents a hint to the user agent about whether optimistic downloading of the audio stream itself or its metadata is considered worthwhile
    /// </summary>
    public enum PreLoadMode
    {
        /// <summary>
        /// Will not be rendered
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// Hints to the user agent that the user is not expected to need the audio/video,
        /// or that minimizing unnecessary traffic is desirable
        /// </summary>
        None = 1,
        /// <summary>
        /// Hints to the user agent that the user is not expected to need the audio/video, 
        /// but that fetching its metadata (dimensions, first frame, track list, duration, and so on) is desirable.
        /// </summary>
        MetaData = 2,
        /// <summary>
        /// Hints to the user agent that optimistically downloading the entire audio/video is considered desirable.
        /// </summary>
        Auto = 3
    }
}
