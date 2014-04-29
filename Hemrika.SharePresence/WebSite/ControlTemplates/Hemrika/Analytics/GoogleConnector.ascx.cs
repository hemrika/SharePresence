using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using Hemrika.SharePresence.Google;
using Hemrika.SharePresence.WebSite.Page;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleConnector : UserControl
    {
        private GoogleSettings _settings = new GoogleSettings();

        protected GoogleSettings Settings
        {
            get { return _settings; }
        }

        private bool _IsSystemPage;

        public bool IsSystemPage
        {
            get { return _IsSystemPage; }
            set { _IsSystemPage = value; }
        }

        private bool _TrackUsers;

        public bool TrackUsers
        {
            get { return _TrackUsers; }
            set { _TrackUsers = value; }
        }

        private static string Hostname(GoogleAnalyticsSettings account)
        {
            try
            {
                if (account.AllowLinker)
                {
                    string[] host = account.WebsiteUrl.Split(new char[] { '.' });

                    int length = host.Length;

                    if (length >= 2)
                    {
                        return host[length - 2] + "." + host[length - 1];
                    }
                    else
                    {
                        return "none";
                    }
                }
                else
                {
                    return "none";
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                return "none";
            }
        }

        protected string WebmasterTools
        {
            get
            {
                StringBuilder meta = new StringBuilder();

                if (!IsSystemPage)
                {
                    try
                    {
                        string core = String.Empty;
                        string[] segments = Request.Url.Host.Split(new char[] { '.' });

                        int length = segments.Length;

                        if (length == 2)
                        {
                            core = segments[0] + "." + segments[1];
                        }
                        if (length > 2)
                        {
                            core = segments[length - 2] + "." + segments[length - 1]; 
                        }

                        if (!string.IsNullOrEmpty(core))
                        {
                            meta.AppendLine("");
                            meta.AppendLine("<!-- Managed Google WebmasterTools -->");

                            foreach (GoogleWebmasterToolsSettings site in Settings.WebmasterTools)
                            {
                                if (site.Domain.Contains(core))
                                {
                                    meta.AppendLine("<!-- " + site.Domain + " ; Verified:" + site.Verified + " -->");
                                    meta.AppendLine(site.MetaTag);
                                }
                            }
                        }
                        else
                        {
                            meta.AppendLine("");
                            meta.AppendLine("<!-- Managed Google WebmasterTools -->");
                            meta.AppendLine("<!-- No matching domains found. -->");
                        }
                    }
                    catch (Exception ex) 
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    }
                }

                return meta.ToString();
            }
        }

        protected string Tracking
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (!IsSystemPage)
                {
                    try
                    {
                        builder.AppendLine("");
                        builder.AppendLine("<!-- Managed Google Tracking -->");
                        builder.AppendLine("<script type=\"text/javascript\">");
                        builder.AppendLine("\tvar _gaq = _gaq || [];");
                        builder.AppendLine(string.Format("\t_gaq.push(['_setAccount', '{0}']);", Settings.Current.WebPropertyId));
                        builder.AppendLine(string.Format("\t_gaq.push(['_setDomainName', '{0}']);", Hostname(Settings.Current)));
                        builder.AppendLine(string.Format("\t_gaq.push(['_setClientInfo', '{0}']);", Settings.Current.ClientInfo.ToString().ToLower()));
                        builder.AppendLine(string.Format("\t_gaq.push(['_setAllowLinker', '{0}']);", Settings.Current.AllowLinker.ToString().ToLower()));
                        builder.AppendLine(string.Format("\t_gaq.push(['_setAllowHash', '{0}']);", Settings.Current.AllowHash.ToString().ToLower()));
                        builder.AppendLine(string.Format("\t_gaq.push(['_setDetectFlash', '{0}']);", Settings.Current.DetectFlash.ToString().ToLower()));
                        builder.AppendLine(string.Format("\t_gaq.push(['_setDetectTitle', '{0}']);", Settings.Current.DetectTitle.ToString().ToLower()));
                        if (SPContext.Current.ListItem != null)
                        {
                            builder.AppendLine(string.Format("\t_gaq.push(['_trackPageview','{0}']);", SPContext.Current.ListItem.File.ServerRelativeUrl));
                        }
                        builder.AppendLine("\t_gaq.push(['_trackPageLoadTime']);");
                        //_gaq.push(['_trackPageview', '/home/landingPage']);
                        int subcount = 0;

                        foreach (GoogleAnalyticsSettings other in Settings.Analytics)
                        {
                            if (other.Name != Settings.Current.Name && other.Active)
                            {
                                subcount++;
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setAccount', '{1}']);", subcount, other.WebPropertyId));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setDomainName', '{1}']);", subcount, Hostname(other)));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setClientInfo', '{1}']);", subcount, other.ClientInfo.ToString().ToLower()));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setAllowLinker', '{1}']);", subcount, other.AllowLinker.ToString().ToLower()));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setAllowHash', '{1}']);", subcount, other.AllowHash.ToString().ToLower()));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setDetectFlash', '{1}']);", subcount, other.DetectFlash.ToString().ToLower()));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_setDetectTitle', '{1}']);", subcount, other.DetectTitle.ToString().ToLower()));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_trackPageview','{1}']);", subcount, SPContext.Current.ListItem.File.ServerRelativeUrl));
                                builder.AppendLine(string.Format("\t_gaq.push(['h{0}_trackPageLoadTime']);", subcount));
                                //builder.AppendLine(string.Format("\t_gaq.push(['h{0}_trackPageview']);", subcount));
                            }
                        }

                        builder.AppendLine("</script>");
                        builder.AppendLine("");
                    }
                    catch (Exception ex) 
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    }
                }
                return builder.ToString();
            }
        }

        protected string Script
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (!IsSystemPage)
                {
                    builder.AppendLine("<script type=\"text/javascript\">");
                    builder.AppendLine("(function() {");
                    builder.AppendLine("\tvar ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
                    builder.AppendLine("\tga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
                    builder.AppendLine("\tvar s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
                    builder.AppendLine("})();");
                    builder.AppendLine("</script>");
                }
                return builder.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IsSystemPage = true;

            _settings = _settings.Load();

            try
            {

                if (Application != null && Request != null && !Request.Url.ToString().ToLower().Contains("_layouts") && !Request.Url.ToString().ToLower().Contains("forms"))
                {
                    IsSystemPage = false;

                }
            }
            catch { };

            try
            {

                if (SPContext.Current.Site.RootWeb.CurrentUser != null && !Settings.Current.Authenticated)
                {
                    TrackUsers = false;
                }
            }
            catch { };

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
        }

    }
}
