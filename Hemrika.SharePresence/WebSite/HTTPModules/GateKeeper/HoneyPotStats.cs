using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    class HoneyPotStats
    {
        /// <summary>
        /// When configured, GateKeeper will display the
        /// stats of the Deny list and Honeypot list
        /// via a virtual url path
        /// </summary>
        public static void Display()
        {
            //GateKeeperModule.log.Debug("Entering HoneyPotStats | Display");
            onDisplayStats(HttpContext.Current);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table style=\"font:10px verdana\">");
            sb.AppendLine("\t<tr>");
            sb.AppendLine("\t\t<th style=\"padding: 0pt 15px;\">Creation date</th>");
            sb.AppendLine("\t\t<th>IP Address</th>");
            sb.AppendLine("\t\t<th style=\"padding: 0pt 15px;\">User Agent</th>");
            sb.AppendLine("\t\t<th style=\"padding: 0pt 15px;\">Referral</th>");
            sb.AppendLine("\t</tr>");

            /*
            foreach (XmlNode node in Utils.xmlHoneyPotList.SelectSingleNode("/gatekeeper/honeypot").ChildNodes)
            {
                sb.AppendLine("\t<tr>");
                sb.AppendLine("\t\t<td style=\"padding: 0pt 15px;\">" + DateTime.Parse(node.Attributes["date"].InnerText).ToString("MMMM dd, yyyy HH:mm:ss") + "</td>");
                sb.AppendLine("\t\t<td>" + node.Attributes["ipaddress"].InnerText + "</td>");
                sb.AppendLine("\t\t<td style=\"padding: 0pt 15px;\">" + node.Attributes["useragent"].InnerText + "</td>");
                sb.AppendLine("\t\t<td style=\"padding: 0pt 15px;\">" + node.Attributes["referrer"].InnerText + "</td>");
                sb.AppendLine("\t</tr>");
            }
            */
            sb.AppendLine("</table>");
            HttpContext.Current.Response.Write(sb.ToString());

            onStatsDisplayed(HttpContext.Current);
            //Utils.End_NewRequest(HttpContext.Current);
            HttpContext.Current.Response.End();

        }

        #region events
        public static event EventHandler<EventArgs> DisplayStats;
        private static void onDisplayStats(HttpContext current)
        {
            if (DisplayStats != null)
            {
                //GateKeeperModule.log.Debug("Executing DisplayStats eventHandler");
                DisplayStats(current, new EventArgs()); 
            }
        }
        
        public static event EventHandler<EventArgs> StatsDisplayed;
        private static void onStatsDisplayed(HttpContext current)
        {
            if (StatsDisplayed != null)
            {
                //GateKeeperModule.log.Debug("Executing StatsDisplayed eventHandler");
                StatsDisplayed(current, new EventArgs()); 
            }
        }
        #endregion //events

    }
}
