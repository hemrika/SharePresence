// -----------------------------------------------------------------------
// <copyright file="WebSiteAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.Adapters;
    using System.Web.UI;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebSiteAdapter : ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                attributes.Add("action", HttpContext.Current.Request.RawUrl);
                HtmlAttributeWriter attributeWriter = new HtmlAttributeWriter(writer, attributes);
                base.Render(attributeWriter);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
