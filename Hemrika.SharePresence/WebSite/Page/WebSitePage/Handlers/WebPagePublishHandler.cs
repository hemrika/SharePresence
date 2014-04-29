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
    public sealed class WebPagePublishHandler : PublishCommandHandler
    {
        public WebPagePublishHandler(SPPageStateControl psc) : base(psc)
        {
        }

        private void BaseRaisePostBackEvent(string eventArgument)
        {
            base.RaisePostBackEvent(eventArgument);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public override void RaisePostBackEvent(string eventArgument)
        {
            try
            {
                if (eventArgument == this.ClientSideCommandId)
                {
                    
                    //PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                    SPFile webPageFile = SPContext.Current.ListItem.File;
                    WebPageStateControl parentStateControl = base.parentStateControl as WebPageStateControl;
                    if ((base.parentStateControl.EnsureItemSavedIfEditMode(false)))// && parentStateControl.EnsureSchedulingStateIsValid())
                    {
                        string checkInComments = this.Page.Request.Form[SPPageStateControl.InputCommentsClientId];
                        string directoryName = Path.GetDirectoryName(webPageFile.Url);
                        string str3 = directoryName;
                        /*
                        if (publishingPage.RequiresRouting)
                        {
                            publishingPage = PublishingPage.Route(publishingPage, checkInComments);
                            str3 = Path.GetDirectoryName(publishingPage.Url);
                        }
                        */
                        //SPFile file = publishingPage.ListItem.File;
                        if (webPageFile.CheckOutType != SPFile.SPCheckOutType.None)
                        {
                            webPageFile.CheckIn(checkInComments,SPCheckinType.MajorCheckIn);
                            parentStateControl.OnPageStateChanged();
                            //ULS.SendTraceTag(0x38347465, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "Checked in current page: {0}", new object[] { file.ServerRelativeUrl });
                        }
                        Guid defaultContentApprovalWorkflowId = SPContext.Current.ListItem.ParentList.DefaultContentApprovalWorkflowId;
                        if (defaultContentApprovalWorkflowId != Guid.Empty)
                        {
                            //PublishAction.LaunchDefaultApprovalWorkflow(publishingPage.ListItem, defaultContentApprovalWorkflowId, publishingPage.Uri.ToString());
                        }
                        else
                        {
                            if (SPContext.Current.ListItem.ParentList.EnableMinorVersions)
                            {
                                webPageFile.Publish(checkInComments);
                            }
                            if (SPContext.Current.ListItem.ParentList.EnableModeration)
                            {
                                webPageFile.Approve(checkInComments);
                                //ULS.SendTraceTag(0x38347467, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Medium, "Approved current page: {0}", new object[] { file.ServerRelativeUrl });
                            }

                            RaisePostBackEventDelegate raisePostBackEventDelegate = new RaisePostBackEventDelegate(this.BaseRaisePostBackEvent);
                            (base.parentStateControl as WebPageStateControl).RaisePostBackEventForPageRouting(eventArgument, this, raisePostBackEventDelegate);

                            //string serverRelativeUrl = webPageFile.ServerRelativeUrl;
                            //SPUtility.Redirect(serverRelativeUrl, SPRedirectFlags.Trusted, HttpContext.Current);
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

