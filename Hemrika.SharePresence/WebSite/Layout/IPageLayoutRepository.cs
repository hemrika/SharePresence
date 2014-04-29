// -----------------------------------------------------------------------
// <copyright file="IPageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Collections;
using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPageLayoutRepository
    {
        IList<PageLayout> GetPageLayouts();

        PageLayout GetPageLayout(Guid UniqueId);
        PageLayout GetPageLayout(int Id);
        PageLayout GetPageLayout(string Name);
        List<PageLayout> GetPageLayouts(SPContentTypeId contentTypeId);
        String GetPageLayoutContent(Guid UniqueId);
        String GetPageLayoutContent(int Id);
        String GetPageLayoutContent(string Name);
    }
}
