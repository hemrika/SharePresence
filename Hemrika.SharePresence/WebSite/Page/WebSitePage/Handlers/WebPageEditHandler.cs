namespace Hemrika.SharePresence.WebSite.Page
{
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI.WebControls.WebParts;
    using System.Globalization;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust"), PermissionSet(SecurityAction.InheritanceDemand, Name="FullTrust")]
    public sealed class WebPageEditHandler : EditCommandHandler
    {
        public WebPageEditHandler(SPPageStateControl psc) : base(psc)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override void RaisePostBackEvent(string eventArgument)
        {
            SPList contextualList = ContextualList;
            if ((contextualList != null) && contextualList.ForceCheckout)
            {
                SPFile contextualFile = ContextualFile;
                if (((contextualFile != null) && (WebPartManager.GetCurrentWebPartManager(this.Page).Personalization.Scope == PersonalizationScope.Shared)) && (contextualFile.CheckOutType == SPFile.SPCheckOutType.None))
                {
                    contextualFile.CheckOut();
                    contextualFile.Update();
                }
                base.parentStateControl.OnPageStateChanged();

                UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url.OriginalString);
                builder.Query = string.Empty;
                builder.Query = "ControlMode=Edit&DisplayMode=Design";
                //return AddQueryStringParameterToUrl(AddQueryStringParameterToUrl(url, "ControlMode", "Edit"), "DisplayMode", "Design");
                SPUtility.Redirect(builder.Uri.PathAndQuery, SPRedirectFlags.Trusted, HttpContext.Current);
            }
            
            SPContext.Current.FormContext.FormMode = SPControlMode.Edit;
            base.parentStateControl.OnPageStateChanged();
            base.RaisePostBackEvent(eventArgument);
        }

        public override string HandlerCommand
        {
            get
            {
                return base.HandlerCommand;//, "SP.Publishing.Resources.notificationMessageLoading");
            }
        }

        public static SPFile ContextualFile
        {
            get
            {
                try
                {
                    if (ContextualListItem != null)
                    {
                        return SPContext.Current.File;
                    }
                    return null;
                }
                catch (SPException)
                {
                    return null;
                }
            }
        }

        internal static SPList ContextualList
        {
            get
            {
                return SPContext.GetContext(HttpContext.Current).List;
            }
        }

        internal static SPListItem ContextualListItem
        {
            get
            {
                try
                {
                    SPListItem listItem = SPContext.Current.ListItem;
                    if (listItem != null)
                    {
                        object obj2;
                        int iD;
                        if (SPContext.Current.GetValueFromPageData("ID", out obj2))
                        {
                            iD = Convert.ToInt32(obj2, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            iD = listItem.ID;
                        }
                        if (iD == 0)
                        {
                            return null;
                        }
                    }
                    return listItem;
                }
                catch (SPException)
                {
                    return null;
                }
            }
        }

    }
}

