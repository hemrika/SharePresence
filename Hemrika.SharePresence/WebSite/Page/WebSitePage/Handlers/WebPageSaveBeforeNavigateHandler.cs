// -----------------------------------------------------------------------
// <copyright file="WebPageSaveBeforeNavigateHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
    using System.Web.UI;
    using System.Text;

    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public sealed class WebPageSaveBeforeNavigateHandler : SPRibbonCommandHandler, ICallbackEventHandler
    {
        // Methods
        public WebPageSaveBeforeNavigateHandler(SPPageStateControl psc)
            : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        string ICallbackEventHandler.GetCallbackResult()
        {
            //this.ClientSideCommandId.ToString();
            if (this.Page is WebSitePage)
            {
                try
                {
                    base.parentStateControl.EnsureItemSavedIfEditMode(true);
                    //((WebSitePage)this.Page).ToString();//.sa.SaveBeforeNavigate();
                }
                catch (Exception exception)
                {
                    base.SetGenericErrorMessage(exception);
                }
            }
            else
            {
                base.parentStateControl.EnsureItemSavedIfEditMode(true);
            }
            base.parentStateControl.PopulateStates();
            return base.BuildReturnValue(SPResource.GetString("PageStateNotificationSave", new object[0]));
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        void ICallbackEventHandler.RaiseCallbackEvent(string inputEventArgument)
        {

        }

        // Properties
        public override string ClientSideCommandId
        {
            get
            {
                return "PageStateSaveBeforeNavigateAuto";
            }
        }

        public override string HandlerCommand
        {
            get
            {
                return (this.CallbackPreamble + this.Page.ClientScript.GetCallbackEventReference(this, "'" + this.ClientSideCommandId + "'", "SP.Ribbon.PageState.Handlers.GenericCompleteHandler", "SP.Ribbon.PageState.Handlers.GenericError", true));

                //StringBuilder builder = new StringBuilder();
                //builder.Append("__theFormPostData = \"\";__theFormPostCollection=new Array(); WebForm_OnSubmit();");
                //builder.Append("WebForm_InitCallback();_spResetFormOnSubmitCalledFlag();");
                //builder.Append(this.Page.ClientScript.GetCallbackEventReference(this, "'" + this.ClientSideCommandId + "'", "SP.Ribbon.PageState.PageStateHandler.SaveBeforeNavigateCallback", "SP.Ribbon.PageState.SaveBeforeNavigateCallback", true));
                //return builder.ToString();

            }
        }
    }
}
