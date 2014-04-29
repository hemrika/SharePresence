using System;
using System.Web;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Text;
using Hemrika.SharePresence.Common.Logging;
using Hemrika.SharePresence.Common.ServiceLocation;

namespace Hemrika.SharePresence.WebSite.Robots
{
    public class RobotsHandler : IHttpHandler
    {
        //TODO 
        //Crawl-delay: 10
        //Bot Acces Log

        #region IHttpHandler Members

        public static readonly Guid Follow = new Guid("733d9e48-5515-44cb-98a7-7869b68cfe70");
        public static readonly Guid UserAgent = new Guid("a1012e42-3f5f-4ea0-9c88-bd1411765811");
        public static readonly Guid Entry = new Guid("2c4e400c-924b-40a5-9043-00050eda6320");

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            SPSite site = SPContext.Current.Site;
            SPList list = SPContext.Current.Site.RootWeb.Lists.TryGetList("Robots");
           
            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Managed Robots Feature by http://www.sharepresence.com");
            //builder.AppendLine("# " + context.Request.Browser.Browser);
            builder.AppendLine();
            builder.AppendLine(String.Format("Sitemap: {0}/indexmap.xml",SPContext.Current.Site.RootWeb.Url));

            if (list != null)
            {
                Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
                SPListItemCollection items = list.Items;

                foreach (SPListItem item in items)
                {
                    string key = item[UserAgent] as string;
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, new List<string>());
                    }
                    dictionary[key].Add(item[Follow].ToString()+": "+item[Entry].ToString());
                }
                
                foreach (KeyValuePair<string, List<string>> pair in dictionary)
                {
                    builder.AppendLine(string.Format("User-agent: {0}", pair.Key));
                    foreach (string value in pair.Value)
                    {
                        builder.AppendLine(value);
                    }
                    builder.AppendLine();
                }

            }
            
            context.Response.Write(builder.ToString());

            context.Response.Flush();
        }

        #endregion
    }
}
