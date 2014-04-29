using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using System.Web;
using System.Collections;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Workflow;
using Hemrika.SharePresence.WebSite.Page.Handlers;
using Hemrika.SharePresence.WebSite.Page;

namespace Hemrika.SharePresence.WebSite.Page
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class WebPageStateControl : SPPageStateControl
    {
        private const string DismissErrorDialogJavascript = "SP.Ribbon.PageState.PageStateHandler.dismissErrorDialog();";
        private const string IndicatorTitleResourceFile = "cms";
        private const string ItemIsLockedStateName = "ItemIsLocked";
        private const string PageIsAwaitingApprovalFromCurrentUserStateName = "ItemIsAwaitingApprovalFromCurrentUser";
        private const string PageIsExpiredStateName = "ItemIsExpired";
        private const string PageIsPublishingPageStateName = "ItemIsPublishingPage";
        private static StateFlagToStateStringEntry[] statesToStringArray = new StateFlagToStateStringEntry[] { 
            new StateFlagToStateStringEntry(AuthoringStates.IsPublishingPageTrue, "ItemIsPublishingPage"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.PageIsExpiredTrue, "ItemIsExpired"), new StateFlagToStateStringEntry(AuthoringStates.CheckedInVersionExistsTrue, "ItemHasCheckedInVersion"), new StateFlagToStateStringEntry(AuthoringStates.IsMasterPageGalleryFileTrue, "ItemIsMasterPageGalleryFile"), new StateFlagToStateStringEntry(AuthoringStates.InEditModeTrue, "ViewModeIsEdit"), new StateFlagToStateStringEntry(AuthoringStates.IsDocLibListItemTrue, "ItemIsInLibrary"), new StateFlagToStateStringEntry(AuthoringStates.IsCheckedOutToCurrentUserTrue, "ItemIsCheckedOutToCurrentUser"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserTrue, "ItemIsCheckedOutToOtherUser"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.IsPublishingSiteTrue, "WebIsPublishingSite"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.InSharedView, "ItemIsInSharedView"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.InPersonalView, "ItemIsInPersonalView"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionTrue, "ItemIsMajorVersion"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionTrue, "ItemIsMinorVersion"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.IsScheduledStatusTrue, "ItemIsScheduled"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.IsPendingApprovalTrue, "ItemIsPendingApproval"), new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.PageHasCustomizableZonesTrue, "ItemHasCustomizableZones"), 
            new StateFlagToStateStringEntry(AuthoringStates.EmptyMask | AuthoringStates.PageHasPersonalizableZonesTrue, "ItemHasPersonalizableZones"), new StateFlagToStateStringEntry(AuthoringStates.IsApprovalWorkflowCancelEnabledTrue, "ItemCancelWorkflowEnabled"), new StateFlagToStateStringEntry(AuthoringStates.IsItemWaitingForApprovalTrue, "ItemIsPendingApproval"), new StateFlagToStateStringEntry(AuthoringStates.IsDefaultPageTrue, "ItemIsDefaultPage"), new StateFlagToStateStringEntry(AuthoringStates.SaveConflictExistsTrue, "ViewModeHasSaveConflict"), new StateFlagToStateStringEntry(AuthoringStates.InWebPartDesignModeTrue, "ViewModeIsWebPartDesign"), new StateFlagToStateStringEntry(AuthoringStates.MinorVersionsEnabledTrue, "DocLibMinorVersionsEnabled"), new StateFlagToStateStringEntry(AuthoringStates.CheckOutRequiredTrue, "DocLibCheckoutRequired"), new StateFlagToStateStringEntry(AuthoringStates.ContentApprovalEnabledTrue, "DocLibApprovalEnabled"), new StateFlagToStateStringEntry(AuthoringStates.IsFormPageTrue, "ItemIsFormsPage"), new StateFlagToStateStringEntry(AuthoringStates.PageHasFieldControlsTrue, "ItemHasFieldControls"), new StateFlagToStateStringEntry(AuthoringStates.IsApprovalWorkflowTaskActiveForUserTrue, "ItemIsAwaitingApprovalFromCurrentUser"), new StateFlagToStateStringEntry(AuthoringStates.IsApprovalWorkflowConfiguredTrue, "DocLibWorkflowEnabled")
         };
        private const string UserHasEnumeratePermissionsRightsStateName = "UserHasEnumeratePermissionsRights";
        private const string UserHasViewVersionsRightsStateName = "UserHasViewVersionsRights";
        private const string WebIsPublishingSiteStateName = "WebIsPublishingSite";

        /*
        internal bool CheckMissingRequiredFields()
        {
            bool flag = false;
            string editPropertiesURLIfAnyMissingRequiredFields = SPPageStateControl.GetEditPropertiesURLIfAnyMissingRequiredFields(false);
            if (editPropertiesURLIfAnyMissingRequiredFields != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("SP.Utilities.HttpUtility.navigateTo('");
                builder.Append(HttpEncodingUtility.UrlPathEncode(editPropertiesURLIfAnyMissingRequiredFields));
                builder.Append("');");
                base.SetErrorCondition(SPResource.GetString("MissingRequiredFieldsErrorMessage"), 2, new string[] { SPResource.GetString("PageStateOkButton", new object[0]), SPResource.GetString("ButtonTextCancel", new object[0]) }, new string[] { builder.ToString(), "SP.Ribbon.PageState.PageStateHandler.dismissErrorDialog();" });
                flag = true;
            }
            return flag;
        }
        */

        private void DetermineInitialTab()
        {
            SPRibbon current = SPRibbon.GetCurrent(this.Page);
            if ((current != null))
            {
                if (!Page.Request.QueryString.AllKeys.Contains("InitialTabId"))
                {
                    current.SetInitialTabId("Ribbon.Hemrika.SharePresence.Page", "WSSPageStateVisibilityContext");
                }
                else
                {
                    string initialTab = Page.Request.QueryString["InitialTabId"];
                    string activeTab = current.ActiveTabId;
                    if(initialTab != activeTab)
                    {
                        if (String.IsNullOrEmpty(activeTab))
                        {
                            current.SetInitialTabId("Ribbon.Hemrika.SharePresence.Page", "WSSPageStateVisibilityContext");
                        }
                        else
                        {
                            current.SetInitialTabId(activeTab, "WSSPageStateVisibilityContext");
                        }
                    }
                }

                current.Minimized = false;
                current.CommandUIVisible = true;
                current.EnableVisibilityContext("WSSPageStateVisibilityContext");
                current.ServerRendered = true;
            }
        }

        /*
        internal bool EnsureSchedulingStateIsValid()
        {
            string isSchedulingStateValidMessage = ConsoleUtilities.GetIsSchedulingStateValidMessage();
            bool flag = true;
            if (!string.IsNullOrEmpty(isSchedulingStateValidMessage))
            {
                base.SetErrorCondition(isSchedulingStateValidMessage, 1, new string[] { SPResource.GetString("PageStateOkButton", new object[0]) }, new string[] { "SP.Ribbon.PageState.PageStateHandler.dismissErrorDialog();" });
                flag = false;
            }
            return flag;
        }
        */

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public static WebPageStateControl GetCurrentWebPageStateControl(System.Web.UI.Page page)
        {
            if (page == null)
            {
                return null;
            }
            return (page.Items[typeof(WebPageStateControl)] as WebPageStateControl);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.Items[typeof(WebPageStateControl)] = this;
            base.RibbonTrimmer = new WebSiteRibbonTrimmer(this.Page, SPRibbon.GetCurrent(this.Page));
        }

        public override void OnPageStateChanged()
        {           

            HttpContext current = HttpContext.Current;

            if (current != null)
            {
                
                if (SPContext.Current != null)
                {
                    SPContext.Current.ResetItem();
                }

                current.Items["currentAuthoringStates"] = null;
                if (current.Items.Contains("CachedObjectWrapper_checkedOutVersion"))
                {
                    current.Items.Remove("CachedObjectWrapper_checkedOutVersion");
                }
                if (current.Items.Contains("currentVisibilityStates"))
                {
                    current.Items.Remove("currentVisibilityStates");
                }
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Visible)
            {
                if (!this.Page.IsPostBack)
                {
                    /*
                    string str = HttpContext.Current.Request.QueryString.Get("AuthoringError");
                    if (!string.IsNullOrEmpty(str) && (string.CompareOrdinal(str, "EditModeLockedByWorkflow") == 0))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(ConsoleUtilities.SetFormFieldInJavascript(this.Page.Form.Name, "MSOAC_EditDuringWorkflow", "1"));
                        builder.Append(base.commandHandlers[3].HandlerCommand);
                        StringBuilder builder2 = new StringBuilder();
                        builder2.Append("SP.Utilities.HttpUtility.navigateTo('");
                        builder2.Append(ConsoleUtilities.RemoveAuthoringErrors(base.ContextUri));
                        builder2.Append("');");
                        base.SetErrorCondition(Resources.GetString("PageStateLockedByWorkflowErrorMessage"), 2, new string[] { SPResource.GetString("PageStateOkButton", new object[0]), SPResource.GetString("ButtonTextCancel", new object[0]) }, new string[] { builder.ToString(), builder2.ToString() });
                    }
                    */
                }
                base.OnPreRender(e);
                this.DetermineInitialTab();
            }
        }

        protected override void PopulateCommandHandlers()
        {
            base.PopulateCommandHandlers();

            base.commandHandlers[0] = new WebPageSaveHandler(this);
            //base.commandHandlers[1] = new WebPageSaveBeforeNavigateHandler(this);
            base.commandHandlers[2] = new WebPageSaveAndStopEditHandler(this);
            base.commandHandlers[3] = new WebPageEditHandler(this);
            base.commandHandlers[4] = new WebPageDontSaveAndStopEditHandler(this);
            base.commandHandlers[5] = new WebPageCheckinHandler(this);
            base.commandHandlers[6] = new WebPageCheckoutHandler(this);
            //base.commandHandlers[7] = new WebPageOverrideCheckoutHandler(this);
            base.commandHandlers[8] = new WebPageDiscardCheckoutHandler(this);
            base.commandHandlers[9] = new WebPageSubmitForApprovalHandler(this);
            base.commandHandlers[11] = new WebPagePublishHandler(this);
            base.commandHandlers[12] = new WebPageUnpublishHandler(this);
            base.commandHandlers[13] = new WebPageApproveHandler(this);
            
            base.commandHandlers[16] = new WebPageUpdatePageStateHandler(this);
        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        internal CombinedBasePermissions CurrentPermissions()
        {
            HttpContext current = HttpContext.Current;
            if (current.Items["currentPermissions"] != null)
            {
                return (CombinedBasePermissions)current.Items["currentPermissions"];
            }
            CombinedBasePermissions permissions = new CombinedBasePermissions();
            current.Items["currentPermissions"] = permissions;
            return permissions;
        }

        public static bool CheckPermissions(SPBasePermissions permission)
        {
            return (((SPContext.Current.ContextPageInfo != null) && SPContext.Current.ContextPageInfo.BasePermissions.HasValue) && ((((SPBasePermissions)SPContext.Current.ContextPageInfo.BasePermissions.Value) & permission) == permission));
        }

        public int CheckedOutByUserId
        {
            get
            {
                if (SPContext.Current.ListItem.Fields.Contains(SPBuiltInFieldId.CheckoutUser))
                {
                    string fieldValue = SPContext.Current.ListItem[SPBuiltInFieldId.CheckoutUser] as string;
                    if (fieldValue != null)
                    {
                        SPFieldLookupValue value2 = new SPFieldLookupValue(fieldValue);
                        if (value2 != null)
                        {
                            return value2.LookupId;
                        }
                    }
                }
                return 0;
            }
        }

        public override void PopulateStates()
        {

            AuthoringStates states = CurrentState(this.Page);
            for (int i = 0; i < statesToStringArray.Length; i++)
            {
                StateFlagToStateStringEntry entry = statesToStringArray[i];
                if ((states & entry.stateFlag) != AuthoringStates.EmptyMask)
                {
                    base.currentState[entry.stateString] = "1";
                }
            }

            CombinedBasePermissions permissions = CurrentPermissions();

            if ((permissions.ListItemPermissions & SPBasePermissions.EditListItems) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasContributorRights"] = "1";
            }
            if ((permissions.ListItemPermissions & (SPBasePermissions.EmptyMask | SPBasePermissions.CancelCheckout)) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasOverrideCheckoutRights"] = "1";
            }
            if ((permissions.ListItemPermissions & SPBasePermissions.ApproveItems) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasApproverRights"] = "1";
            }
            if ((permissions.ListItemPermissions & SPBasePermissions.DeleteListItems) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasDeleteRights"] = "1";
            }
            if ((permissions.SitePermissions & SPBasePermissions.ManageWeb) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasManageWebRights"] = "1";
            }

            if ((SPContext.Current.List != null) && SPContext.Current.List.EnableVersioning)
            {
                base.currentState["DocLibVersioningEnabled"] = "1";
            }
            if ((permissions.ListItemPermissions & SPBasePermissions.ViewVersions) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasViewVersionsRights"] = "1";
            }
            if ((permissions.ListItemPermissions & SPBasePermissions.EnumeratePermissions) != SPBasePermissions.EmptyMask)
            {
                base.currentState["UserHasEnumeratePermissionsRights"] = "1";
            }
            if (CheckPermissions(SPBasePermissions.EditListItems))
            {

                if ((SPContext.Current.ListItem != null) && (CheckedOutByUserId == 0x3fffffff))
                {
                    base.currentState["ItemIsCheckedOutToSystemUser"] = "1";
                }
            }
            /*
            CachedPage pageForCurrentItem = CacheManager.GetManagerForContextSite().ObjectFactory.GetPageForCurrentItem();
            if ((pageForCurrentItem != null) && pageForCurrentItem.IsLocked)
            {
                base.currentState["ItemIsLocked"] = "1";
            }
            */
        }

        private AuthoringStates CurrentState(System.Web.UI.Page page)
        {
            HttpContext current = HttpContext.Current;
            if (current.Items["currentAuthoringStates"] != null)
            {
                return (AuthoringStates)current.Items["currentAuthoringStates"];
            }

            SPContext context = SPContext.Current;
            //SPControlMode formContextMode = ConsoleUtilities.FormContextMode;
            //SPControlMode contextualControlMode = ConsoleUtilities.GetContextualControlMode(currentPage);
            SPWeb web = context.Web;
            SPList list = context.List;
            SPUser currentUser = web.CurrentUser;
            CombinedBasePermissions permissions = CurrentPermissions();
            AuthoringStates emptyMask = AuthoringStates.EmptyMask;
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(Page);
            
            if ((permissions.ListItemPermissions & SPBasePermissions.EditListItems) == SPBasePermissions.EditListItems)
            {

                if ((SPContext.Current.ListItem != null) && SPContext.Current.ListItem.File.CheckOutType != SPFile.SPCheckOutType.None)
                {
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.CheckedOutVersionExistsTrue;
                    try
                    {
                        if ((currentUser == null) || (CheckedOutByUserId != currentUser.ID))
                        {
                            emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserTrue;
                        }
                    }
                    catch (SPException)
                    {
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserTrue;
                    }
                }
                else
                {
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.CheckedOutVersionExistsFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserFalse;
                }
            }

            if (((SPContext.Current.ListItem != null) && ((permissions.ListItemPermissions & SPBasePermissions.EditListItems) == SPBasePermissions.EditListItems)) && ((permissions.ListItemPermissions & SPBasePermissions.ViewVersions) == SPBasePermissions.ViewVersions))
            {
                if (SPContext.Current.ListItem != null)
                {
                    if(SPContext.Current.ListItem.HasPublishedVersion)
                    {
                        emptyMask |= AuthoringStates.CheckedInVersionExistsTrue;
                    }
                    else
                    {
                        emptyMask |= AuthoringStates.CheckedInVersionExistsFalse;
                    }
                }
            }

            bool flag = false;
            if ((((permissions.ListItemPermissions & SPBasePermissions.EditListItems) == SPBasePermissions.EditListItems) || (SPContext.Current.FileLevel == SPFileLevel.Checkout)) || (((list != null) && list.DoesUserHavePermissions(SPBasePermissions.EditListItems)) && list.DoesUserHavePermissions(SPBasePermissions.EmptyMask | SPBasePermissions.ManageLists)))
            {
                if (SPContext.Current.ListItem != null)
                {
                    if ((SPContext.Current.ListItem != null) && WorkflowUtilities.IsApprovalWorkflowTaskActiveForUser(SPContext.Current.ListItem))
                    {
                        emptyMask |= AuthoringStates.IsApprovalWorkflowTaskActiveForUserTrue;
                    }
                    if (WorkflowUtilities.GetCurrentUserWorkflowTasks(SPContext.Current.ListItem).Count > 0)
                    {
                        emptyMask |= AuthoringStates.UserWorkflowTaskExistsTrue;
                    }
                    if (SPContext.Current.ListItem.Workflows.Count > 0)
                    {
                        emptyMask |= AuthoringStates.ActiveWorkflowsExistTrue;
                    }
                    else
                    {
                        emptyMask |= AuthoringStates.ActiveWorkflowsExistFalse;
                    }

                    if (WorkflowUtilities.IsDefaultApprovalWorkflowConfigured(SPContext.Current.ListItem))
                    {
                        emptyMask |= AuthoringStates.IsApprovalWorkflowConfiguredTrue;
                    }
                    else
                    {
                        emptyMask |= AuthoringStates.IsApprovalWorkflowConfiguredFalse;
                    }

                    SPWorkflow runningDefaultApprovalWorkflow = WorkflowUtilities.GetRunningDefaultApprovalWorkflow(SPContext.Current.ListItem);
                    if (runningDefaultApprovalWorkflow != null)
                    {
                        flag = true;
                    }
                    else
                    {
                        emptyMask |= AuthoringStates.IsApprovalWorkflowRunningFalse;
                        flag = false;
                    }

                    if ((((runningDefaultApprovalWorkflow != null) && (list != null)) && ((currentUser != null) && list.DoesUserHavePermissions(SPBasePermissions.EditListItems))) && (list.DoesUserHavePermissions(SPBasePermissions.EmptyMask | SPBasePermissions.ManageLists) || (runningDefaultApprovalWorkflow.Author == currentUser.ID)))
                    {
                        emptyMask |= AuthoringStates.IsApprovalWorkflowCancelEnabledTrue;
                    }

                    bool flag2 = false;
                    if (((SPContext.Current.ListItem != null) && SPContext.Current.ListItem.ModerationInformation != null) && (SPContext.Current.ListItem.ModerationInformation.Status == SPModerationStatusType.Pending))
                    {
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsPendingApprovalTrue;
                        flag2 = true;
                    }
                    else
                    {
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsPendingApprovalFalse;
                        flag2 = false;
                    }
                    if (flag || flag2)
                    {
                        emptyMask |= AuthoringStates.IsItemWaitingForApprovalTrue;
                    }

                    if (SPContext.Current.FormContext.FormMode == SPControlMode.Edit)
                    {
                        emptyMask |= AuthoringStates.InEditModeTrue;
                        if (page.IsPostBack)// && (ConsoleContext.AuthoringItemVersion != ConsoleContext.CurrentItemVersion))
                        {
                            emptyMask |= AuthoringStates.SaveConflictExistsTrue;
                        }
                        else
                        {
                            emptyMask |= AuthoringStates.SaveConflictExistsFalse;
                        }
                    }
                    else
                    {
                        emptyMask |= AuthoringStates.InEditModeFalse;
                        emptyMask |= AuthoringStates.SaveConflictExistsFalse;
                    }
                    /*
                    try
                    {
                        if ((page != null) && (page.EndDate < ConsoleUtilities.CorrectDate(DateTime.Now)))
                        {
                            emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageIsExpiredTrue;
                        }
                        else
                        {
                            emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageIsExpiredFalse;
                        }
                    }
                    catch (ArgumentException)
                    {
                        ULS.SendTraceTag(0x636c6d37, ULSCat.msoulscat_CMS_Publishing, ULSTraceLevel.Verbose, "ConsoleNode: Failed to acquire end date of page {0}", new object[] { currentPage.Request.Url });
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageIsExpiredFalse;
                    }
                    */


                    if (!SPContext.Current.ListItem.File.Exists)
                    {
                        emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionFalse;
                    }
                    else if (SPContext.Current.ListItem.File.Level == SPFileLevel.Checkout)
                    {
                        emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserTrue;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionFalse;
                    }
                    else if (SPContext.Current.ListItem.File.Level == SPFileLevel.Draft)
                    {
                        emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionTrue;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionFalse;
                    }
                    else if (SPContext.Current.ListItem.File.Level == SPFileLevel.Published)
                    {
                        emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserFalse;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionTrue;
                        emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionFalse;
                    }
                }
                else
                {
                    emptyMask |= AuthoringStates.IsApprovalWorkflowTaskActiveForUserFalse;
                    emptyMask |= AuthoringStates.ActiveWorkflowsExistFalse;
                    emptyMask |= AuthoringStates.IsApprovalWorkflowConfiguredFalse;
                    emptyMask |= AuthoringStates.IsApprovalWorkflowRunningFalse;
                    emptyMask |= AuthoringStates.InEditModeFalse;
                    emptyMask |= AuthoringStates.SaveConflictExistsFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsPendingApprovalFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageIsExpiredFalse;
                }
            }
            else
            {
                emptyMask |= AuthoringStates.IsApprovalWorkflowTaskActiveForUserFalse;
                emptyMask |= AuthoringStates.ActiveWorkflowsExistFalse;
                emptyMask |= AuthoringStates.IsApprovalWorkflowConfiguredFalse;
                emptyMask |= AuthoringStates.IsApprovalWorkflowRunningFalse;
                emptyMask |= AuthoringStates.InEditModeFalse;
                emptyMask |= AuthoringStates.SaveConflictExistsFalse;
                emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsPendingApprovalFalse;
                emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageIsExpiredFalse;
                if (!SPContext.RenderingFromCurrentMetainfo && ((SPContext.Current.ItemId == 0) || (SPContext.Current.File == null)))
                {
                    emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionFalse;
                }
                else if (SPContext.Current.FileLevel == SPFileLevel.Checkout)
                {
                    emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserTrue;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsCheckedOutToOtherUserFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionFalse;
                }
                else if (SPContext.Current.FileLevel == SPFileLevel.Draft)
                {
                    emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionTrue;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionFalse;
                }
                else if (SPContext.Current.FileLevel == SPFileLevel.Published)
                {
                    emptyMask |= AuthoringStates.IsCheckedOutToCurrentUserFalse;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMajorVersionTrue;
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsMinorVersionFalse;
                }
            }

            if (SPContext.Current.FormContext.FormMode == SPControlMode.Edit)
            {
                emptyMask |= AuthoringStates.InWebPartDesignModeTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.InWebPartDesignModeFalse;
            }

            //if (ConsoleVisibleUtilities.GetConsoleFullyVisible())
            //{
            emptyMask |= AuthoringStates.EditingMenuEnabled;
            //}
            /*
            else
            {
                emptyMask |= AuthoringStates.EditingMenuDisabled;
            }
            */

            if ((SPContext.Current.ItemId != 0) && (SPContext.RenderingFromCurrentMetainfo || (SPContext.Current.File != null)))
            {
                emptyMask |= AuthoringStates.IsDocLibListItemTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.IsDocLibListItemFalse;
            }

            /*
            if (ConsoleUtilities.IsFormPage(currentPage))
            {
                emptyMask |= AuthoringStates.IsFormPageTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.IsFormPageFalse;
            }
            */

            //if (PublishingWeb.IsPublishingWeb(web))
            //{
            emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsPublishingSiteTrue;
            //}
            /*
            else
            {
                emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.IsPublishingSiteFalse;
            }
            */

            //if ((SPContext.Current.List != null) && PublishingPage.IsPagesLibrary(SPContext.Current.List))
            //{
            emptyMask |= AuthoringStates.IsPublishingPageTrue;
            //}
            /*
            else
            {
                emptyMask |= AuthoringStates.IsPublishingPageFalse;
            }
            */

            if ((currentWebPartManager == null) || (currentWebPartManager.Personalization.Scope == PersonalizationScope.Shared))
            {
                emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.InSharedView;
            }
            else
            {
                emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.InPersonalView;
            }

            if ((SPContext.Current.ContextPageInfo != null) && SPContext.Current.ContextPageInfo.IsWebWelcomePage)
            {
                emptyMask |= AuthoringStates.IsDefaultPageTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.IsDefaultPageFalse;
            }

            /*
            if (ConsoleUtilities.IsMasterPageGalleryUrlForSite())
            {
                emptyMask |= AuthoringStates.IsMasterPageGalleryFileTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.IsMasterPageGalleryFileFalse;
            }
*/
            emptyMask |= AuthoringStates.IsMasterPageGalleryFileFalse;

            bool flag3 = false;
            bool flag4 = false;
            if (((currentWebPartManager == null) || (currentWebPartManager.Zones == null)) || (currentWebPartManager.Zones.Count == 0))
            {
                emptyMask |= AuthoringStates.PageHasPersonalizableZonesFalse | AuthoringStates.PageHasCustomizableZonesFalse;
            }
            else
            {
                foreach (System.Web.UI.WebControls.WebParts.WebPartZone zone in currentWebPartManager.Zones)
                {
                    Microsoft.SharePoint.WebPartPages.WebPartZone zone2 = zone as Microsoft.SharePoint.WebPartPages.WebPartZone;
                    if (zone2 != null)
                    {
                        if (zone2.AllowPersonalization)
                        {
                            flag3 = true;
                        }
                        if (zone2.AllowCustomization)
                        {
                            flag4 = true;
                        }
                    }
                }
                if (flag3)
                {
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageHasPersonalizableZonesTrue;
                }
                else
                {
                    emptyMask |= AuthoringStates.PageHasPersonalizableZonesFalse;
                }
                if (flag4)
                {
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageHasCustomizableZonesTrue;
                }
                else
                {
                    emptyMask |= AuthoringStates.EmptyMask | AuthoringStates.PageHasCustomizableZonesFalse;
                }
            }
            if ((list != null) && list.EnableMinorVersions)
            {
                emptyMask |= AuthoringStates.MinorVersionsEnabledTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.MinorVersionsEnabledFalse;
            }
            if ((list != null) && list.ForceCheckout)
            {
                emptyMask |= AuthoringStates.CheckOutRequiredTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.CheckOutRequiredFalse;
            }
            if ((list != null) && list.EnableModeration)
            {
                emptyMask |= AuthoringStates.ContentApprovalEnabledTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.ContentApprovalEnabledFalse;
            }
            if (((context.FormContext != null) && (context.FormContext.FieldControlCollection != null)) && (context.FormContext.FieldControlCollection.Count != 0))
            {
                emptyMask |= AuthoringStates.PageHasFieldControlsTrue;
            }
            else
            {
                emptyMask |= AuthoringStates.PageHasFieldControlsFalse;
            }
            /*
            if ((list != null) && !ScheduledItem.GetIsSchedulingEnabledOnList(list))
            {
                emptyMask |= AuthoringStates.IsSchedulingEnabledFalse;
            }
            */
            //emptyMask |= AuthoringStates.IsScheduledStatusTrue;

            //if (ConsoleVisibleUtilities.IsRibbonAlwaysShownByDefault(web))
            //{
            emptyMask |= AuthoringStates.IsRibbonAlwaysShownTrue;
            //}

            //if (cacheResult)
            //{
            current.Items["currentAuthoringStates"] = emptyMask;
            //}
            return emptyMask;
        }

        public override void PopulateStatusMessages()
        {
            /*
            if (base.currentState["ItemIsFormsPage"] == null)
            {
                //CachedPage myPage = null;
                //CachedListItem contextualListItemCached = null;
                DictionaryEntry entry = new DictionaryEntry();
                entry.Key = HttpContext.GetGlobalResourceObject("cms", "statusindicator_status_label", CultureInfo.CurrentUICulture).ToString() + ":";
                entry.Value = PublishingPageStatusIndicator.RenderStatusString(this.Page, string.Empty, this.ID);
                base.statusMessages.Add(entry);
                SPList contextualList = ConsoleUtilities.ContextualList;
                if (contextualList != null)
                {
                    contextualListItemCached = ConsoleUtilities.ContextualListItemCached;
                }
                if (contextualListItemCached != null)
                {
                    myPage = contextualListItemCached as CachedPage;
                }
                if (((contextualList != null) && ScheduledItem.GetIsSchedulingEnabledOnList(contextualList)) && ((contextualListItemCached != null) && !PublishingPageStatusIndicator.IsLockedAsSystem(contextualListItemCached)))
                {
                    DictionaryEntry entry2 = new DictionaryEntry();
                    entry2.Key = HttpContext.GetGlobalResourceObject("cms", "statusindicator_publicationstartdate_label", CultureInfo.CurrentUICulture).ToString() + ":";
                    StringBuilder builder = new StringBuilder();
                    string valueToEncode = EditPropertiesAction.EditPageUrl(this.Page, contextualListItemCached);
                    bool flag = ConsoleUtilities.ContextualPersonalizationScope(this.Page) == PersonalizationScope.Shared;
                    bool flag2 = ConsoleUtilities.IsCheckedOutByOtherUser(contextualListItemCached);
                    bool flag3 = flag && flag2;
                    if (flag3)
                    {
                        builder.Append("<a id=\"" + HttpEncodingUtility.HtmlAttributeEncode(this.ID + "_anchor") + "\" href=\"" + SPHttpUtility.HtmlUrlAttributeEncode(SPHttpUtility.NoEncode(valueToEncode)) + "\" title=\"");
                        builder.Append(SPHttpUtility.HtmlEncode(Resources.GetFormattedString("ConsoleEditPublicationScheduleTooltip", new object[0])));
                        builder.Append("\">");
                    }
                    if (myPage != null)
                    {
                        builder.Append(ConsoleUtilities.GetPageStartPublishDateText(myPage));
                    }
                    else
                    {
                        builder.Append(SPHttpUtility.HtmlEncodeAllowSimpleTextFormatting(Resources.GetFormattedString("PageDetailsNoPublishDate", new object[0])));
                    }
                    if (flag3)
                    {
                        builder.Append("</a>");
                    }
                    entry2.Value = builder.ToString();
                    base.statusMessages.Add(entry2);
                }
                if (((myPage != null) && (HttpContext.Current != null)) && (HttpContext.Current.Request.QueryString.Get("route") == "1"))
                {
                    DictionaryEntry entry3 = new DictionaryEntry();
                    entry3.Key = Resources.GetString("PageRoutedInformationTitle");
                    entry3.Value = Resources.GetString("PageRoutedInformationValue");
                    base.statusMessages.Add(entry3);
                }
                if (((myPage != null) && !myPage.IsOnHold) && (!myPage.IsRecord && (base.currentState["ItemIsCheckedOutToOtherUser"] != null)))
                {
                    DictionaryEntry entry4 = new DictionaryEntry();
                    entry4.Value = ConsoleUtilities.GetPageCheckedOutMessage(ConsoleUtilities.ContextualFile);
                    entry4.Key = Resources.GetString("PageCheckoutInformationTitle");
                    base.statusMessages.Add(entry4);
                }
            }
            */
        }

        public static string PrependNotificationCode(string strJSCode, string notificationResource)
        {
            
            StringBuilder builder = new StringBuilder();
            /*
            builder.Append("SP.UI.Notify.addNotification(");
            builder.Append(notificationResource);
            builder.Append(");");
            */
            builder.Append(strJSCode);
            return builder.ToString();
            
        }

        internal void RaisePostBackEventForPageRouting(string eventArgument, SPRibbonCommandHandler control, RaisePostBackEventDelegate raisePostBackEventDelegate)
        {
            try
            {
                if (eventArgument == control.ClientSideCommandId)
                {
                    //PublishingPage publishingPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                    WebPageCheckinHandler handler = control as WebPageCheckinHandler;
                    WebPageSaveHandler handler2 = control as WebPageSaveHandler;
                    WebPageSaveAndStopEditHandler handler3 = control as WebPageSaveAndStopEditHandler;
                    
                    WebPagePublishHandler publish = control as WebPagePublishHandler;
                    WebPageUnpublishHandler unpublish = control as WebPageUnpublishHandler;

                    if (publish != null || unpublish != null)
                    {
                        /*
                        int versionId = 0;
                        SPFileVersionCollection versions = SPContext.Current.File.Versions;

                        try
                        {
                            foreach (SPFileVersion version in versions)
                            {
                                if (version.Level == SPFileLevel.Published)
                                {
                                    if (versionId <= version.ID)
                                    {
                                        versionId = version.ID;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (versionId > 0)
                        {
                            SPContext.Current.File.CheckOut(SPFile.SPCheckOutType.None, DateTime.Now.ToString());

                            if (SPContext.Current.File.Properties.ContainsKey("vti_publicversion"))
                            {
                                SPContext.Current.File.SetProperty("vti_publicversion",versionId);
                            }
                            else
                            {
                                SPContext.Current.File.AddProperty("vti_publicversion", versionId);
                                //SPContext.Current.File.Properties.Add("vti_publicversion", versionId);
                            }
                            
                            SPContext.Current.File.Update();

                            SPContext.Current.File.CheckIn("publicversion", SPCheckinType.OverwriteCheckIn);
                        }
                        */
                        string url = SPContext.Current.ListItem.File.ServerRelativeUrl;

                        SPRedirectFlags flags = SPRedirectFlags.Trusted;
                        if (unpublish != null)
                        {
                            flags = (SPRedirectFlags.Default | SPRedirectFlags.DoNotEndResponse | SPRedirectFlags.DoNotEncodeUrl);
                        }

                        SPUtility.Redirect(url, flags, HttpContext.Current);
                        //CleanRedirect(url, true, true);

                    }

                    else if (((handler != null) || ((handler2 != null) && !SPContext.Current.List.ForceCheckout)) || ((control is WebPageSaveAndStopEditHandler) && !SPContext.Current.List.ForceCheckout))
                    {
                        if (base.EnsureItemSavedIfEditMode(eventArgument))
                        {
                            string checkInComments = string.Empty;
                            if (handler != null)
                            {
                                checkInComments = control.Page.Request.Form[SPPageStateControl.InputCommentsClientId];
                            }

                            //if (!this.CheckMissingRequiredFields())
                            //{
                            string directoryName = Path.GetDirectoryName(SPContext.Current.ListItem.File.Url);
                            //publishingPage = PublishingPage.Route(publishingPage, checkInComments);
                            string str3 = Path.GetDirectoryName(SPContext.Current.ListItem.File.Url);
                            if ((handler != null) && (SPContext.Current.ListItem.File.CheckOutType != SPFile.SPCheckOutType.None))
                            {
                                SPContext.Current.ListItem.File.CheckIn(checkInComments,SPCheckinType.MinorCheckIn);
                                this.OnPageStateChanged();
                            }

                            if (handler2 != null)
                            {
                                string url = AddQueryStringParameter(AddQueryStringParameter(base.ContextUri, "ControlMode", "Edit"), "DisplayMode", "Design");

                                SPUtility.Redirect("~/"+url, SPRedirectFlags.Trusted, HttpContext.Current);
                            }
                            else
                            {
                                string url = SPContext.Current.ListItem.File.Name;
                                /*
                                if (directoryName != str3)
                                {
                                    url = AddQueryStringParameterToUrl(url, "route", "1");
                                }
                                */
                                CleanRedirect(url, true, true);
                            }
                            
                            //}
                        }
                    }
                    else
                    {
                        raisePostBackEventDelegate(eventArgument);

                    }
                }
            }
            catch (SPException exception)
            {
                control.SetGenericErrorMessage(exception);
            }
        }


        protected override void RefreshAuthoredItemVersion()
        {
            base.RefreshAuthoredItemVersion();
            /*
            if (ConsoleUtilities.ContextualListItemCached != null)
            {
                ConsoleContext.AuthoringItemVersion = ConsoleUtilities.GetListItemVersion(ConsoleUtilities.ContextualListItemCached);
            }
            */
        }

        /*
        public override string CurrentItemCheckoutOwner
        {
            get
            {
                return ConsoleContext.CurrentItemCheckoutOwnerId;
            }
        }

        public override string CurrentItemVersion
        {
            get
            {
                return ConsoleContext.CurrentItemVersion;
            }
        }
        */

        internal static string CleanDisplayModeUrl(string url)
        {
            string str = "DisplayMode";
            string str2 = "ToolPaneView";
            string str3 = "ControlMode";
            string[] strArray = url.Split(new char[] { '?' });
            string str4 = strArray[0];
            string str5 = string.Empty;
            if (strArray.Length > 1)
            {
                str5 = strArray[1];
            }
            string[] strArray2 = str5.Split(new char[] { '&' });
            StringBuilder builder = new StringBuilder();
            foreach (string str6 in strArray2)
            {
                if ((!str6.StartsWith(str + "=", StringComparison.OrdinalIgnoreCase) && !str6.StartsWith(str2 + "=", StringComparison.OrdinalIgnoreCase)) && (!str6.StartsWith(str3 + "=", StringComparison.OrdinalIgnoreCase) && (str6.Length != 0)))
                {
                    builder.Append(str6 + "&");
                }
            }
            if (builder.ToString().Length != 0)
            {
                str4 = str4 + "?" + builder.ToString();
            }
            return str4;
        }

        internal static void CleanRedirect(string url)
        {
            CleanRedirect(url, true);
        }

        internal static void CleanRedirect(string url, bool encodeUrl)
        {
            CleanRedirect(url, encodeUrl, false);
        }

        internal static void CleanRedirect(string url, bool encodeUrl, bool endResponse)
        {
            if ((url != null) && (url.Length > 0))
            {
                SPRedirectFlags flags = SPRedirectFlags.Default;
                if (!endResponse)
                {
                    flags |= SPRedirectFlags.DoNotEndResponse;
                }
                if (!encodeUrl)
                {
                    flags |= SPRedirectFlags.DoNotEncodeUrl;
                }

                flags |= SPRedirectFlags.Trusted;

                SPUtility.Redirect(RemoveAuthoringErrors(CleanDisplayModeUrl(url)), flags, HttpContext.Current);
            }
        }

        internal static string RemoveAuthoringErrors(string url)
        {
            return RemoveQueryStringParameter(url, "AuthoringError");
        }

        internal static string RemoveQueryStringParameter(string url, string paramName)
        {
            string[] strArray = url.Split(new char[] { '?' });
            string str = strArray[0];
            string str2 = string.Empty;
            if (strArray.Length > 1)
            {
                str2 = strArray[1];
            }
            string[] strArray2 = str2.Split(new char[] { '&' });
            StringBuilder builder = new StringBuilder();
            foreach (string str3 in strArray2)
            {
                if (!str3.StartsWith(paramName + "=", StringComparison.OrdinalIgnoreCase) && (str3.Length != 0))
                {
                    builder.Append(str3 + "&");
                }
            }
            string str4 = builder.ToString().TrimEnd(new char[] { '&' });
            if (str4.Length != 0)
            {
                str = str + "?" + str4;
            }
            return str;
        }

        internal static string AddQueryStringParameter(string url, string paramName, string paramValue)
        {
            string str = SPHttpUtility.UrlKeyValueEncode(paramName) + "=" + SPHttpUtility.UrlKeyValueEncode(paramValue);

            string[] strArray = url.Split(new char[] { '?' });
            string str2 = strArray[0];
            string str3 = string.Empty;
            if (strArray.Length > 1)
            {
                str3 = strArray[1];
            }
            if (str3.Length == 0)
            {
                return (url + "?" + str);
            }
            if (!str3.Contains(paramName))
            {
                return (url + "&" + str);
            }
            string[] strArray2 = str3.Split(new char[] { '&' });
            StringBuilder builder = new StringBuilder();
            foreach (string str4 in strArray2)
            {
                if (!str4.StartsWith(paramName + "=", StringComparison.OrdinalIgnoreCase) && (str4.Length != 0))
                {
                    builder.Append(str4 + "&");
                }
            }
            builder.Append(str);
            return (str2 + "?" + builder.ToString());

        }
        
    }
}