namespace Hemrika.SharePresence.WebSite.Page
{
    //using Microsoft.SharePoint.Publishing.Internal;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using Microsoft.SharePoint;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageUpdatePageStateHandler : UpdatePageStateCommandHandler
    {
        public WebPageUpdatePageStateHandler(SPPageStateControl psc) : base(psc)
        {
            base.waitScreenTitle = SPResource.GetString("SchedulingWaitScreenTitle");
            base.waitScreenText = SPResource.GetString("SchedulingWaitScreenText");
        }
    }
}

