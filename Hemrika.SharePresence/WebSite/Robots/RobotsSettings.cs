using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.WebSite.Robots
{
    [Serializable]
    public class RobotsSettings
    {
        private int _CrawlDelay;

        public int CrawlDelay
        {
            get { return _CrawlDelay; }
            set { _CrawlDelay = value; }
        }
    }
}
