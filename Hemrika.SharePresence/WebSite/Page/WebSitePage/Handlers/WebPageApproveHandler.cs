namespace Hemrika.SharePresence.WebSite.Page
{
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using Microsoft.SharePoint.Utilities;
    using System.Web;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageApproveHandler : ApproveCommandHandler
    {
        public WebPageApproveHandler(SPPageStateControl psc) : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public override void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == this.ClientSideCommandId)
            {
                WebPageStateControl parentStateControl = base.parentStateControl as WebPageStateControl;
                //if (parentStateControl.EnsureSchedulingStateIsValid())
                //{
                    if (base.WorkflowPostbackCommand != null)
                    {
                        base.RaisePostBackEvent(eventArgument);
                    }
                    else
                    {
                        base.ApproveFileWithComments();
                        SPUtility.Redirect(base.parentStateControl.ContextUri, SPRedirectFlags.Trusted, HttpContext.Current);
                    }
                //}
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

