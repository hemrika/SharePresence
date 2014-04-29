// -----------------------------------------------------------------------
// <copyright file="INodeManipulator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface INodeManipulator
    {
        List<WebSiteNode> ManipulateNodes(List<WebSiteNode> nodes);
    }
}