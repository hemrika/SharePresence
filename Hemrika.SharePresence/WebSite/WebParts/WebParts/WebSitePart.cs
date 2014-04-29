// -----------------------------------------------------------------------
// <copyright file="WebSitePart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.WebParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls.WebParts;
    using System.Web;
    using System.Web.UI;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebPartPages;
    using System.Reflection;
    using System.ComponentModel;
    using System.IO;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using Hemrika.SharePresence.Html5.WebControls;
    using Hemrika.SharePresence.Common.UI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebSitePart : EnhancedWebPart, IWebPart, IWebActionable
    {
        //protected LiteralControl editLink;
        protected Nav menuControls;

        public WebSitePart()
        {
            menuControls = new Nav();
            menuControls.Attributes.Add("class", "webpartmenu");
            menuControls.Attributes.Add("parent", this.ID);
            menuControls.Attributes.Add("align", "right");
            this.Load += new EventHandler(WebSitePart_LoadNavigation);
        }

        private void WebSitePart_LoadNavigation(object sender, EventArgs e)
        {
            if (inEditMode)
            {
                CreateWebSitePartMenu();
                WebSitePartMenu menu = new WebSitePartMenu(this);
                menuControls.Controls.Add(menu);
                ScriptLink.Register(this.Page, "Hemrika/WebSitePart/Hemrika.SharePresence.WebSitePart.js", false, true);
                this.Controls.Add(menuControls);
                this.EnsureChildControls();
            }

        }

        public bool inEditMode
        {
            get
            {
                SPWebPartManager currentWebPartManager = null;
                if (WebPartManager != null)
                {
                    currentWebPartManager = (SPWebPartManager)WebPartManager.GetCurrentWebPartManager(this.Page);
                }

                return (((currentWebPartManager != null) && !base.IsStandalone) && currentWebPartManager.GetDisplayMode().AllowPageDesign);
            }
        }

        public override void CreateWebPartChildControls()
        {
            /*
            if (inEditMode)
            {
                ScriptLink.Register(this.Page, "Hemrika/WebSitePart/Hemrika.SharePresence.WebSitePart.js", false, true);
                this.Controls.Add(menuControls);
            }
            */
            base.CreateWebPartChildControls();
        }

        public override void CreateWebPartMenu()
        {
            this.WebPartMenu.MenuItems.Clear();
            //this.Controls.Add(menuControls);
            this.EnsureChildControls();

        }

        public virtual void CreateWebSitePartMenu()
        {
        }

        public override Common.Ribbon.Definitions.ContextualGroupDefinition GetContextualGroupDefinition()
        {
            return null;
        }
    }
}
