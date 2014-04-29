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
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint;
    using System.Web.UI.WebControls.WebParts;


    [Guid("D5A8BBC7-111E-4BF8-A29C-F1760A90F7A6")]
    public class RibbonWebPartGallerySource : WebPartGallerySourceBase
    {
        private const string Fields = "<FieldRef Name=\"Title\"></FieldRef><FieldRef Name=\"WebPartDescription\"></FieldRef><FieldRef Name=\"WebPartPartImageLarge\"></FieldRef><FieldRef Name=\"WebPartAssembly\"></FieldRef><FieldRef Name=\"WebPartTypeName\"></FieldRef><FieldRef Name=\"QuickAddGroups\"></FieldRef><FieldRef Name=\"EncodedAbsUrl\"></FieldRef><FieldRef Name=\"Group\"></FieldRef>";
        private readonly WebPartAdder.Selector[] included = new WebPartAdder.Selector[0];
        private readonly WebPartAdder.Selector[] recommended = new WebPartAdder.Selector[0];

        public string RibbonPostbackId { get; set; }
        public bool catalogRetrieved { get; set; }
        public SPDocumentLibrary catalog { get; set; }

        public RibbonWebPartGallerySource(System.Web.UI.Page page) : base(page) { }

        protected override WebPartGalleryItemBase[] GetItemsCore()
        {
            List<WebPartGalleryItemBase> list = new List<WebPartGalleryItemBase>();
            SPSecurity.SuppressAccessDeniedRedirectInScope scope = new SPSecurity.SuppressAccessDeniedRedirectInScope();
            try
            {
                if (this.Catalog != null)
                {
                    SPQuery query = new SPQuery
                    {
                        ViewFields = "<FieldRef Name=\"Title\"></FieldRef><FieldRef Name=\"WebPartDescription\"></FieldRef><FieldRef Name=\"WebPartPartImageLarge\"></FieldRef><FieldRef Name=\"WebPartAssembly\"></FieldRef><FieldRef Name=\"WebPartTypeName\"></FieldRef><FieldRef Name=\"QuickAddGroups\"></FieldRef><FieldRef Name=\"EncodedAbsUrl\"></FieldRef><FieldRef Name=\"Group\"></FieldRef>"
                    };
                    foreach (SPListItem item in this.Catalog.GetItems(query))
                    {
                        WebPartAdder.Selector[] itemSelectors = GetItemSelectors(item);
                        if (this.ShouldShowItem(itemSelectors))
                        {
                            //XmlReader xmlReader = new XmlTextReader(item.File.OpenBinaryStream());
                            //string errorMessage;
                            //WebPart base2 = WebPartManager.ImportWebPart(xmlReader, out errorMessage) as Microsoft.SharePoint.WebPartPages.WebPart;
                            //if (base2 != null)
                            //{
                            //   list.Add(base2);
                            //}
                            RibbonWebPartGalleryItem galleryitem = new RibbonWebPartGalleryItem(this,this.Page,item, false);
                            list.Add(galleryitem);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }
            return list.ToArray();
        }

        public override WebPartGalleryItem[] GetItems()
        {
            return GetItemsCore();
            //return base.GetItems();
        }

        private SPDocumentLibrary Catalog
        {
            get
            {
                if (!this.catalogRetrieved)
                {
                    this.catalogRetrieved = true;
                    SPSecurity.SuppressAccessDeniedRedirectInScope scope = new SPSecurity.SuppressAccessDeniedRedirectInScope();
                    try
                    {
                        this.catalog = (SPDocumentLibrary)SPContext.Current.Site.GetCatalog(SPListTemplateType.WebPartCatalog);
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    finally
                    {
                        if (scope != null)
                        {
                            scope.Dispose();
                        }
                    }
                }
                return this.catalog;
            }
        }

        private static WebPartAdder.Selector[] GetItemSelectors(SPListItem item)
        {
            string str = item["QuickAddGroups"] as string;
            if (!string.IsNullOrEmpty(str))
            {
                return ParseSelectors(str.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries));
            }
            return new WebPartAdder.Selector[0];
        }

        private static WebPartAdder.Selector[] ParseSelectors(string[] selectorStrings)
        {
            List<WebPartAdder.Selector> list = new List<WebPartAdder.Selector>(selectorStrings.Length);
            foreach (string str in selectorStrings)
            {
                WebPartAdder.Selector selector;
                if (WebPartAdder.Selector.TryParse(str, out selector))
                {
                    list.Add(selector);
                }
            }
            return list.ToArray();
        }

        private bool ShouldShowItem(WebPartAdder.Selector[] selectors)
        {
            if (this.included != null)
            {
                bool flag = false;
                bool flag2 = false;
                foreach (WebPartAdder.Selector selector in this.included)
                {
                    if (!selector.Negated)
                    {
                        flag = true;
                    }
                    foreach (WebPartAdder.Selector selector2 in selectors)
                    {
                        if (selector.PositivelyMatches(selector2))
                        {
                            if (selector.Negated)
                            {
                                return false;
                            }
                            flag2 = true;
                        }
                        else if (selector2.NegativelyMatches(selector))
                        {
                            return false;
                        }
                    }
                }
                if (flag && !flag2)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
