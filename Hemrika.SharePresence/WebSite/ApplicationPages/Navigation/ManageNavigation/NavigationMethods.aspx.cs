// <copyright file="ManageNavigation.cs" company="Hemrika">
// Copyright Hemrika. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2012-02-01 10:19:51Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System.Web.Services;
    using Hemrika.SharePresence.WebSite.Navigation;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Add comment for ManageNavigation
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class NavigationMethods : LayoutsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the ManageNavigation class
        /// </summary>
        public NavigationMethods()
        {
            this.RightsCheckMode = RightsCheckModes.OnPreInit;
        }

        /// <summary>
        /// Defines which rights are required
        /// </summary>
        protected override SPBasePermissions RightsRequired
        {
            get
            {
                return base.RightsRequired | SPBasePermissions.BrowseUserInfo | SPBasePermissions.ManageLists;
            }
        }

        /// <summary>
        /// Sets the inital values of controls
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            SPSite siteCollection = this.Site;
            SPWeb site = this.Web;
        }

        /*
        Use this code to perform own security checks
        protected virtual void CheckCustomRights()
        {

          bool userCheckedForCustomLogic = false;
          //write here a custom logic to check if user has enough rights to access application page
          //if yes, set userCheckedForCustomLogic to true;

          if (!userCheckedForCustomLogic)
          {
            SPUtility.HandleAccessDenied(new UnauthorizedAccessException());
          }
        } 

        protected override void OnLoad(EventArgs e)
        {
          this.CheckCustomRights();   
        }
        */

        [System.Web.Services.WebMethod]
        public static List<WebSiteNode> GetSortableLists(int pageNumber, int pageSize)
        {
            return new List<WebSiteNode>();
            //DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();
            //return SLR.GetSortableLists(true);
        }

        [System.Web.Services.WebMethod]
        public static List<WebSiteNode> GetSortableList(int sortableListId)
        {
            return new List<WebSiteNode>();
            //DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();
            //return SLR.GetSortableList(sortableListId, true);
        }

        [System.Web.Services.WebMethod]
        public static List<WebSiteNode> GetSortableListItems(int sortableListId)
        {
            return new List<WebSiteNode>();
            //DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();
            //return SLR.GetSortableListItems(sortableListId);
        }

        [System.Web.Services.WebMethod]
        public static WebSiteNode GetSortableListItem(int sortableListItemId)
        {
            return new WebSiteNode();
            //DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();
            //return SLR.GetSortableListItem(sortableListItemId, true);

        }

        [System.Web.Services.WebMethod]
        public static void SaveListPosition(int sortableListItemId, int newPosition)
        {
            //DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();
            //SLR.MoveSortableListItem(sortableListItemId, newPosition);
        }


        [System.Web.Services.WebMethod]
        public static int SaveSortableList(int sortableListId, string listDescription)
        {
            /*
            DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();

            DragAndDropSortableList.Data.Model.SortableList list = new DragAndDropSortableList.Data.Model.SortableList();

            if (sortableListId > 0)
                list = SLR.GetSortableList(sortableListId);

            list.Description = listDescription;

            list = SLR.SaveList(list);

            return list.SortableListId;
            */
            return sortableListId;
        }

        [System.Web.Services.WebMethod]
        public static void SaveSortableListItem(int sortableListId, int sortableListItemId, string headline, string description, string linkUrl)
        {
            /*
            DragAndDropSortableList.Data.Repository.SortableListRepository SLR = new DragAndDropSortableList.Data.Repository.SortableListRepository();

            DragAndDropSortableList.Data.Model.SortableListItem item = new DragAndDropSortableList.Data.Model.SortableListItem();
            if (sortableListItemId > 0)
                item = SLR.GetSortableListItem(sortableListItemId);

            item.SortableListId = sortableListId;
            item.Headline = headline;
            item.Description = description;
            item.LinkUrl = linkUrl;

            SLR.SaveSortableListItem(item);
            */
        }

    }
}

