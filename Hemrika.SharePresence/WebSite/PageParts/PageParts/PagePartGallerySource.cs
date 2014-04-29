// -----------------------------------------------------------------------
// <copyright file="PagePartGallerySource.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.PageParts
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint;
    using System.Web.UI.WebControls.WebParts;


    [Guid("777bfd6c-0609-4b4c-8f37-72db4bf4209e")]
    public class PagePartGallerySource : WebPartGallerySourceBase
    {
        public PagePartGallerySource(System.Web.UI.Page page) : base(page) {}

        protected override WebPartGalleryItemBase[] GetItemsCore()
        {
            List<PagePartGalleryItem> items = new List<PagePartGalleryItem>();

            SPListItem item = SPContext.Current.ListItem;

            if (item != null)
            {
                foreach (SPField field in item.ContentType.Fields)
                {
                    PagePartGalleryItem ppgi = new PagePartGalleryItem(this, base.Page, field);//, item.ContentType);
                    
                    //if (!items.Contains(ppgi))
                    if(!items.Exists(n => n.Title == ppgi.Title))
                    {
                        items.Add(ppgi);
                    }
                }
            }
            return items.ToArray();
        }

        protected override void AddItemToPage(System.Web.UI.WebControls.WebParts.WebPartZoneBase zone, int zoneIndex, System.Web.UI.WebControls.WebParts.WebPart webPart)
        {
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
            SPWebPartManager spmanager = currentWebPartManager as SPWebPartManager;
            if (spmanager != null)
            {
                webPart = spmanager.AddWebPart(webPart, zone, zoneIndex);
            }
            else
            {
                webPart = currentWebPartManager.AddWebPart(webPart, zone, zoneIndex);
            }
        }

        public override void AddItemToPage(System.Web.UI.WebControls.WebParts.WebPartZoneBase zone, int zoneIndex, WebPartGalleryItem item, string wpid)
        {
            if (zone == null)
            {
                throw new ArgumentNullException("zone");
            }
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            System.Web.UI.WebControls.WebParts.WebPart webPart = ((WebPartGalleryItemBase)item).Instantiate();

            /*
            if (!item.IsSafeAgainstScript)
            {
                SPWebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page) as SPWebPartManager;
                if (!CanUserScript(SPContext.Current.Web))
                {
                    currentWebPartManager.SafeForScriptingManager.ProcessAllowContributorScriptPropertiesRollback(webPart);
                }
            }
            */
            if (!string.IsNullOrEmpty(wpid))
            {
                webPart.ID = wpid;
                webPart.TitleIconImageUrl = webPart.CatalogIconImageUrl;
                webPart.ChromeType = PartChromeType.None;
            }

            this.AddItemToPage(zone, zoneIndex, webPart);
        }

        public override WebPartGalleryItem[] GetItems()
        {
            return GetItemsCore();
            //return base.GetItems();
        }

        /*
        internal static bool CanUserScript(SPWeb web)
        {
            if (!web.AllowContributorsToEditScriptableParts && !web.DoesUserHavePermissions(SPBasePermissions.EmptyMask | SPBasePermissions.AddAndCustomizePages))
            {
                return false;
            }
            return true;
        }
        */
    }
}
