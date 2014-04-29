// -----------------------------------------------------------------------
// <copyright file="CombinedBasePermissions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Page
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class CombinedBasePermissions
    {
        private SPBasePermissions currentListItemPermissions;
        private SPBasePermissions currentListPermissions;
        private SPBasePermissions currentSitePermissions;
        private SPBasePermissions rootSitePermissions;

        internal CombinedBasePermissions()
        {
            SPContext current = SPContext.Current;

            if (current != null)
            {
                if (current.Site != null)
                {
                    SPWeb rootWeb = current.Site.RootWeb;
                    if (rootWeb != null)
                    {
                        try
                        {
                            this.rootSitePermissions = rootWeb.EffectiveBasePermissions;
                        }
                        catch (NullReferenceException)
                        {
                        }
                    }
                }

                if (current.Web != null)
                {
                    this.currentSitePermissions = current.Web.EffectiveBasePermissions;
                }

                if (current.List != null)
                {
                    if (!current.List.HasUniqueRoleAssignments)
                    {
                        this.currentListPermissions = this.currentSitePermissions;
                    }
                    else
                    {
                        this.currentListPermissions = current.List.EffectiveBasePermissions;
                    }
                }
                else
                {
                    this.currentListPermissions = SPBasePermissions.EmptyMask;
                }
                if (((current != null) && (current.ContextPageInfo != null)) && current.ContextPageInfo.BasePermissions.HasValue)
                {
                    this.currentListItemPermissions = current.ContextPageInfo.BasePermissions.Value;
                }
                else
                {
                    if (current.ListItem != null)
                    {
                        try
                        {
                            if (current.ListItem.ID > 0)
                            {
                                this.currentListItemPermissions = current.ListItem.EffectiveBasePermissions;
                            }
                            else if (current.ListItem.File != null)
                            {
                                this.currentListItemPermissions = current.ListItem.File.ParentFolder.Item.EffectiveBasePermissions;
                            }
                            else
                            {
                                this.currentListItemPermissions = this.currentListPermissions;
                            }
                            return;
                        }
                        catch (NullReferenceException)
                        {
                            this.currentListItemPermissions = this.currentListPermissions;
                            return;
                        }
                    }
                    this.currentListItemPermissions = SPBasePermissions.EmptyMask;
                }
            }
        }

        internal SPBasePermissions ListItemPermissions
        {
            get
            {
                return this.currentListItemPermissions;
            }
        }

        internal SPBasePermissions ListPermissions
        {
            get
            {
                return this.currentListPermissions;
            }
        }

        internal SPBasePermissions RootSitePermissions
        {
            get
            {
                return this.rootSitePermissions;
            }
        }

        internal SPBasePermissions SitePermissions
        {
            get
            {
                return this.currentSitePermissions;
            }
        }
    }
}