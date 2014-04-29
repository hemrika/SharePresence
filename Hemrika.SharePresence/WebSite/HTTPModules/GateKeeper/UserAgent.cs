using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    [Serializable]
    public class UserAgent : Node, IComparable<UserAgent>
    {
        private string useragent;
        
        public string userAgent
        {
            get { return useragent; }
            set { useragent = value; }
        }

        public static Comparison<UserAgent> UAComparison = delegate(UserAgent p1, UserAgent p2)
        {
            return p1.useragent.CompareTo(p2.useragent);
        };

        public int CompareTo(UserAgent other)
        {
            return useragent.CompareTo(other.useragent);
        }

        
        /// <summary>
        /// Check if the UserAgent already exists
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Boolean UserAgentExists(string value)
        {
            /*
            XmlNodeList entryNodes = document.SelectNodes("/gatekeeper/useragent/entry");
            foreach (XmlNode node in entryNodes)
            {
                string pattern = node.Attributes[@"useragent"].Value;
                //GateKeeperModule.log.DebugFormat("Matching UserAgent entry : {0} = {1}", value, pattern);
                if (!string.IsNullOrEmpty(pattern) && Regex.IsMatch(value, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase))
                {
                    //GateKeeperModule.log.Debug("UserAgent match found");
                    return true; 
                }
            }
            //GateKeeperModule.log.Debug("UserAgent match not found");
            */
            return false;
        }
    }
}
