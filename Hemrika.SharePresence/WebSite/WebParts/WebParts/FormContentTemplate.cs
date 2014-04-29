// -----------------------------------------------------------------------
// <copyright file="FormContentTemplate.cs" company="">
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
    using Microsoft.SharePoint.WebPartPages;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormContentTemplate : ITemplate
    {
        internal static string InnerHTML;
        public WebPart WebPart { get; set; }
        internal FormCanvas canvas;

        internal string innerHtml
        {
            get
            {
                return InnerHTML;
            }
            set
            {
                InnerHTML = value;
            }
        }

        public void InstantiateIn(Control container)
        {
            canvas = new FormCanvas(InnerHTML, container);
            canvas.WebPart = WebPart as FormWebPart;
            container.Controls.Add(canvas);
        }
    }
}
