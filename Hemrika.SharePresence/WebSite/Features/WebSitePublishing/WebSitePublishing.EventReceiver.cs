// <copyright file="Hemrika_SharePoint_WebSitePublishing.EventReceiver.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-01-11 13:52:15Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Hemrika.SharePresence.Common.WebSiteController;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.Utilities;
    using System.Security.Principal;
    using Microsoft.SharePoint.Administration.Claims;
    using System.IO;

    /// <summary>
    /// TODO: Add comment to WebSitePublishingEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class WebSitePublishingEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPUtility.ValidateFormDigest();

            try
            {
                ConfigurePageLibrary(properties);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //ex.ToString();
            }

            EnableFormsLockDown(properties);

            try
            {
                ConfigureMasterPageGallery(properties);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

            try
            {
                ConfigureStyleDocumentLibrary(properties);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

            try
            {
                AddSecuritySettings(properties);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb rootWeb = site.RootWeb;
            SPBasePermissions permissions = rootWeb.AnonymousPermMask64;

        }

        private static void ConfigurePageLibrary(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            SPList page = site.RootWeb.Lists["Pages"];
            //page.DraftVersionVisibility = DraftVisibilityType.Author | DraftVisibilityType.Approver;
            page.EnableVersioning = true;
            page.EnableMinorVersions = true;
            page.EnableThrottling = true;
            page.Update();
        }

        private void EnableFormsLockDown(SPFeatureReceiverProperties properties)
        {
            if (properties.Feature.Parent is SPSite)
            {
                SPSite site = (SPSite)properties.Feature.Parent;
                foreach (SPWeb web in site.AllWebs)
                {
                    try
                    {

                        //SPWeb rootWeb = site.RootWeb;
                        SPRoleDefinition guestRole = web.RoleDefinitions.GetByType(SPRoleType.Guest);
                        guestRole.BasePermissions &= ~(SPBasePermissions.EmptyMask | SPBasePermissions.ViewFormPages);
                        guestRole.BasePermissions &= ~SPBasePermissions.UseRemoteAPIs;
                        guestRole.BasePermissions |= SPBasePermissions.ViewVersions;
                        guestRole.Update();
                        SPBasePermissions guest = guestRole.BasePermissions;

                        web.AnonymousPermMask64 &= ~(SPBasePermissions.UseRemoteAPIs | SPBasePermissions.ViewFormPages);
                        web.Update();
                        web.AnonymousPermMask64 |= SPBasePermissions.ViewVersions;
                        web.Update();
                        SPBasePermissions permissions = web.AnonymousPermMask64;
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    }
                }
            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            DisableFormsLockDown(properties);
        }

        private void DisableFormsLockDown(SPFeatureReceiverProperties properties)
        {
            if (properties.Feature.Parent is SPSite)
            {
                try
                {
                    SPSite site = (SPSite)properties.Feature.Parent;
                    SPWeb rootWeb = site.RootWeb;
                    SPRoleDefinition guestRole = rootWeb.RoleDefinitions.GetByType(SPRoleType.Guest);
                    guestRole.BasePermissions |= SPBasePermissions.EmptyMask | Microsoft.SharePoint.SPBasePermissions.ViewFormPages;// | Microsoft.SharePoint.SPBasePermissions.Open | Microsoft.SharePoint.SPBasePermissions.BrowseUserInfo | Microsoft.SharePoint.SPBasePermissions.UseClientIntegration | Microsoft.SharePoint.SPBasePermissions.UseRemoteAPIs;
                    guestRole.BasePermissions |= SPBasePermissions.UseRemoteAPIs;
                    guestRole.Update();
                    //rootWeb.AnonymousPermMask64 = Microsoft.SharePoint.SPBasePermissions.ViewListItems | Microsoft.SharePoint.SPBasePermissions.ViewVersions | Microsoft.SharePoint.SPBasePermissions.ViewFormPages | Microsoft.SharePoint.SPBasePermissions.Open | Microsoft.SharePoint.SPBasePermissions.ViewPages | Microsoft.SharePoint.SPBasePermissions.UseClientIntegration;
                    rootWeb.Update();

                }
                catch (Exception ex)
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                    //ex.ToString();
                }

            }
        }

        /// <summary>
        /// TODO: Add comment to describe the actions after feature installation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature uninstalling
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature upgrading
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        /// <param name="upgradeActionName">The name of the custom upgrade action to execute. The value can be null if the override of this method implements only one upgrade action.</param>
        /// <param name="parameters">Parameter names and values for the custom action</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            ////TODO: place receiver code here or remove method
        }

        private SPList ConfigureMasterPageGallery(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb rootWeb = site.RootWeb;

            SPList catalog = null;
            try
            {

                //bool updateRequired = false;
                catalog = rootWeb.GetCatalog(SPListTemplateType.MasterPageCatalog);
                DisableCrawlOnList(catalog);
                /*
                EnableStandardVersioningOnList(catalog, ref updateRequired);
                */
                /*
                if (!catalog.EnableModeration)
                {
                    catalog.EnableModeration = true;
                    updateRequired = true;
                }
                if (updateRequired)
                {
                    catalog.Update();
                }
                */
                EnableAllowEveryoneViewItems(catalog);
                SPFolder rootFolder = catalog.RootFolder;
                AddFolder(rootFolder, "Preview Images");
                //AddFolder(rootFolder, "Editing Menu");
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            return catalog;
        }

        private SPList ConfigureStyleDocumentLibrary(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb rootWeb = site.RootWeb;

            SPList list = null;
            try
            {
                SPListCollection lists = rootWeb.Lists;

                bool updateRequired = false;
                bool newListCreated = false;

                list = AddList(lists, "Style Library", "Style Library", "$Resources:cmscore,ListDescriptionRootStyles;", new Guid("00BFEA71-E717-4E80-AA17-D0C71B360101"), null, SPListTemplateType.DocumentLibrary, out newListCreated);
                //list = lists["Style Library"];

                if (list != null)
                {
                    EnableFolderCreationOnList(list, ref updateRequired);
                    if (list.AllowDeletion)
                    {
                        list.AllowDeletion = false;
                        updateRequired = true;
                    }
                    if (updateRequired)
                    {
                        list.Update();
                    }
                    //EnableStandardVersioningOnList(list, ref updateRequired);
                    EnableAllowEveryoneViewItems(list);
                    DisableCrawlOnList(list);

                    list.Update();
                    SPView defaultView = list.DefaultView;
                    if (defaultView != null)
                    {
                        SPViewFieldCollection viewFields = defaultView.ViewFields;
                        AddFieldToView(list, SPBuiltInFieldId.CheckoutUser, viewFields);
                        defaultView.Update();
                    }

                    //AddFolder(list.RootFolder, "XSL Style Sheets");
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            return list;
        }

        private void AddSecuritySettings(SPFeatureReceiverProperties properties)
        {
            SPGroup authenticatedGroup = null;
            SPGroup designersGroup = null;
            SPGroup approversGroup = null;
            SPGroup viewersGroup = null;
            //SPGroup guestGroup = null;

            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb rootWeb = site.RootWeb;

            try
            {

                SPGroupCollection siteGroups = rootWeb.SiteGroups;
                SPUser currentUser = rootWeb.CurrentUser;
                bool addOwnerToGroup = true;
                if (rootWeb.Site.SystemAccount.ID == currentUser.ID)
                {
                    addOwnerToGroup = false;
                }

                authenticatedGroup = AddSecurityGroup(siteGroups, "All Authenticated Users", "All Authenticated Users", currentUser, addOwnerToGroup);
                designersGroup = AddSecurityGroup(siteGroups, "Designers", "Designers", currentUser, addOwnerToGroup);
                approversGroup = AddSecurityGroup(siteGroups, "Approvers", "Approvers", currentUser, addOwnerToGroup);
                viewersGroup = AddSecurityGroup(siteGroups, "Viewers", "Viewers", currentUser, addOwnerToGroup);
                //guestGroup = AddSecurityGroup(siteGroups, "Guests", "Guests", currentUser, addOwnerToGroup);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            
            SPRoleDefinition contributerRole = null;
            SPRoleDefinition readerRole = null;
            SPRoleDefinition designerRole = null;
            SPRoleDefinition administratorRole = null;
            SPRoleDefinition guestRole = null;

            SPRoleDefinition approverRole = null;
            SPRoleDefinition viewerRole = null;

            try
            {
                SPRoleDefinitionCollection roleDefinitions = rootWeb.RoleDefinitions;
                bool flag2 = false;
                foreach (SPRoleDefinition definition7 in roleDefinitions)
                {
                    if ((definition7.Order != 0x7fffffff) && (definition7.Order != 0))
                    {
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    contributerRole = SetRoleDefinitionOrder(rootWeb, SPRoleType.Contributor, 5);
                    readerRole = SetRoleDefinitionOrder(rootWeb, SPRoleType.Reader, 6);
                    designerRole = SetRoleDefinitionOrder(rootWeb, SPRoleType.WebDesigner, 2);
                    administratorRole = SetRoleDefinitionOrder(rootWeb, SPRoleType.Administrator, 1);
                    guestRole = SetRoleDefinitionOrder(rootWeb, SPRoleType.Guest, 8);
                }
                else
                {
                    contributerRole = roleDefinitions.GetByType(SPRoleType.Contributor);
                    readerRole = roleDefinitions.GetByType(SPRoleType.Reader);
                    designerRole = roleDefinitions.GetByType(SPRoleType.WebDesigner);
                    administratorRole = roleDefinitions.GetByType(SPRoleType.Administrator);
                    guestRole = roleDefinitions.GetByType(SPRoleType.Guest);
                }

                approverRole = AddRoleDefinition(roleDefinitions, "Approver", "Approver", SPBasePermissions.BrowseDirectories | SPBasePermissions.AddDelPrivateWebParts | SPBasePermissions.BrowseUserInfo | SPBasePermissions.CreateSSCSite | SPBasePermissions.EditMyUserInfo | SPBasePermissions.CreateAlerts | SPBasePermissions.UpdatePersonalWebParts | SPBasePermissions.UseRemoteAPIs | SPBasePermissions.UseClientIntegration | SPBasePermissions.ApproveItems | SPBasePermissions.DeleteListItems | SPBasePermissions.ViewVersions | SPBasePermissions.OpenItems | SPBasePermissions.EditListItems | SPBasePermissions.AddListItems | SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages | SPBasePermissions.Open | SPBasePermissions.ViewFormPages | SPBasePermissions.CancelCheckout | SPBasePermissions.DeleteVersions | SPBasePermissions.ManagePersonalViews, flag2 ? 0x7fffffff : 4);
                viewerRole = AddRoleDefinition(roleDefinitions, "Viewer", "Viewer", SPBasePermissions.OpenItems | SPBasePermissions.ViewListItems | SPBasePermissions.ViewPages | SPBasePermissions.Open, flag2 ? 0x7fffffff : 7);

            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

            SPUser authenticatedUser = null;

            try
            {
                string logonName = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null).Translate(typeof(NTAccount)).Value;
                authenticatedUser = rootWeb.EnsureUser(logonName);
                authenticatedGroup.AddUser(authenticatedUser);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                authenticatedUser = null;
            }

            /*
            SPUser anonymousUser = null;
            try
            {
                string logonName = new SecurityIdentifier(WellKnownSidType.AnonymousSid, null).Translate(typeof(NTAccount)).Value;
                anonymousUser = rootWeb.EnsureUser(logonName);
                guestGroup.AddUser(anonymousUser);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                anonymousUser = null;
            }
            */

            try
            {
                SPClaim claim = SPAllUserClaimProvider.CreateAuthenticatedUserClaim(true);
                string str4 = SPClaimProviderManager.Local.EncodeClaim(claim);
                authenticatedUser = rootWeb.EnsureUser(str4);
                authenticatedGroup.AddUser(authenticatedUser);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                authenticatedUser = null;
            }


            try
            {
                /*
                SPList catalog = rootWeb.GetCatalog(SPListTemplateType.MasterPageCatalog);
                SPList styleLibrary = rootWeb.Lists["Style Library"];

                catalog.BreakRoleInheritance(true);
                styleLibrary.BreakRoleInheritance(true);
                */
                SPRoleAssignmentCollection roleAssignments = rootWeb.RoleAssignments;
                //SPRoleAssignmentCollection catalogRoles = catalog.RoleAssignments;
                //SPRoleAssignmentCollection styleRoles = styleLibrary.RoleAssignments;

                SPRoleAssignment DesignerAssignment = new SPRoleAssignment(designersGroup);                
                DesignerAssignment.RoleDefinitionBindings.Add(designerRole);

                //SPRoleAssignment GuestAssignment = new SPRoleAssignment(guestGroup);
                //GuestAssignment.RoleDefinitionBindings.Add(guestRole);

                roleAssignments.Add(DesignerAssignment);
                //roleAssignments.Add(GuestAssignment);
                //catalogRoles.Add(DesignerAssignment);
                //catalogRoles.Add(GuestAssignment);
                //styleRoles.Add(DesignerAssignment);
                //styleRoles.Add(GuestAssignment);

                AddRoleAssignment(roleAssignments, approversGroup, approverRole, true);
                AddRoleAssignment(roleAssignments, viewersGroup, viewerRole, true);

                //AddRoleAssignment(roleAssignments, guestGroup, guestRole, true);

                /*
                AddRoleAssignment(catalogRoles, approversGroup, readerRole, true);
                AddRoleAssignment(catalogRoles, viewersGroup, readerRole, true);
                AddRoleAssignment(catalogRoles, authenticatedGroup, readerRole, false);

                AddRoleAssignment(catalogRoles, guestGroup, guestRole, false);

                AddRoleAssignment(catalogRoles, approversGroup, viewerRole, true);
                AddRoleAssignment(catalogRoles, viewersGroup, viewerRole, true);
                AddRoleAssignment(catalogRoles, authenticatedGroup, viewerRole, true);

                AddRoleAssignment(styleRoles, approversGroup, viewerRole, true);
                AddRoleAssignment(styleRoles, viewersGroup, viewerRole, true);
                AddRoleAssignment(styleRoles, authenticatedGroup, viewerRole, true);

                AddRoleAssignment(styleRoles, guestGroup, guestRole, true);
                */
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
        }

        internal static void EnableStandardVersioningOnList(SPList list, ref bool updateRequired)
        {
            EnableStandardVersioningOnList(list, true, ref updateRequired);
        }

        internal static void EnableStandardVersioningOnList(SPList list, bool forceCheckout, ref bool updateRequired)
        {
            EnableStandardVersioningOnList(list, forceCheckout, true, ref updateRequired);
        }

        internal static void EnableStandardVersioningOnList(SPList list, bool forceCheckout, bool enableMinorVersions, ref bool updateRequired)
        {
            if (!list.EnableVersioning)
            {
                list.EnableVersioning = true;
                updateRequired = true;
            }
            if (list.DraftVersionVisibility != DraftVisibilityType.Author)
            {
                list.DraftVersionVisibility = DraftVisibilityType.Author;
                updateRequired = true;
            }
            if (list.BaseType == SPBaseType.DocumentLibrary)
            {
                if (list.EnableMinorVersions != enableMinorVersions)
                {
                    list.EnableMinorVersions = enableMinorVersions;
                    updateRequired = true;
                }
                if (list.ForceCheckout != forceCheckout)
                {
                    list.ForceCheckout = forceCheckout;
                    updateRequired = true;
                }
            }
        }

        internal static void EnableAllowEveryoneViewItems(SPList list)
        {
            list.AllowEveryoneViewItems = true;
            list.Update();
        }

        internal static void EnableFolderCreationOnList(SPList list, ref bool updateRequired)
        {
            if (!list.EnableFolderCreation)
            {
                if (list.ServerTemplateCanCreateFolders)
                {
                    list.EnableFolderCreation = true;
                    updateRequired = true;
                }
            }
        }

        internal static SPFolder AddFolder(SPFolder parentFolder, string folderUrl)
        {
            SPFolder folder = Microsoft.Office.Server.Utilities.SPFolderHierarchy.GetSubFolder(parentFolder, folderUrl, true);
            SPListItem item = null;
            try
            {
                item = folder.Item;
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }

            if (item != null)
            {
                SPModerationInformation moderationInformation = item.ModerationInformation;
                if (moderationInformation != null)
                {
                    moderationInformation.Status = SPModerationStatusType.Approved;
                    item.Update();
                }
            }

            return folder;
        }

        internal static void DisableCrawlOnList(SPList list)
        {
            if (!list.NoCrawl)
            {
                list.NoCrawl = true;
                list.Update();
            }
        }

        internal static bool AddFieldToView(SPList list, Guid fieldId, SPViewFieldCollection viewfields)
        {
            bool flag = false;
            try
            {
                SPField field = list.Fields[fieldId];
                if ((field != null) && !viewfields.Exists(field.InternalName))
                {
                    viewfields.Add(field);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            return flag;
        }

        internal static SPGroup AddSecurityGroup(SPGroupCollection groups, string name, string description, SPUser owner, bool addOwnerToGroup)
        {
            SPGroup item = null;
            try
            {
                item = groups[name];
                if (addOwnerToGroup)
                {
                    item.AddUser(owner);
                }
            }
            catch (SPException ex)
            {
                if (-2146232832 == ex.ErrorCode)
                {
                    SPMember associatedOwnerGroup = owner;
                    if (groups.Web.AssociatedOwnerGroup != null)
                    {
                        associatedOwnerGroup = groups.Web.AssociatedOwnerGroup;
                    }
                    if (addOwnerToGroup)
                    {
                        groups.Add(name, associatedOwnerGroup, owner, description);
                    }
                    else
                    {
                        groups.Add(name, associatedOwnerGroup, null, description);
                    }
                    item = groups[name];
                }
                else
                {
                    SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                }
            }
            IList<SPGroup> associatedGroups = item.ParentWeb.AssociatedGroups;
            if ((item != null) && (associatedGroups != null))
            {
                bool flag = true;
                foreach (SPGroup group2 in associatedGroups)
                {
                    if (group2.ID == item.ID)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    associatedGroups.Add(item);
                }
            }
            return item;
        }

        internal static SPRoleDefinition SetRoleDefinitionOrder(SPWeb rootWeb, SPRoleType roleType, int order)
        {
            SPRoleDefinition byType = null;
            try
            {
                byType = rootWeb.RoleDefinitions.GetByType(roleType);
                byType.Order = order;
                byType.Update();
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            return byType;
        }

        internal static SPRoleDefinition AddRoleDefinition(SPRoleDefinitionCollection roles, string name, string description, SPBasePermissions permissions, int order)
        {
            SPRoleDefinition role = null;
            try
            {
                role = roles[name];
                role.BasePermissions = permissions;
                role.Description = description;
                role.Order = order;
            }
            catch (SPException exception)
            {
                if (-2146232832 != exception.ErrorCode)
                {
                    throw;
                }
                role = new SPRoleDefinition();
                role.Name = name;
                role.Description = description;
                role.BasePermissions = permissions;
                role.Order = order;
                roles.Add(role);
                role = roles[name];
            }
            return role;
        }

        internal static void AddRoleAssignment(SPRoleAssignmentCollection roleAssignments, SPPrincipal groupOrUser, SPRoleDefinition role, bool addToCurrentScopeOnly)
        {

            SPRoleAssignment roleAssignment = new SPRoleAssignment(groupOrUser);
            roleAssignment.RoleDefinitionBindings.Add(role);
            if (addToCurrentScopeOnly)
            {
                roleAssignments.AddToCurrentScopeOnly(roleAssignment);
            }
            else
            {
                roleAssignments.Add(roleAssignment);
            }
        }

        internal static SPList AddList(SPListCollection lists, string urlName, string title, string description, Guid featureId, Guid[] previousVersionFeatureIds, SPListTemplateType templateType, out bool newListCreated)
        {
            return AddList(lists, urlName, title, description, featureId, previousVersionFeatureIds, (int)templateType, out newListCreated);
        }

        private static SPList AddList(SPListCollection lists, string urlName, string title, string description, Guid featureId, Guid[] previousVersionFeatureIds, int templateType, out bool newListCreated)
        {
            SPList listIfExists = null;
            newListCreated = false;
            try
            {
                listIfExists = lists[title];

                if (listIfExists == null)
                {
                    listIfExists = lists.Web.GetList(urlName);
                }
            }
            catch (SPException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            catch (FileNotFoundException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
            if (listIfExists != null)
            {

                bool flag = listIfExists.TemplateFeatureId.Equals(featureId);
                if ((!flag && (previousVersionFeatureIds != null)) && (previousVersionFeatureIds.Length > 0))
                {
                    foreach (Guid guid in previousVersionFeatureIds)
                    {
                        if (listIfExists.TemplateFeatureId.Equals(guid))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }

            if (listIfExists == null)
            {
                Guid guid2 = lists.Add(title, description, urlName, featureId.ToString("D"), templateType, null);
                newListCreated = true;
                listIfExists = lists[guid2];
            }
            return listIfExists;
        }

    }
}

