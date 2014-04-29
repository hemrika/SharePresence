using System;
using System.Globalization;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System.Net;
using System.Web;

namespace Hemrika.SharePresence.WebSite.SiteMap
{
    public class SiteMapPing : SPJobDefinition
    {
        public SiteMapPing() : base() { }
        public SiteMapPing(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.Job)
        { this.Title = "SiteMap Ping"; }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                foreach (SPSite site in WebApplication.Sites)
                {
                    long lastmod = site.LastContentModifiedDate.Ticks;
                    long lastrun = LastRunTime.Ticks;
                    SPMonthlySchedule schedule = Schedule as SPMonthlySchedule;
                   //Schedule.GetType()
                    SiteMapSettings settings = new SiteMapSettings();
                    settings = settings.Load(site);

                    foreach (string searchengine in settings.SearchEngines)
                    {
                        string sitemap = string.Format("{0}/indexmap.xml", site.RootWeb.Url);
                        SubmitMap(searchengine,sitemap);


                        if (settings.UseMobile)
                        {
                            sitemap = string.Format("{0}/mobilemap.xml", site.RootWeb.Url);
                            SubmitMap(searchengine,sitemap);
                        }

                        if (settings.UseNews)
                        {
                            sitemap = string.Format("{0}/newsmap.xml", site.RootWeb.Url);
                            SubmitMap(searchengine,sitemap);
                        }

                        if (settings.UseVideo)
                        {
                            sitemap = string.Format("{0}/videomap.xml", site.RootWeb.Url);
                            SubmitMap(searchengine,sitemap);
                        }
                    }

                }
                this.UpdateProgress(100);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            finally
            {
            }
        }

        private void SubmitMap(string searchengine,string url)
        {
            try
            {
            System.Net.WebRequest request = System.Net.WebRequest.Create(searchengine + HttpUtility.UrlEncode(url));
            request.GetResponse();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            /*
            //Google
            System.Net.WebRequest reqGoogle = System.Net.WebRequest.Create("http://www.google.com/webmasters/tools/ping?sitemap=" + HttpUtility.UrlEncode(url));
            reqGoogle.GetResponse();

            //Ask
            System.Net.WebRequest reqAsk = System.Net.WebRequest.Create("http://submissions.ask.com/ping?sitemap=" + HttpUtility.UrlEncode(url));
            reqAsk.GetResponse();

            //Binging
            System.Net.WebRequest reqBing = System.Net.WebRequest.Create("http://www.bing.com/webmaster/ping.aspx?siteMap=" + HttpUtility.UrlEncode(url));
            reqBing.GetResponse();

            //Live
            System.Net.WebRequest reqLive = System.Net.WebRequest.Create("http://webmaster.live.com/ping.aspx?siteMap=" + HttpUtility.UrlEncode(url));
            reqLive.GetResponse();

            //MoreOver
            System.Net.WebRequest reqMore = System.Net.WebRequest.Create("http://api.moreover.com/ping?u=" + HttpUtility.UrlEncode(url));
            reqMore.GetResponse();
            */
        }
    }
}
