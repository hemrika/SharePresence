namespace Hemrika.SharePresence.WebSite.Page
{
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.IO;
    using System.Security.Permissions;
    using Microsoft.SharePoint.Utilities;
    using System.Web;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageSubmitForApprovalHandler : SubmitForApprovalCommandHandler
    {
        public WebPageSubmitForApprovalHandler(SPPageStateControl psc) : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public override void RaisePostBackEvent(string eventArgument)
        {
            try
            {
                if (eventArgument == this.ClientSideCommandId)
                {
                    SPFile webPageFile = SPContext.Current.ListItem.File;
                    //PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                    WebPageStateControl parentStateControl = base.parentStateControl as WebPageStateControl;
                    if ((base.parentStateControl.EnsureItemSavedIfEditMode(false)))// && parentStateControl.EnsureSchedulingStateIsValid())
                    {
                        string directoryName = Path.GetDirectoryName(webPageFile.Url);
                        string str2 = directoryName;
                        string checkInComments = this.Page.Request.Form[SPPageStateControl.InputCommentsClientId];
                        /*
                        if (publishingPage.RequiresRouting)
                        {
                            publishingPage = PublishingPage.Route(publishingPage, checkInComments);
                            str2 = Path.GetDirectoryName(publishingPage.Url);
                        }
                        */
                        //SPFile file = publishingPage.ListItem.File;
                        if (webPageFile.CheckOutType != SPFile.SPCheckOutType.None)
                        {
                            webPageFile.CheckIn(checkInComments);
                            parentStateControl.OnPageStateChanged();
                            //ULS.SendTraceTag(0x38347468, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "Checked in current page: {0}", new object[] { file.ServerRelativeUrl });
                        }
                        Guid defaultContentApprovalWorkflowId = SPContext.Current.ListItem.ParentList.DefaultContentApprovalWorkflowId;
                        if (defaultContentApprovalWorkflowId != Guid.Empty)
                        {
                            //PublishAction.LaunchDefaultApprovalWorkflow(webPageFile.Item, defaultContentApprovalWorkflowId, webPageFile.Url.ToString());
                        }
                        else
                        {
                            if (webPageFile.Item.ParentList.EnableModeration)
                            {
                                webPageFile.Publish(checkInComments);
                                //ULS.SendTraceTag(0x38347469, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "Publishing current page: {0}", new object[] { file.ServerRelativeUrl });
                            }
                            string serverRelativeUrl = webPageFile.ServerRelativeUrl;
                            /*
                            if (directoryName != str2)
                            {
                                serverRelativeUrl = ConsoleUtilities.AddQueryStringParameterToUrl(serverRelativeUrl, "route", "1");
                            }
                            ConsoleUtilities.CleanRedirect(serverRelativeUrl, true, true);
                            */
                            SPUtility.Redirect(serverRelativeUrl, SPRedirectFlags.Trusted, HttpContext.Current);
                        }
                    }
                }
            }
            catch (SPException exception)
            {
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

