// -----------------------------------------------------------------------
// <copyright file="IPageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.MetaData.Keywords
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
    public interface IKeywordRepository
    {
        IList<Noise> GetKeywords();
        List<Noise> Keywords();
    }
}
