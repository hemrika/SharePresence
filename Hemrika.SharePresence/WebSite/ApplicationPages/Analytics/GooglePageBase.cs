// -----------------------------------------------------------------------
// <copyright file="GooglePageBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Google.Analytics;
    using Hemrika.SharePresence.Google.WebmasterTools;
    using Hemrika.SharePresence.Google;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GooglePageBase : EnhancedLayoutsPage
    {
        private GoogleSettings _settings = new GoogleSettings();

        public GoogleSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        private AnalyticsService _analytics = null;

        public AnalyticsService Analytics
        {
            get
            {
                if (_analytics == null)
                {
                    _analytics = new AnalyticsService("SharePresenceAnalytics");
                }

                return _analytics;
            }
        }


        private WebmasterToolsService _webmastertools = null;

        public WebmasterToolsService Webmastertools
        {
            get
            {
                if (_webmastertools == null)
                {
                    _webmastertools = new WebmasterToolsService("SharePresenceWebmasterTools");
                }

                return _webmastertools;
            }
        }

        private string _referringpage = null;

        public string Referringpage
        {
            get
            {
                if (_referringpage == null)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(base.Request.QueryString["Path"]))
                        {
                            _referringpage = base.Request.QueryString["Path"].ToString();
                        }
                        else
                        {
                            _referringpage = null;
                        }
                    }
                    catch
                    {
                        _referringpage = null;
                    }
                }

                return _referringpage;
            }
        }

        private string _report = null;

        public string Report
        {
            get
            {
                if (_report == null)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(base.Request.QueryString["Report"]))
                        {
                            _report = base.Request.QueryString["Report"].ToString();
                        }
                        else
                        {
                            _report = null;
                        }
                    }
                    catch
                    {
                        _report = null;
                    }
                }

                return _report;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            _settings = _settings.Load();// (SPContext.Current.Site);
            /*
            try
            {
                Analytics.SetAuthenticationToken(Settings.Current.Token);
                Webmastertools.SetAuthenticationToken(Settings.Current.Token);
            }
            catch (Exception ex)
            {
                //lbl_error.Text += "Token" + ex.ToString();
            }
            */

            try
            {
                Analytics.setUserCredentials(Settings.Username, Settings.Password);
                Webmastertools.setUserCredentials(Settings.Username, Settings.Password);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //lbl_error.Text += "Credentials" + ex.ToString();
            }

            /*
            try
            {
                Analytics.setUserCredentials("hemrika@gmail.com", "557308453");
                Webmastertools.setUserCredentials("hemrika@gmail.com", "557308453");
            }
            catch (Exception ex)
            {
                //lbl_error.Text += "Credentials" + ex.ToString();
            }
            */
        }

        public override Hemrika.SharePresence.Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }

        public override Type SetTypeForLicense()
        {
            return typeof(GooglePageBase);
            //throw new NotImplementedException();
        }
    }
}