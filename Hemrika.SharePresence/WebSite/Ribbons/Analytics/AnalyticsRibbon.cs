// <copyright file="AnalyticsRibbon.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-03-22 15:41:28Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Security.Permissions;
    using System.Text;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.UI;
    using Hemrika.SharePresence.Common.Ribbon.Definitions;
    using Hemrika.SharePresence.Common.Ribbon.Libraries;
    using Hemrika.SharePresence.Common.Ribbon.Definitions.Controls;
    using Microsoft.SharePoint.WebPartPages;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using System.Linq;
    using Microsoft.SharePoint.Utilities;
    using Hemrika.SharePresence.Common.Ribbon;
    using Hemrika.SharePresence.WebSite.PageParts;
    using Microsoft.SharePoint.WebControls;
    using System.Web.UI;
    using System.Web.UI.WebControls.WebParts;
    using System.Web;
    using Hemrika.SharePresence.WebSite.ContentTypes;
    using Hemrika.SharePresence.Common;
    using Hemrika.SharePresence.Common.ServiceLocation;
    using Hemrika.SharePresence.WebSite.Fields;
    using Hemrika.SharePresence.WebSite.FieldTypes;
    using Hemrika.SharePresence.Google;

    /// <summary>
    /// TODO: Add comment for ContentRibbon
    /// </summary> 
    public class AnalyticsRibbon : SPRibbonCommandHandler, ICallbackEventHandler
    {
        GoogleSettings settings;

        public AnalyticsRibbon(SPPageStateControl psc)
            : base(psc)
        {
            settings = new GoogleSettings();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var tabDefinition = GetTabDefinition();
            try
            {
                if (SPRibbon.GetCurrent(this.Page) == null)
                    return;
                if (tabDefinition != null && !this.DesignMode)
                    RibbonController.Current.AddRibbonTabToPage(tabDefinition, this.Page, false);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private SPRibbon Ribbon
        {
            get
            {
                if (this.Page != null)
                {
                    return SPRibbon.GetCurrent(this.Page);
                }
                return null;
            }
        }

        public TabDefinition GetTabDefinition()
        {
            var groups = new List<GroupDefinition>();

            if (groups.Count > 0)
            {
                return new TabDefinition()
                {
                    Id = "Hemrika.SharePresence.Analytics",
                    Title = "Analytics",
                    Sequence = "140",
                    GroupTemplates = GroupTemplateLibrary.AllTemplates,
                    Groups = groups.ToArray()
                };
            }
            else
            {
                return null;
            }
        }

        public string GetCallbackResult()
        {
            try
            {
                if (result != null)
                {
                    throw (result);
                }
            }
            catch (SPException exception)
            {
                base.SetGenericErrorMessage(exception);
            }

            base.RefreshPageState();
            base.parentStateControl.OnPageStateChanged();

            SPRibbon ribbon = SPRibbon.GetCurrent(this.Page);

            if (ribbon != null)
            {
                string currentTab = ribbon.ActiveTabId;
                ribbon.SetInitialTabId(currentTab, "WSSPageStateVisibilityContext");
            }

            //UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url.OriginalString);
            //SPUtility.Redirect(builder.Uri.PathAndQuery, SPRedirectFlags.Trusted, HttpContext.Current);

            return base.BuildReturnValue(string.Format("{0} has been set for your page.",action));        

        }

        private Exception result = null;
        private String action = string.Empty;

        public void RaiseCallbackEvent(string eventArgument)
        {
            try
            {
                string[] eventArg = eventArgument.Split(new string[1] { "#;" }, StringSplitOptions.None);

                SPWeb rootWeb = SPContext.Current.Site.RootWeb;
                SPListItem item = SPContext.Current.ListItem;

                if (!string.IsNullOrEmpty(eventArg[0]))
                {
                }

                if (!string.IsNullOrEmpty(eventArg[1]))
                {
                }
            }
            catch (Exception ex)
            {
                result = ex;
            }
        }
    }
}

