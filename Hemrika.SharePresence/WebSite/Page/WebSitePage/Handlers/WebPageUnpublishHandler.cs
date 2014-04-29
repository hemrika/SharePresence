namespace Hemrika.SharePresence.WebSite.Page
{
    //using Microsoft.Office.Server.Diagnostics;
    using Microsoft.SharePoint;
    //using Microsoft.SharePoint.Publishing;
    //using Microsoft.SharePoint.Publishing.WebControls;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using System.Web.UI;
    using Microsoft.SharePoint.Utilities;
    using System.Web;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageUnpublishHandler : UnpublishCommandHandler, IPostBackEventHandler
    {
        public WebPageUnpublishHandler(SPPageStateControl psc) : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public override void RaiseCallbackEvent(string evtHandler)
        {
        }

        private void BaseRaiseCallbackEvent(string eventArgument)
        {
            base.RaiseCallbackEvent(eventArgument);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == this.ClientSideCommandId)
            {/*
                SPListItem contextualListItem = null;// ConsoleUtilities.ContextualListItem;
                //CachedPage pageForCurrentItem = CacheManager.GetManagerForContextSite().ObjectFactory.GetPageForCurrentItem();
                if ((pageForCurrentItem != null) && pageForCurrentItem.IsLocked)
                {
                    SPSite siteNonElev = SPContext.Current.Site;
                    SPWeb webNonElev = SPContext.Current.Web;
                    string listItemUrl = contextualListItem.Url;
                    CommonUtilities.RunWithElevatedSiteAndWebEx(siteNonElev, webNonElev, true, false, delegate (SPSite siteElev, SPWeb webElev, bool isNewUserToken) {
                        CommonUtilities.BypassLocks(CommonUtilities.GetListItemFromWeb(webElev, listItemUrl), new BypassLockedItemDelegateMethod(this.Unpublish));
                    });
                }
                else
                {
              */
                    this.Unpublish(SPContext.Current.ListItem);

                    RaisePostBackEventDelegate raisePostBackEventDelegate = new RaisePostBackEventDelegate(this.BaseRaiseCallbackEvent);
                    (base.parentStateControl as WebPageStateControl).RaisePostBackEventForPageRouting(eventArgument, this, raisePostBackEventDelegate);

              //  }
            }
        }

        private void Unpublish(SPListItem listItem)
        {
            try
            {
                if ((listItem != null) && (listItem.File != null))
                {
                    if (listItem.ParentList.EnableModeration)
                    {
                        listItem.File.UnPublish(string.Empty);
                    }

                    listItem.File.TakeOffline();

                    /*
                    SPRedirectFlags flags = (SPRedirectFlags.Default | SPRedirectFlags.DoNotEndResponse |SPRedirectFlags.DoNotEncodeUrl);

                    SPUtility.Redirect(listItem.File.ServerRelativeUrl, flags, HttpContext.Current);
                    */
                }
            }
            catch (SPException exception)
            {
                //ULS.SendTraceTag(0x656c6135, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Unexpected, "Failed to unpublish page ({0}) : {1}", new object[] { base.parentStateControl.ContextUri, exception.Message });
                base.SetGenericErrorMessage(exception);
            }
        }

        public override string HandlerCommand
        {
            get
            {
                return base.DefaultPostbackHandlerCommand;
            }
        }
    }
}

