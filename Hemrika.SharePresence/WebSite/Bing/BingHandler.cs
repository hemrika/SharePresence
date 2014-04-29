using System;
using System.Security.Permissions;
using System.Web;
using System.Xml;
using System.Web.Script.Serialization;
using System.Text;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.Bing
{
    public class BingHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            BingSettings settings = new BingSettings();
            settings = settings.Load();

            context.Response.ContentType = "text/xml";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            StringBuilder bing = new StringBuilder();
            bing.AppendLine("<?xml version=\"1.0\"?>");
            bing.AppendLine("<users>");
            bing.Append("<user>");
            bing.Append(settings.User);
            bing.Append("</user>");
            bing.AppendLine("</users>");

            context.Response.Write(bing.ToString());

            context.Response.Flush();
        }

        #endregion
    }
}
