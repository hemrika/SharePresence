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
    public class FormContentTemplate : ITemplate, IDisposable
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

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    canvas.Dispose();
                        canvas = null;
                    WebPart.Dispose();
                    WebPart = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FormContentTemplate()
        {
            Dispose(false);
        }
    }
}
