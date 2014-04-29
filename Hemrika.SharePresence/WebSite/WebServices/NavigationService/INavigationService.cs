// <copyright file="INavigation.cs" company="SharePresence">
// Copyright SharePresence. All rights reserved.
// </copyright>
// <author>MARKETING\Administrator</author>
// <date>2013-03-15 20:32:38Z</date>
namespace Hemrika.SharePresence.WebSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using Hemrika.SharePresence.WebSite.Navigation;

    [ServiceContract]
    public interface INavigationService
    {
        [OperationContract]
        /*[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]*/
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped ,RequestFormat= WebMessageFormat.Json)]
        List<WebSiteNode> GetSortableLists(int pageNumber, int pageSize);

        [OperationContract]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void GetSortableList(int sortableListId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        void GetSortableListItems(int sortableListId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        void GetSortableListItem(int sortableListItemId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        void SaveListPosition(int sortableListItemId, int newPosition);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        void SaveSortableList(int sortableListId, string listDescription);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        void SaveSortableListItem(int sortableListId, int sortableListItemId, string headline, string description, string linkUrl);
    }
}

