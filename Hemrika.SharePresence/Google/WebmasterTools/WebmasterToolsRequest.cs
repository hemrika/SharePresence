namespace Hemrika.SharePresence.Google.WebmasterTools
{
    using Client;
    using WebmasterTools;
    using System;

    public class WebmasterToolsRequest : FeedRequest<WebmasterToolsService>
    {
        public WebmasterToolsRequest(RequestSettings settings) : base(settings)
        {
            base.Service = new WebmasterToolsService(settings.Application);
            base.PrepareService();
        }

        public Sites AddSite(Sites s)
        {
            Sites sites = null;
            if (s != null)
            {
                Uri feedUri = base.CreateUri("https://www.google.com/webmasters/tools/feeds/sites/");
                sites = new Sites();
                sites.AtomEntry = base.Service.Insert<AtomEntry>(feedUri, s.AtomEntry);
            }
            return sites;
        }

        public Sitemap AddSitemap(string siteId, Sitemap sitemap)
        {
            Sitemap sitemap2 = null;
            if (sitemap.AtomEntry != null)
            {
                Uri feedUri = base.CreateUri(SitemapsQuery.CreateCustomUri(siteId));
                sitemap2 = new Sitemap();
                sitemap2.AtomEntry = base.Service.Insert<AtomEntry>(feedUri, sitemap.AtomEntry);
            }
            return sitemap2;
        }

        public Feed<CrawlIssues> GetCrawlIssues(string siteId)
        {
            CrawlIssuesQuery q = base.PrepareQuery<CrawlIssuesQuery>(CrawlIssuesQuery.CreateCustomUri(siteId));
            return this.PrepareFeed<CrawlIssues>(q);
        }

        public Feed<Keywords> GetKeywords(string siteId)
        {
            KeywordsQuery q = base.PrepareQuery<KeywordsQuery>(KeywordsQuery.CreateCustomUri(siteId));
            return this.PrepareFeed<Keywords>(q);
        }

        public Feed<Messages> GetMessages()
        {
            MessagesQuery q = base.PrepareQuery<MessagesQuery>("https://www.google.com/webmasters/tools/feeds/messages/");
            return this.PrepareFeed<Messages>(q);
        }

        public Feed<Sitemap> GetSitemaps(string siteId)
        {
            SitemapsQuery q = base.PrepareQuery<SitemapsQuery>(SitemapsQuery.CreateCustomUri(siteId));
            return this.PrepareFeed<Sitemap>(q);
        }

        public Feed<Sites> GetSites()
        {
            SitesQuery q = base.PrepareQuery<SitesQuery>("https://www.google.com/webmasters/tools/feeds/sites/");
            return this.PrepareFeed<Sites>(q);
        }

        public Sites UpdateSiteEntry(Sites entry, string siteId)
        {
            Sites sites = null;
            if (entry != null)
            {
                Uri uri = base.CreateUri(SitesQuery.CreateCustomUri(siteId).AbsoluteUri);
                entry.AtomEntry.EditUri = uri;
                SitesEntry entry2 = base.Service.Update<SitesEntry>(entry.SitesEntry);
                if (entry2 != null)
                {
                    sites = new Sites();
                    sites.AtomEntry = entry2;
                }
            }
            return sites;
        }
    }
}

