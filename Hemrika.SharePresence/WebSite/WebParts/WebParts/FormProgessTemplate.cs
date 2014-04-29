// -----------------------------------------------------------------------
// <copyright file="FormProgessTemplate.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormProgessTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            LiteralControl tekst = new LiteralControl { Text = "Processing Template" };
            container.Controls.Add(tekst);
        }
    }
}
