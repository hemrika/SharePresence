namespace Hemrika.SharePresence.WebSite.Page
{
    //using Microsoft.Office.Server.Diagnostics;
    using Microsoft.SharePoint;
    //using Microsoft.SharePoint.Publishing.WebControls;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    public sealed class WebPageCheckoutHandler : CheckoutCommandHandler, IPostBackEventHandler
    {
        public WebPageCheckoutHandler(SPPageStateControl psc) : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == this.ClientSideCommandId)
            {
                SPListItem listItem = SPContext.Current.ListItem;
                try
                {
                    if ((listItem != null) && (listItem.File != null))
                    {
                        if (listItem.File.CheckedOutByUser == null ||listItem.File.CheckedOutByUser.ID != SPContext.Current.Web.CurrentUser.ID )
                        {
                            listItem.File.CheckOut();
                            if (!base.parentStateControl.EnsureItemSavedIfEditMode(false))
                            {
                                listItem.File.UndoCheckOut();
                            }
                            else
                            {
                                parentStateControl.OnPageStateChanged();
                                //base.parentStateControl.OnPageStateChanged();
                                string contextUri = base.parentStateControl.ContextUri;
                                if (SPContext.Current.FormContext.FormMode == SPControlMode.Edit)
                                {
                                    SPUtility.Redirect((contextUri), SPRedirectFlags.DoNotEncodeUrl | SPRedirectFlags.DoNotEndResponse | SPRedirectFlags.Trusted, HttpContext.Current);
                                }
                                else
                                {
                                    SPUtility.Redirect((contextUri), SPRedirectFlags.DoNotEncodeUrl | SPRedirectFlags.DoNotEndResponse | SPRedirectFlags.Trusted, HttpContext.Current);
                                }

                                    /*
                                else
                                {
                                    ConsoleUtilities.CleanRedirect(contextUri, false, true);
                                }
                                 */
                            }
                        }
                        else
                        {
                            base.SetGenericBlockingMessage(SPResource.GetString("PageStatePageCheckedOutBySomebodyElse", new object[0]));
                        }
                    }
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

