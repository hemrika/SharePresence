// -----------------------------------------------------------------------
// <copyright file="IPageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Master
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMasterPageRepository
    {
        List<MasterPage> GetMasterPages();

        MasterPage GetMasterPage(Guid UniqueId);
        MasterPage GetMasterPage(int Id);
        MasterPage GetMasterPage(string Name);

        String GetMasterPageContent(Guid UniqueId);
        String GetMasterPageContent(int Id);
        String GetMasterPageContent(string Name);
    }
}
