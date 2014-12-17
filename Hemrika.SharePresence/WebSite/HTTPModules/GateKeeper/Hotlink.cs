using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    // Thanks to A List Apart (www.alistapart.com/articles/hotlinking) for helping understand hotlinking
    // Testing can be done at http://altlab.com/hotlinkchecker.php 
    class Hotlink
    {
        public static Boolean IsHotLink(HttpContext current)
        {
            if (current.Request.UrlReferrer == null) 
            {
                //GateKeeperModule.log.Debug("UrlReferrer empty");
                return false;
            }

            // Compare url request against the hotlink matching expression ex. (png|gif|jpg|bmp)
            if (!Regex.IsMatch(current.Request.Url.PathAndQuery, GateKeeperModule.config.HotLinkExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                //GateKeeperModule.log.DebugFormat("Url did not match hotlink expression : [{0}]", GateKeeperModule.config.HotLinkExpression);
                return false; 
            }

            // Check that the request url.scheme and url.host (ex. (http/https)://www.domain.com...) matches the urlreferrer value
            Regex regExpression = new Regex(string.Format("^{0}://{1}", current.Request.Url.Scheme, current.Request.Url.Host), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (regExpression.IsMatch(current.Request.UrlReferrer.AbsoluteUri))
            {
                //GateKeeperModule.log.Debug("Referral from self");
                return false; 
            }

            // Check if included in the hotlink allowed list
            if (Regex.IsMatch(current.Request.UrlReferrer.AbsoluteUri, GateKeeperModule.config.HotLinkAllowedSites, RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                //GateKeeperModule.log.Debug("Permitted hotlink");
                return false; 
            }

            // Check if we should display a deny image
            if (GateKeeperModule.config.DisplayHotLinkDenyImage && !string.IsNullOrEmpty(GateKeeperModule.config.HotLinkDenyImage))
            {
                string hotlinkpath = string.Format("~/{0}", GateKeeperModule.config.HotLinkDenyImage);
                //GateKeeperModule.log.DebugFormat("HotLink Deny Image will be displayed : [{0}]", hotlinkpath);
                current.Response.StatusCode = 301;
                current.Response.Redirect(hotlinkpath, false);
            }

            // After failing all tests this must be a hotlink request...
            OnHotlinkedTagged(current);
            //GateKeeperModule.log.Info("Request denied : [HOTLINK]");
            return true;
        }

        #region events
        /// <summary>
        /// Occurs after a request has been tagged a hotlink.
        /// </summary>
        public static event EventHandler<EventArgs> HotlinkedTagged;
        private static void OnHotlinkedTagged(HttpContext current)
        {
            if (HotlinkedTagged != null)
            {
                //GateKeeperModule.log.Debug("Executing HotlinkTagged eventHandler");
                HotlinkedTagged(current, new EventArgs());
            }
        }

        #endregion //events
    }
}
