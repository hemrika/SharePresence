using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Web.Extensions;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Google.Analytics;
using System.Text;
using System.Linq;
using Hemrika.SharePresence.Google.WebmasterTools;
using Hemrika.SharePresence.Google;
using Hemrika.SharePresence.Google.Client;
using System.Collections.Generic;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
	public partial class GoogleAccountProfiles : UserControl
	{
		GooglePageBase google;

		protected void Page_Load(object sender, EventArgs e)
		{
			google = Page as GooglePageBase;

			if (google.Settings.Analytics.Length == 0 && !String.IsNullOrEmpty(google.Settings.Username) && !string.IsNullOrEmpty(google.Settings.Password))
			{
				ReloadAnalytics();
				
			}

			if (google.Settings.WebmasterTools.Length == 0 && !String.IsNullOrEmpty(google.Settings.Username) && !string.IsNullOrEmpty(google.Settings.Password))
			{
				ReloadWebmasterTools();
			}

			if (!Page.IsPostBack)
			{

				foreach (GoogleAnalyticsSettings analytics in google.Settings.Analytics)
				{
					if (SPContext.Current.Site.HostName == analytics.WebsiteUrl)
					{
                        tbx_Hostname.Text = analytics.Name;
						tbx_AccountId.Text = analytics.AccountId;
						//tbx_ProfileId.Text = analytics.ProfileId;
						//tbx_Token.Text = analytics.Token;
						cbx_Active.Checked = analytics.Active;
						cbx_ClientInfo.Checked = analytics.ClientInfo;
						cbx_Flash.Checked = analytics.DetectFlash;
						cbx_Hash.Checked = analytics.AllowHash;
						cbx_Linker.Checked = analytics.AllowLinker;
						cbx_Title.Checked = analytics.DetectTitle;
                        ddl_Google_Account_Profiles.Items.Add(new ListItem(analytics.Name, analytics.WebPropertyId, true));
						ddl_Google_Account_Profiles.SelectedValue = analytics.WebPropertyId;
					}
					else
					{
                        ddl_Google_Account_Profiles.Items.Add(new ListItem(analytics.Name, analytics.WebPropertyId));
					}
				}
			}
		}

		private void ReloadWebmasterTools()
		{
			SitesQuery qeury = new SitesQuery();

			SitesFeed sites = google.Webmastertools.Query(qeury);

			google.Settings.WebmasterTools = new GoogleWebmasterToolsSettings[sites.Entries.Count];

			int index = 0;
			foreach (SitesEntry entry in sites.Entries)
			{
				try
				{
					GoogleWebmasterToolsSettings settings = new GoogleWebmasterToolsSettings();
					settings.Domain = entry.Title.Text;
					settings.MetaTag = entry.VerificationMethod.Value;
					settings.Verified = Boolean.Parse(entry.Verified);
					string[] segments = entry.Id.Uri.Content.Split(new char[] { '/' });
					int length = segments.Length;
					settings.EncodedSiteId = segments[length - 1];
					google.Settings.WebmasterTools.SetValue(settings, index);
					index++;
				}
				catch (Exception ex)
				{
					ex.ToString();
				}
			}
			google.Settings.Save();
		}

		private void ReloadAnalytics()
		{
            List<GoogleAnalyticsSettings> settings = new List<GoogleAnalyticsSettings>();

            FeedQuery jsonFeed = new FeedQuery(AccountQuery.HttpsFeedUrl);
            RootObject Accounts = google.Analytics.JSON(jsonFeed);

            foreach (Item account in Accounts.items)
            {
                if (account.childLink != null)
                {
                    jsonFeed = new FeedQuery(account.childLink.href);
                    RootObject WebProperties = google.Analytics.JSON(jsonFeed);

                    foreach (Item webProperty in WebProperties.items)
                    {
                        if (webProperty.childLink != null)
                        {
                            jsonFeed = new FeedQuery(webProperty.childLink.href);
                            RootObject Profiles = google.Analytics.JSON(jsonFeed);

                            if (Profiles.items != null)
                            {
                                foreach (Item profile in Profiles.items)
                                {
                                    GoogleAnalyticsSettings setting = new GoogleAnalyticsSettings();
                                    setting.AccountId = profile.accountId;
                                    setting.Created = profile.created;
                                    setting.Currency = profile.currency;
                                    setting.DefaultPage = profile.defaultPage;
                                    setting.ECommerceTracking = profile.eCommerceTracking;
                                    setting.Id = profile.id;
                                    setting.InternalWebPropertyId = profile.internalWebPropertyId;
                                    setting.Name = profile.name;
                                    setting.Timezone = profile.timezone;
                                    setting.WebPropertyId = profile.webPropertyId;
                                    setting.WebsiteUrl = profile.websiteUrl;
                                    /*
                                    profile.profileId
                                    setting.ProfileId = profile.ProfileId;
                                    setting.WebPropertyId = profile.WebPropertyId;
                                    setting.ProfileName = profile.ProfileName;
                                    setting.TableId = profile.TableId;
                                    */
                                    if (SPContext.Current.Site.HostName == profile.websiteUrl)
                                    {
                                        setting.Active = true;
                                    }

                                    settings.Add(setting);

                                    /*
                                    if (profile.childLink != null)
                                    {
                                        jsonFeed = new FeedQuery(profile.childLink.href);
                                        RootObject SubSubItemroot = google.Analytics.JSON(jsonFeed);
                                    }
                                    */
                                }
                            }
                        }
                    }
                }
            }

            /*
            AccountQuery query = new AccountQuery();

            AccountFeed accountFeed = google.Analytics.Query(query);

            //google.Settings.Analytics = new GoogleAnalyticsSettings[accountFeed.Entries.Count];

            List<GoogleAnalyticsSettings> settings = new List<GoogleAnalyticsSettings>();

            foreach (AccountEntry accountEntry in accountFeed.Entries)
            {
                try
                {
                    foreach (WebPropertiesQuery webpropertiesQuery in accountEntry.WebProperties)
                    {
                        WebPropertiesFeed webpropertiesFeed = google.Analytics.Query(webpropertiesQuery);

                        foreach (WebPropertyEntry webpropertyEntry in webpropertiesFeed.Entries)
                        {
                            foreach (ProfilesQuery profile in webpropertyEntry.Profiles)
                            {
                                ProfilesFeed profilesFeed = google.Analytics.Query(profile);

                                foreach (ProfilesEntry entry in profilesFeed.Entries)
                                {

                                    GoogleAnalyticsSettings setting = new GoogleAnalyticsSettings();

                                    setting.AccountId = entry.AccountId;
                                    setting.ProfileId = entry.ProfileId;
                                    setting.WebPropertyId = entry.WebPropertyId;
                                    setting.ProfileName = entry.ProfileName;
                                    setting.TableId = entry.TableId;
                                    settings.Add(setting);
                                }
                            }
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    //lbl_error.Text = "Account" + ex.ToString();
                }
            }
            */
            google.Settings.Analytics = settings.ToArray<GoogleAnalyticsSettings>();
            google.Settings.Save();

                    /*
                    GoogleAnalyticsSettings settings = new GoogleAnalyticsSettings();

                    settings.HostName = entry.Title.Text;
                    settings.ProfileId = entry.ProfileId.Value;
                    settings.TableId = entry.ProfileId.Value;

                    foreach (Property prop in entry.Properties)
                    {
                        if (prop.Name == "ga:webPropertyId")
                        {
                            settings.AccountId = prop.Value.ToString();
                            settings.WebPropertyId = prop.Value.ToString();
                            break;
                        }
                    }
                    if (SPContext.Current.Site.HostName == entry.Title.Text)
                    {
                        settings.Active = true;
                    }
                    google.Settings.Analytics.SetValue(settings, index);
                    index++;
                    */

            /*
            DataQuery accountQuery = new DataQuery(AccountQuery.HttpsFeedUrl);
            DataFeed accountFeed = google.Analytics.Query(accountQuery);

            google.Settings.Analytics = new GoogleAnalyticsSettings[accountFeed.Entries.Count];
            /*
            foreach (AtomEntry account in accountFeed.Entries)
            {
                string webproperties = account.Links.Where(x => x.HRef.Content.EndsWith("webproperties")).First().HRef.Content;
            }

            string webPropsUrl = accountFeed.Entries.First().Links.Where(x => x.HRef.Content.EndsWith("webproperties")).First().HRef.Content;

            DataFeed webPropFeed = google.Analytics.Query(new DataQuery(webPropsUrl));

            foreach (AtomEntry entry in webPropFeed.Entries)
            {
                List<AtomLink> profileFeedUrl = entry.Links.Where(x => x.HRef.Content.EndsWith("profiles")).ToList<AtomLink>();
                foreach (AtomLink link in profileFeedUrl)
                {
                    DataFeed profileFeed = google.Analytics.Query(new DataQuery(link.HRef.Content));

                    foreach (AtomEntry profile in profileFeed.Entries)
                    {
                        string profileUrl = profile.Links[0].HRef.Content;
                        string profileId = profileUrl.Split(new char[]{'/'}).Last();//(profileUrl.Split("/").Length - 1)
                    }
                }
            }
            */

            /*
			AccountQuery query = new AccountQuery();

			AccountFeed accountFeed = google.Analytics.Query(query);
            */
/*
			StringBuilder builder = new StringBuilder();

			google.Settings.Analytics = new GoogleAnalyticsSettings[accountFeed.Entries.Count];

			int index = 0;
			foreach (AccountEntry entry in accountFeed.Entries)
			{
				try
				{
					GoogleAnalyticsSettings settings = new GoogleAnalyticsSettings();

					settings.HostName = entry.Title.Text;
					settings.ProfileId = entry.ProfileId.Value;
					settings.TableId = entry.ProfileId.Value;

					foreach (Property prop in entry.Properties)
					{
						if (prop.Name == "ga:webPropertyId")
						{
							settings.AccountId = prop.Value.ToString();
							settings.WebPropertyId = prop.Value.ToString();
							break;
						}
					}
					if (SPContext.Current.Site.HostName == entry.Title.Text)
					{
						settings.Active = true;
					}
					google.Settings.Analytics.SetValue(settings, index);
					index++;
				}
				catch (Exception ex)
				{
					//lbl_error.Text = "Account" + ex.ToString();
				}
			}

			google.Settings.Save();
 */
		}

		protected void ddl_Google_Account_Profiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			lbl_Profile_correct.Text = string.Empty;
			foreach (GoogleAnalyticsSettings analytics in google.Settings.Analytics)
			{
				if (ddl_Google_Account_Profiles.SelectedValue == analytics.WebPropertyId)
				{
					tbx_Hostname.Text = analytics.Name;
					tbx_AccountId.Text = analytics.AccountId;
					//tbx_ProfileId.Text = analytics.ProfileId;
					//tbx_Token.Text = analytics.Token;
					cbx_Active.Checked = analytics.Active;
					cbx_ClientInfo.Checked = analytics.ClientInfo;
					cbx_Flash.Checked = analytics.DetectFlash;
					cbx_Hash.Checked = analytics.AllowHash;
					cbx_Linker.Checked = analytics.AllowLinker;
					cbx_Title.Checked = analytics.DetectTitle;
				}
			}
		}

		protected void btn_Google_Account_Profile_Click(object sender, EventArgs e)
		{
			foreach (GoogleAnalyticsSettings analytics in google.Settings.Analytics)
			{
                if (ddl_Google_Account_Profiles.SelectedValue == analytics.WebPropertyId)
				{

					analytics.Active = cbx_Active.Checked;
					analytics.ClientInfo = cbx_ClientInfo.Checked;
					analytics.DetectFlash = cbx_Flash.Checked;
					analytics.AllowHash = cbx_Hash.Checked;
					analytics.AllowLinker = cbx_Linker.Checked;
					analytics.DetectTitle = cbx_Title.Checked;

					google.Settings.Save();

					lbl_Profile_correct.Text = "Saved";
				}
			}
		}

		protected void btn_Google_Profiles_Reload_Click(object sender, EventArgs e)
		{
			ReloadAnalytics();
			ReloadWebmasterTools();

			foreach (GoogleAnalyticsSettings analytics in google.Settings.Analytics)
			{
				if (SPContext.Current.Site.HostName == analytics.WebsiteUrl)
				{
					tbx_Hostname.Text = analytics.Name;
					tbx_AccountId.Text = analytics.AccountId;
					//tbx_ProfileId.Text = analytics.ProfileId;
					//tbx_Token.Text = analytics.Token;
					cbx_Active.Checked = analytics.Active;
					cbx_ClientInfo.Checked = analytics.ClientInfo;
					cbx_Flash.Checked = analytics.DetectFlash;
					cbx_Hash.Checked = analytics.AllowHash;
					cbx_Linker.Checked = analytics.AllowLinker;
					cbx_Title.Checked = analytics.DetectTitle;
					ddl_Google_Account_Profiles.Items.Add(new ListItem(analytics.Name, analytics.WebPropertyId,true));
					ddl_Google_Account_Profiles.SelectedValue = analytics.WebPropertyId;
				}
				else
				{
                    ddl_Google_Account_Profiles.Items.Add(new ListItem(analytics.Name, analytics.WebPropertyId));
				}
			}

		}
	}
}
