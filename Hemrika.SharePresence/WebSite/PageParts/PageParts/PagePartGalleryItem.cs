using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;

namespace Hemrika.SharePresence.WebSite.PageParts
{
    public class PagePartGalleryItem : WebPartGalleryItemBase, IEqualityComparer<PagePartGalleryItem>
    {
        private readonly SPField field;
        //private readonly SPContentType contenttype;

        public PagePartGalleryItem(WebPartGallerySource source, System.Web.UI.Page page, SPField field) : base(source, page)
        {
            this.field = field;
            //this.contenttype = contenttype;
        }

        public PagePartGalleryItem(WebPartGallerySource source, System.Web.UI.Page page, SPField field,SPContentType contenttype ) : base(source, page)
        {
            this.field = field;
            //this.contenttype = contenttype;
        }

        public override string OnClientAdd
        {
            get
            {
                return base.OnClientAdd;
            }
        }

        public override WebPartGallerySource Source
        {
            get
            {
                return base.Source;
            }
        }

        public override string WebPartContent
        {
            get
            {
                return base.WebPartContent;
            }
            set
            {
                base.WebPartContent = value;
            }
        }

        public override bool IsSafeAgainstScript
        {
            get
            {
                return base.IsSafeAgainstScript;
            }
        }

        public override string RibbonCommand
        {
            get
            {
                return base.RibbonCommand;
            }
        }

        public override string GetPreviewHtml()
        {
            return base.GetPreviewHtml();
        }

        public override string V3PickerKey
        {
            get
            {
                return base.V3PickerKey;
            }
        }

        public override System.Web.UI.WebControls.WebParts.WebPart Instantiate()
        {
            return new PagePart(field);//,contenttype);
        }

        public override string Category
        {
            get { return "Page Parts"; }
        }

        public override string Description
        {
            get { return field.Description; }
        }

        public override string IconUrl
        {
            get
            {
                return "/_layouts/Images/WPICON.GIF";
            }
        }

        public override string Id
        {
            get { return field.Id.ToString(); }
        }

        public override bool IsRecommended
        {
            get { return false; }
        }

        public override string Title
        {
            get { return field.Title;}// +"( " + field.FieldRenderingControl.DisplayTemplateName + " )"; }
        }

        public bool Equals(PagePartGalleryItem x, PagePartGalleryItem y)
        {
            return x.Title == y.Title;
        }

        public int GetHashCode(PagePartGalleryItem obj)
        {
            return obj.GetHashCode();
            //throw new NotImplementedException();
        }
    }
}