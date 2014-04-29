namespace Hemrika.SharePresence.WebSite.Page
{
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageDontSaveAndStopEditHandler : DontSaveAndStopCommandHandler
    {
        public WebPageDontSaveAndStopEditHandler(SPPageStateControl psc) : base(psc)
        {
        }

        public override string HandlerCommand
        {
            get
            {
                return base.DefaultPostbackHandlerCommand;//, "SP.Publishing.Resources.notificationMessageReverting");
            }
        }
    }
}

