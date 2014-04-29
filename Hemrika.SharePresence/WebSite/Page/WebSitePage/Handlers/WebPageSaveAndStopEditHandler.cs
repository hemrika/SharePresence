namespace Hemrika.SharePresence.WebSite.Page
{
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using System.Text;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageSaveAndStopEditHandler : SaveAndStopEditCommandHandler
    {
        public WebPageSaveAndStopEditHandler(SPPageStateControl psc) : base(psc)
        {
        }

        public override string ClientSideCommandId
        {
            get
            {
                return "PageStateGroupSaveAndStop";
            }
        }

        public new string ForceSaveCommand
        {
            get
            {
                return this.Page.ClientScript.GetPostBackEventReference(this, "forceSave");
            }
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
                StringBuilder builder = new StringBuilder();
                builder.Append("SP.Ribbon.PageState.PageStateHandler.ignoreNextUnload = false;");
                builder.Append(base.DefaultPostbackHandlerCommand);
                return builder.ToString();

                //return base.DefaultPostbackHandlerCommand;//, "SP.Publishing.Resources.notificationMessageSaving");
            }
        }

        public override string IsEnabledHandler
        {
            get
            {
                return "SP.Ribbon.PageState.Handlers.isSaveAndStopEditEnabled();";
            }
        }

    }
}

