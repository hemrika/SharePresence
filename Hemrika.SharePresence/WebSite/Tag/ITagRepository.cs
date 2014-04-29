// -----------------------------------------------------------------------
// <copyright file="IPageLayoutRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Tag
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITagRepository
    {
        List<Tag> GetTags();
        /*
        Tag GetTag(Guid UniqueId);
        Tag GetTag(int Id);
        Tag GetTag(string Name);
        */
    }
}
