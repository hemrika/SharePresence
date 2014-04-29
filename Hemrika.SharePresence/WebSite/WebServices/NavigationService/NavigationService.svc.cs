// <copyright file="Navigation.svc.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2013-03-15 20:32:38Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System.ServiceModel.Activation;
    using Microsoft.SharePoint.Client.Services;
    using System.Web.Services;
    using Hemrika.SharePresence.WebSite.Navigation;
    using System.Collections.Generic;

    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [WebService(Namespace = "http://sharepresence.com/", Description = "Navigation Service", Name = "NavigationService")]
    public class NavigationService : INavigationService
    {
        // Access the web service from URL http://<Servername>/_vti_bin/SharePresence/NavigationService.svc/mex

        public List<WebSiteNode> GetSortableLists(int pageNumber, int pageSize)
        {
            WebSiteDataSource source = new WebSiteDataSource { ShowStartingNode = true, SiteMapProvider = "WebSiteProvider", SelectMethod = "GetLists", SelectParameters = null };
            return source.Provider.RootNode.Children;
        }

        public void GetSortableList(int sortableListId)
        {
        }

        public void GetSortableListItems(int sortableListId)
        {
        }

        public void GetSortableListItem(int sortableListItemId)
        {
        }

        public void SaveListPosition(int sortableListItemId, int newPosition)
        {
        }

        public void SaveSortableList(int sortableListId, string listDescription)
        {
        }

        public void SaveSortableListItem(int sortableListId, int sortableListItemId, string headline, string description, string linkUrl)
        {
        }
    }
}

