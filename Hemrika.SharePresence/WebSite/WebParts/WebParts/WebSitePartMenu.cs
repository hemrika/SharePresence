// -----------------------------------------------------------------------
// <copyright file="WebSitePartMenu.cs" company="">
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
    using Microsoft.SharePoint;
    using System.IO;
    using System.Reflection;
    using System.Web.UI.WebControls;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.UI.WebControls.WebParts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class WebSitePartMenu : Control
    {
        WebSitePart parentPart;
        ImageButton settingsButton;
        ImageButton closeButton;
        //protected LiteralControl settingsLink;
        //protected LiteralControl propertiesLink;
        //protected LiteralControl closeLink;
        //protected System.Web.UI.WebControls.LinkButton closeLinkButton;

        public WebSitePartMenu(WebSitePart webSitePart)
        {
            parentPart = webSitePart;

            EnsureChildControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page != null && Page.IsPostBack)
            {
                string _target = this.Page.Request["__EVENTTARGET"];
                string _event = this.Page.Request["__EVENTARGUMENT"];

                if (_target == parentPart.ClientID)
                {
                    if (_event == "DeleteWebPart")
                    {
                        SPWebPartManager spwpm = WebPartManager.GetCurrentWebPartManager(Page) as SPWebPartManager;
                        spwpm.DeleteWebPart(this.parentPart);
                    }
                }
            }

        }

        protected override void CreateChildControls()
        {            
            SetCloseLink();
            SetToolPaneLink();
            EnsureChildControls();
        }

        private void SetCloseLink()
        {
            closeButton = new ImageButton();
            closeButton.OnClientClick = String.Format("RemoveWebSitePart('{0}', '{1}', '{2}');return false;", parentPart.ClientID, parentPart.ID.ToString(), this.IdSeparator);
            closeButton.ImageUrl = "_layouts/images/delete.gif";
            closeButton.ImageAlign = ImageAlign.Left;           
            closeButton.AlternateText = "Delete";
            closeButton.ToolTip = "Delete Webpart from page.";
            Controls.Add(closeButton);
            this.CloseWebPart += new CloseWebPartHandler(OnCloseWebPart);
            //closeLink = new LiteralControl();
            //closeLink.Text = String.Format("<img scr=\"_layouts/Hemrika/Content/images/HTML5ImageCheck_16.png\" alt=\"Remove\" onclick=\"RemoveWebSitePart('{0}', '{1}', '{2}');return false;\" style=\"cursor: pointer; padding:2px;\" />", parentPart.ClientID, parentPart.ID.ToString(), this.IdSeparator);
            //Controls.Add(closeLink);
        }

        // Delegate  
        public delegate void CloseWebPartHandler(object sender, EventArgs e);  
        // The event  
        event CloseWebPartHandler CloseWebPart;

        // The method which fires the Event
        void OnCloseWebPart(object sender, EventArgs e)
        {
            SPWebPartManager spwpm = WebPartManager.GetCurrentWebPartManager(Page) as SPWebPartManager;
            spwpm.DeleteWebPart(this.parentPart);
        }

        private void SetToolPaneLink()
        {
            settingsButton = new ImageButton();
            settingsButton.OnClientClick = String.Format("ShowToolPane2Wrapper('Edit',this,'{0}');return false;", parentPart.StorageKey);
            settingsButton.ImageUrl = "_layouts/images/Hemrika/Settings/16/RibbonIcon_Settings_features_site.png";
            settingsButton.ImageAlign = ImageAlign.Left;
            settingsButton.AlternateText = "Settings";
            settingsButton.ToolTip = "Modify settings of Webpart";
            Controls.Add(settingsButton);
        }
    }
}
