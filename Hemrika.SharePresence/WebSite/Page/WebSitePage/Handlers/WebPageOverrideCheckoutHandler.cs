namespace Hemrika.SharePresence.WebSite.Page
{
    //using Microsoft.Office.Server.Diagnostics;
    using Microsoft.SharePoint;
    //using Microsoft.SharePoint.Publishing.WebControls;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI;
    using Microsoft.SharePoint.Utilities;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageOverrideCheckoutHandler : OverrideCheckoutCommandHandler, IPostBackEventHandler
    {
        public WebPageOverrideCheckoutHandler(SPPageStateControl psc) : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == this.ClientSideCommandId)
            {
                try
                {
                    //if (!ConsoleContext.VersionConflictExists)
                    //{
                        SPFile contextualFile = SPContext.Current.File;
                        contextualFile.UndoCheckOut();
                        contextualFile.CheckOut();
                        //ULS.SendTraceTag(0x66386b74, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "Console: Successful override of checkout on current page: {0}", new object[] { HttpContext.Current.Request.Url.AbsoluteUri });
                        base.parentStateControl.OnPageStateChanged();
                        UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url.AbsoluteUri);
                        builder.Query = string.Empty;
                        SPUtility.Redirect(builder.Uri.PathAndQuery, SPRedirectFlags.Trusted, HttpContext.Current);
                        //ConsoleUtilities.CleanRedirect(HttpContext.Current.Request.Url.AbsoluteUri, false, true);
                    //}
                    /*
                    else
                    {
                        ULS.SendTraceTag(0x66386b75, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Verbose, "Console: Current checkout not overridden due to version conflict: {0}", new object[] { HttpContext.Current.Request.Url.AbsoluteUri });
                        base.SetGenericErrorMessage(SPResource.GetString("PageStatePageOverrideCheckoutVersionConflict", new object[0]));
                    }
                    */
                }
                catch (SPException exception)
                {
                    base.SetGenericErrorMessage(exception);
                }
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

