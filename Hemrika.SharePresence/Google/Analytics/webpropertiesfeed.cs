// -----------------------------------------------------------------------
// <copyright file="webproperties.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Google.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Google.Client;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPropertiesFeed : AbstractFeed
    {
        //private List<Segment> segments;
        
        /// <summary>
        ///  default constructor
        /// </summary>
        /// <param name="uriBase">the base URI of the feedEntry</param>
        /// <param name="iService">the Service to use</param>
        public WebPropertiesFeed(Uri uriBase, IService iService)
            : base(uriBase, iService)
        {
            //AddExtension(new Segment());
        }

        /// <summary>
        /// This needs to get implemented by subclasses
        /// </summary>
        /// <returns>AtomEntry</returns>
        public override AtomEntry CreateFeedEntry()
        {
            return new WebPropertyEntry();
        }

        /// <summary>
        /// Is called after we already handled the custom entry, to handle all 
        /// other potential parsing tasks
        /// </summary>
        /// <param name="e"></param>
        /// <param name="parser">the atom feed parser used</param>
        protected override void HandleExtensionElements(ExtensionElementEventArgs e, 
                                                        AtomFeedParser parser)
        {
            base.HandleExtensionElements(e, parser);
        }

        /*
        /// <summary>
        /// This field controls the segments.
        /// </summary>
        public List<Segment> Segments
        {
            get
            {
                if (segments == null)
                {
                    segments = FindExtensions<Segment>(AnalyticsNameTable.XmlSegmentElement, 
                                                       AnalyticsNameTable.gAnalyticsNamspace);
                }
                return segments;
            }
        }
        */
    }
}
