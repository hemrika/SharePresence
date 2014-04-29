namespace Hemrika.SharePresence.WebSite.Page
{
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageCheckinHandler : CheckinCommandHandler
    {
        public WebPageCheckinHandler(SPPageStateControl psc) : base(psc)
        {
        }

        private void BaseRaisePostBackEvent(string eventArgument)
        {
            base.RaisePostBackEvent(eventArgument);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public override void RaisePostBackEvent(string eventArgument)
        {
            RaisePostBackEventDelegate raisePostBackEventDelegate = new RaisePostBackEventDelegate(this.BaseRaisePostBackEvent);
            (base.parentStateControl as WebPageStateControl).RaisePostBackEventForPageRouting(eventArgument, this, raisePostBackEventDelegate);
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

