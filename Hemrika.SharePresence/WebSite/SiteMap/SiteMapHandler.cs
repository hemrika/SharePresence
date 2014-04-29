// -----------------------------------------------------------------------
// <copyright file="SiteMapHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Microsoft.SharePoint;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Hemrika.SharePresence.WebSite.ContentTypes;
    using Microsoft.SharePoint.Utilities;
    using System.Globalization;
    using Hemrika.SharePresence.WebSite.Company;
    using Hemrika.SharePresence.WebSite.Fields;
    using Hemrika.SharePresence.WebSite.SiteMap.News;
    using Hemrika.SharePresence.WebSite.Modules.SemanticModule;
using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using Hemrika.SharePresence.WebSite.MetaData.Keywords;
    using Hemrika.SharePresence.WebSite.MetaData;
    using Hemrika.SharePresence.Common;
    using System.Web.UI.WebControls.WebParts;
    using Hemrika.SharePresence.WebSite.Page;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapHandler : IHttpHandler
    {
        StringBuilder textWriter = null;
        SiteMapSettings settings = null;
        CompanySettings company = null;
        Guid siteId = Guid.Empty;
        Guid webId = Guid.Empty;

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            settings = new SiteMapSettings();
            settings = settings.Load();//SPContext.Current.Site);

            SPWeb rootWeb;
            bool dispose = false;

            if (SPContext.Current != null)
            {
                rootWeb = SPContext.Current.Site.RootWeb;
            }
            else
            {
                SPSite site = new SPSite(context.Request.Url.ToString());
                rootWeb = site.RootWeb;
                dispose = true;
            }

            //SPWeb web = SPContext.Current.Web;

            textWriter = new StringBuilder(string.Empty);

            if (rootWeb != null && rootWeb.Exists)
            {
                siteId = rootWeb.Site.ID;
                webId = rootWeb.Site.OpenWeb().ID;

                if (context.Request.Path.ToLower().Contains("index"))
                {
                    BuildSiteIndex();
                }

                if (context.Request.Path.ToLower().Contains("site"))
                {
                    BuildSiteMap();
                }

                if (context.Request.Path.ToLower().Contains("news"))
                {
                    company = new CompanySettings();
                    company = company.Load();

                    BuildNewsMap();
                }

                if (context.Request.Path.ToLower().Contains("video"))
                {
                    BuildVideoMap();
                }

                if (context.Request.Path.ToLower().Contains("mobile"))
                {
                    BuildMobileMap();
                }
            }

            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/xml";
            context.Response.Buffer = false;
            context.Response.HeaderEncoding = Encoding.UTF8;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Write(textWriter.ToString());
            context.Response.Flush();
            context.Response.End();
            if (dispose) { rootWeb.Dispose(); };
        }

        #endregion

        #region SiteIndex

        private void BuildSiteIndex()
        {
            Index.sitemapindex indexmap = new Index.sitemapindex();

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {

                        SPWebCollection webs = site.AllWebs; //GetAllWebs(web);

                        foreach (SPWeb subweb in webs)
                        {
                            if (subweb.AllowAnonymousAccess && subweb.WebTemplate.ToLower().Contains("website"))
                            {
                                Index.tSitemap sitemap = new Index.tSitemap();
                                sitemap.lastmod = subweb.LastItemModifiedDate.ToString("yyyy-MM-dd");//,.ToShortDateString();
                                sitemap.loc = subweb.Url + "/sitemap.xml";
                                indexmap.sitemap.Add(sitemap);

                                if (settings.UseMobile)
                                {
                                    Index.tSitemap mobilemap = new Index.tSitemap();
                                    mobilemap.lastmod = subweb.LastItemModifiedDate.ToString("yyyy-MM-dd");//,.ToShortDateString();
                                    mobilemap.loc = subweb.Url + "/mobilemap.xml";
                                    indexmap.sitemap.Add(mobilemap);
                                }
                            }
                        }

                        if (settings.UseNews)
                        {
                            Index.tSitemap newsmap = new Index.tSitemap();
                            newsmap.lastmod = site.LastContentModifiedDate.ToString("yyyy-MM-dd");//,.ToShortDateString();
                            newsmap.loc = web.Url + "/newsmap.xml";
                            indexmap.sitemap.Add(newsmap);
                        }

                        if (settings.UseVideo)
                        {
                            Index.tSitemap videomap = new Index.tSitemap();
                            videomap.lastmod = site.LastContentModifiedDate.ToString("yyyy-MM-dd");//,.ToShortDateString();
                            videomap.loc = web.Url + "/videomap.xml";
                            indexmap.sitemap.Add(videomap);
                        }

                    }
                }
            });

            textWriter.Append(indexmap.Serialize(Encoding.UTF8));
        }

        #endregion

        #region SiteMap

        private void BuildSiteMap()
        {
            urlset sitemap = new urlset();

            //Guid siteId = SPContext.Current.Site.ID;
            //Guid webId = SPContext.Current.Web.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        foreach (SPList list in web.Lists)
                        {
                            if (ContainsWebPageTypes(list))
                            {
                                SPListItemCollection items = null;

                                SPQuery query = new SPQuery();
                                query.ViewAttributes = "Scope=\"Recursive\"";
                                query.Query = "<Where><IsNotNull><FieldRef Name=\"PublishingPageDesign\" /></IsNotNull></Where>";
                                query.ViewFields = "<FieldRef Name=\"Title\"/><FieldRef Name=\"EncodedAbsUrl\"/><FieldRef Name=\"FileRef\"/><FieldRef Name=\"ContentType\"/><FieldRef Name=\"Created\"/><FieldRef Name=\"Modified\"/>";
                                items = list.GetItems(query);

                                foreach (SPListItem item in items)
                                {
                                    List<SemanticUrl> entries = new List<SemanticUrl>();
                                    if (item.File.Exists && item.File.Level == SPFileLevel.Published)
                                    {
                                        entries = CheckSemantics(item.File.ServerRelativeUrl.ToLower().ToString());

                                        if (entries != null && entries.Count > 0)
                                        {
                                            foreach (SemanticUrl surl in entries)
                                            {
                                                tUrl url = new tUrl();
                                                url.changefreqSpecified = true;
                                                url.changefreq = DetermineFrequency(item["Created"].ToString(), item["Modified"].ToString(), item.Versions.Count);
                                                url.lastmod = DateTime.Parse(item["Modified"].ToString()).ToString("yyyy-MM-dd");//,.ToShortDateString();
                                                url.loc = surl.Semantic;
                                                url.prioritySpecified = true;
                                                url.priority = new decimal(0.5f);
                                                if (!sitemap.url.Contains(url))
                                                {
                                                    sitemap.url.Add(url);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            tUrl url = new tUrl();
                                            url.changefreqSpecified = true;
                                            url.changefreq = DetermineFrequency(item["Created"].ToString(), item["Modified"].ToString(), item.Versions.Count);
                                            url.lastmod = DateTime.Parse(item["Modified"].ToString()).ToString("yyyy-MM-dd");//,.ToShortDateString();
                                            url.loc = item["EncodedAbsUrl"].ToString();
                                            url.prioritySpecified = true;
                                            url.priority = new decimal(0.5f);
                                            if (!sitemap.url.Contains(url))
                                            {
                                                sitemap.url.Add(url);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });

            textWriter.Append(sitemap.Serialize(Encoding.UTF8));
        }

        private List<SemanticUrl> CheckSemantics(string _url)
        {
            SemanticModule module = new SemanticModule();
            List<SemanticUrl> entries = new List<SemanticUrl>();

            Uri baseUri = new Uri(SPContext.Current.Web.Url, UriKind.RelativeOrAbsolute);
            Uri url = new Uri(baseUri, _url); ////WebSiteControllerModule.GetFullUrl(application.Context);

            try
            {
                bool isControlled = false;
                SPSite site = new SPSite(url.OriginalString);
                CheckUrlOnZones(site, url, out url, out isControlled, module.RuleType);

                if (isControlled)
                {
                    System.Collections.Generic.List<WebSiteControllerRule> Allrules = WebSiteControllerConfig.GetRulesForSiteCollection(new Uri(site.Url), module.RuleType);
                    List<WebSiteControllerRule> rules = new List<WebSiteControllerRule>();

                    foreach (WebSiteControllerRule arule in Allrules)
                    {
                        if (arule.RuleType == module.RuleType && arule.Properties.ContainsKey("OriginalUrl"))
                        {
                            string original = arule.Properties["OriginalUrl"].ToString().ToLower();
                            string _lurl = _url.ToString().ToLower();
                            if ((original == _lurl) || (original.EndsWith(_lurl)))
                            {
                                string org = arule.Url;
                                if (org.EndsWith("/"))
                                {
                                    org = org.TrimEnd(new char[1] { '/' });
                                }
                                //if ((org != SPContext.Current.Site.Url) && (org != SPContext.Current.Web.Url))
                                //{
                                    SemanticUrl sem = new SemanticUrl();
                                    sem.OriginalUrl = arule.Properties["OriginalUrl"].ToString().ToLower();
                                    sem.Semantic = arule.Url;
                                    sem.Id = arule.Id;
                                    sem.Disabled = arule.IsDisabled;
                                    entries.Add(sem);
                                //}
                            }
                        }
                    }

                    if (entries.Count > 0)
                    {
                        entries.Sort(SemanticUrl.UrlComparison);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return entries;
        }

        private void CheckUrlOnZones(SPSite site, Uri curl, out Uri url, out bool isControlled, string RuleType)
        {
            Uri zoneUri = curl;

            isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, zoneUri, RuleType);
            UriBuilder builder = new UriBuilder(curl);

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Default);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Internet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Extranet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Intranet);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

            }

            if (!isControlled)
            {
                try
                {
                    zoneUri = site.WebApplication.GetResponseUri(SPUrlZone.Custom);
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }

            if (!isControlled)
            {
                Uri altZone = null;
                foreach (SPAlternateUrl altUrl in site.WebApplication.AlternateUrls)
                {

                    if (altUrl.UrlZone == site.Zone)
                    {
                        altZone = altUrl.Uri;
                        break;
                    }
                }

                if (altZone != null)
                {
                    builder.Host = zoneUri.Host;
                    builder.Port = zoneUri.Port;
                    isControlled = WebSiteControllerConfig.IsPageControlled(site.WebApplication, builder.Uri, RuleType);
                    zoneUri = builder.Uri;
                }
            }
            url = zoneUri;
        }

        private tChangeFreq DetermineFrequency(string created, string modified, int versions)
        {
            TimeSpan tsCreated = new TimeSpan(DateTime.Parse(created).Ticks);
            TimeSpan tsNow = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan span = tsNow.Subtract(tsCreated);

            if (versions == 0)
            {
                return tChangeFreq.never;
            }

            if ((span.TotalSeconds / versions) < 60)
            {
                return tChangeFreq.always;
            }

            if ((span.TotalHours / versions) < 24)
            {
                return tChangeFreq.hourly;
            }

            if ((span.TotalDays / versions) < 7)
            {
                return tChangeFreq.daily;
            }

            if ((span.TotalDays / versions) < 30)
            {
                return tChangeFreq.weekly;
            }

            if ((span.TotalDays / versions) < 365)
            {
                return tChangeFreq.monthly;
            }

            return tChangeFreq.yearly;
        }

        private bool ContainsWebPageTypes(SPList list)
        {
            bool found = false;

            foreach (SPContentType ct in list.ContentTypes)
            {
                string templateUrl = ct.DocumentTemplateUrl;
                string documentControl = ct.NewDocumentControl;

                if (ct.Id == ContentTypeId.WebSitePage || ct.Id.IsChildOf(ContentTypeId.WebSitePage))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        #endregion

        #region NewsMap
        
        private void BuildNewsMap()
        {
            News.urlset newsmap = new News.urlset();

            //Guid siteId = SPContext.Current.Site.ID;
            //Guid webId = SPContext.Current.Web.ID;

            string geo_location = string.Format("{0},{1},{2}", company.City, company.State, company.Country); ;
            geo_location = geo_location.TrimStart(new char[] { ',' });
            geo_location = geo_location.TrimEnd(new char[] { ',' });

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    SPWebCollection webs = site.AllWebs;

                    foreach (SPWeb aweb in webs)
                    {
                        using (SPWeb web = site.OpenWeb(aweb.ID))
                        {
                            foreach (SPList list in web.Lists)
                            {
                                if (ContainsWebPageTypes(list))
                                {
                                    SPListItemCollection items = null;
                                    SPQuery query = new SPQuery();
                                    query.ViewAttributes = "Scope=\"Recursive\"";
                                    query.Query = "<Where><IsNotNull><FieldRef Name=\"PublishingPageDesign\" /></IsNotNull></Where>";
                                    query.ViewFields = "<FieldRef Name=\"Title\"/><FieldRef Name=\"keywords\"/><FieldRef Name=\"EncodedAbsUrl\"/><FieldRef Name=\"FileRef\"/><FieldRef Name=\"ContentType\"/><FieldRef Name=\"Created\"/>";
                                    items = list.GetItems(query);

                                    try
                                    {
                                        foreach (SPListItem item in items)
                                        {
                                            List<SemanticUrl> entries = new List<SemanticUrl>();

                                            if (item.File.Exists && item.File.Level == SPFileLevel.Published && (item.File.TimeLastModified >= (DateTime.Now.AddDays(-3))))
                                            {
                                                try
                                                {
                                                    CultureInfo info = new CultureInfo((int)SPContext.Current.Web.RegionalSettings.LocaleId);
                                                    //CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo((int)web.Language);
                                                    entries = CheckSemantics(item.File.ServerRelativeUrl.ToLower().ToString());

                                                    if (entries != null && entries.Count > 0)
                                                    {
                                                        foreach (SemanticUrl surl in entries)
                                                        {
                                                            News.tUrl url = new News.tUrl();
                                                            url.loc = surl.Semantic;
                                                            url.news.accessSpecified = false;
                                                            url.news.genres = DetermineGenres(item);

                                                            if (item.Fields.ContainsField("keywords"))
                                                            {
                                                                object words = item["keywords"];
                                                                if (words != null)
                                                                {
                                                                    url.news.keywords += GetAutoKeywords(words.ToString(), item);
                                                                }
                                                            }

                                                            url.news.publication.name = web.Title.ToString();
                                                            url.news.accessSpecified = false;
                                                            url.news.publication.language = info.TwoLetterISOLanguageName;
                                                            url.news.publication_date = DateTime.Parse(item[SPBuiltInFieldId.Modified].ToString()).ToString("yyyy-MM-dd");

                                                            if (!String.IsNullOrEmpty(company.Stock))
                                                            {
                                                                url.news.stock_tickers = company.Stock;
                                                            }

                                                            if (!string.IsNullOrEmpty(geo_location))
                                                            {
                                                                url.news.geo_location = geo_location;
                                                            }

                                                            url.news.title = item[SPBuiltInFieldId.Title].ToString();
                                                            newsmap.url.Add(url);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        News.tUrl url = new News.tUrl();
                                                        url.loc = item["EncodedAbsUrl"].ToString();
                                                        url.news.accessSpecified = false;
                                                        url.news.genres = DetermineGenres(item);

                                                        if (item.Fields.ContainsField("keywords"))
                                                        {
                                                            object words = item["keywords"];
                                                            if (words != null)
                                                            {
                                                                url.news.keywords += GetAutoKeywords(words.ToString(), item);
                                                            }
                                                        }

                                                        url.news.publication.name = web.Title.ToString();
                                                        url.news.accessSpecified = false;
                                                        url.news.publication.language = info.TwoLetterISOLanguageName;
                                                        url.news.publication_date = DateTime.Parse(item[SPBuiltInFieldId.Modified].ToString()).ToString("yyyy-MM-dd");

                                                        if (!String.IsNullOrEmpty(company.Stock))
                                                        {
                                                            url.news.stock_tickers = company.Stock;
                                                        }

                                                        if (!string.IsNullOrEmpty(geo_location))
                                                        {
                                                            url.news.geo_location = geo_location;
                                                        }

                                                        url.news.title = item[SPBuiltInFieldId.Title].ToString();
                                                        newsmap.url.Add(url);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ex.ToString();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ToString();
                                    }
                                }
                            }
                        }
                        //<Query><Where><Geq><FieldRef Name="Modified" /><Value Type="DateTime">[4/12/2012 11:53:33 AM-30Day(s)]</Value></Geq></Where></Query>
                        //<Query><Where><And><Geq><FieldRef Name="Modified" /><Value Type="DateTime">[4/12/2012 11:28:42 AM-2Day(s)]</Value></Geq><IsNotNull><FieldRef Name=\"PublishingPageDesign\" /></IsNotNull><And></Where></Query>
                    }
                }
            });

            textWriter.Append(newsmap.Serialize(Encoding.UTF8));
        }

        private MetaDataSettings metasettings;
        private IEnumerable<SPField> contentfields;

        private SPListItem _listItem;

        public SPListItem listItem
        {
            get { return _listItem; }
            set { _listItem = value; }
        }
        private IEnumerable<Keyword> keywords;

        private string GetAutoKeywords(string uservalue, SPListItem item)
        {
            metasettings = new MetaDataSettings();
            metasettings = metasettings.Load(SPContext.Current.Site);

            StringBuilder content = new StringBuilder();

            GetContentFields();

            foreach (SPField oField in contentfields)
            {
                string fieldname = SharePointWebControls.GetFieldName(oField);

                if (listItem.Fields.ContainsField(fieldname))
                {
                    try
                    {
                        SPField field = listItem.Fields.GetField(fieldname);
                        string value = field.GetFieldValueAsText(listItem[fieldname]);
                        //string value = listItem[fieldname] as string;
                        content.Append(value + " ");
                    }
                    catch { };
                }
            }

            /*
            if(WebSitePage.IsWebSitePage(item))
            {
                WebSitePage page = new WebSitePage();
            }
            
            WebPartManager webPartManager = WebPartManager.GetCurrentWebPartManager(item.File);

            if (webPartManager != null)
            {
                foreach (WebPart webpart in webPartManager.WebParts)
                {
                    try
                    {
                        IWebPartMetaData data = (IWebPartMetaData)webpart;
                        if (data != null)
                        {
                            content.Append(data.MetaData() + " ");
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                }
            }
            */

            int providednumber = 0;
            if (!String.IsNullOrEmpty(uservalue))
            {
                providednumber = uservalue.Split(new char[] { ',' }).Length;
            }
            string clean = HtmlRemoval.StripTagsCharArray(content.ToString());
            KeywordAnalyzer analyzer = new KeywordAnalyzer();
            KeywordAnalysis analysis = analyzer.Analyze(clean);
            int numberleft = (metasettings.NumberOfkeywords - providednumber);

            StringBuilder builder = new StringBuilder();

            if (numberleft > 0)
            {
                keywords = analysis.Keywords.Take(numberleft);

                if (!uservalue.EndsWith(","))
                {
                    builder.Append(", ");
                }

                foreach (Keyword keyword in keywords)
                {
                    builder.AppendFormat("{0}, ", keyword.Word);
                }
            }

            string returnvalue = builder.ToString();
            returnvalue = returnvalue.TrimEnd(new char[] { ',', ' ' });
            returnvalue = returnvalue.TrimStart(new char[] { ',', ' ' });

            return returnvalue;
        }

        private void GetContentFields()
        {
            Func<SPField, bool> contentgroup = null;

            if (contentgroup == null)
            {
                contentgroup = delegate(SPField f)
                {
                    return f.Group != metasettings.GroupName && f.Hidden == false;
                };
            }

            SPFieldCollection fields = listItem.Fields;
            contentfields = fields.Cast<SPField>().Where<SPField>(contentgroup);
        }

        private tUrlNewsGenres DetermineGenres(SPListItem item)
        {
            tUrlNewsGenres genres = tUrlNewsGenres.UserGenerated;

            try
            {

                if (!item.Fields.Contains(BuildFieldId.NewsGenres))
                {
                    SPField genresField = SPContext.Current.Web.Fields.GetFieldByInternalName("news_genres");
                    item.Fields.Add(genresField);
                    item.Update();
                }

                if (item.Properties.ContainsKey("News Genres"))
                {
                    //object obj = item[BuildFieldId.NewsGenres];
                    //string value = item.Properties["News Genres"] as string;

                    //object newgenres = item[BuildFieldId.NewsGenres];
                    object newsgenres = item.Properties["News Genres"];
                    string g = newsgenres.ToString();
                    string[] sgenres = g.Split(new string[1] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    //bool notset = true;
                    foreach (string sgenre in sgenres)
                    {
                        //if (notset)
                        //{
                            genres = (tUrlNewsGenres) Enum.Parse(typeof(tUrlNewsGenres), sgenre, true);
                            break;
                        //    notset = false;
                        //}
                        //else
                        //{
                        //    tUrlNewsGenres extra = (tUrlNewsGenres)Enum.Parse(typeof(tUrlNewsGenres), sgenre, true);
                        //    genres = genres | extra;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return genres;
        }

        #endregion

        #region MobileMap

        private void BuildMobileMap()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region VideoMap

        private void BuildVideoMap()
        {
            throw new NotImplementedException();
        }

        #endregion

        private string BuildSiteMap(string siteUrl, bool recursive)
        {
            string sitemap = string.Empty;
            try
            {

                if (recursive)
                {
                    //using (SPWeb web = SPContext.Current.Site.RootWeb)
                    //{
                        StringBuilder mapWriter = new StringBuilder(string.Empty);

                        SPSiteDataQuery sdquery = new SPSiteDataQuery();

                        sdquery.Query = "<Where><Neq><FieldRef Name=\"ContentType\" /><Value Type=\"Text\"></Value></Neq></Where>";
                        sdquery.ViewFields = "<FieldRef Name=\"Title\"/>";//<FieldRef Name=\"EncodedAbsUrl\"/><FieldRef Name=\"FileRef\"/><FieldRef Name=\"ContentType\"/><FieldRef Name=\"Modified\"/>";
                        sdquery.Lists = "<Lists ServerTemplate=\"20002\"/>";
                        //sdquery.Lists = "<Lists BaseType=\"1\" />";
                        //sdquery.Webs = "<Webs Scope=\"SiteCollection\" />";
                        //sdquery.QueryThrottleMode = SPQueryThrottleOption.Override;

                        DataTable pages = SPContext.Current.Site.RootWeb.GetSiteData(sdquery);// web.GetSiteData(sdquery);
                        pages.TableName = "SiteMap";
                        StringWriter sw = new StringWriter(mapWriter);
                        pages.WriteXml(sw);
                        sw.Flush();

                        sitemap = mapWriter.ToString();
                    //}
                }

                //using (SPSite site = new SPSite(siteUrl))
                //{
                //    using (SPWeb web = site.OpenWeb())
                //    {
                //        textWriter.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                //        textWriter.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                //        LoadTreeViewForSubWebs(web); // kick it off with the root web
                //        textWriter.Append("</urlset>");
                //        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(textWriter.ToString()));
                //        web.Files.Add("sitemap.xml", stream, true);
                //        stream.Close();
                //    }
                //}

            }//try

            catch (Exception ex)
            {
                ex.ToString();
                //errorHandler(ex, "BuildSitemap");
            }

            return sitemap;
        }
    }
}
