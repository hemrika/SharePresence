using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Common.UI;
using Microsoft.SharePoint.Utilities;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class UploadImage : EnhancedControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Request.QueryString.HasKeys() && !(String.IsNullOrEmpty(this.Request.QueryString.Get("webId"))))
                {
                    string webId = SPHttpUtility.HtmlDecode(this.Request.QueryString.Get("webId"));

                    SPSite site = SPContext.Current.Site;
                    SPWeb web = site.OpenWeb(new Guid(webId));
                    GetImageLists(web);
                }
                else
                {
                    GetImageLists(SPContext.Current.Web);
                }

                SetDefaultSelection();
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        //protected void Btn_Save_Click(object sender, ImageClickEventArgs e)
        {
            string filename = UploadImageToLibrary();
            var eventArgsJavaScript = String.Format("{{imageUrl:'{0}',controlIDs:window.frameElement.dialogArgs}}", filename);
            
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        //protected void Btn_Cancel_Click(object sender, ImageClickEventArgs e)
        {
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.Cancel);
        }

        /// <summary>
        /// Returns the URL of the newly uploaded image
        /// </summary>
        /// <param name="uploadLocation">The name of a library in the current web where the image should be uploaded to</param>
        /// <returns></returns>
        private string UploadImageToLibrary()
        {
            if (fup_file.HasFile)
            {
                string[] value = ddl_library.SelectedValue.Split(new string [1]{ "#;" },StringSplitOptions.None);

                var web = SPContext.Current.Site.OpenWeb(new Guid(value[1]));
                var library = web.Lists[new Guid(value[0])];

                //TODO: Handle existing file
                SPFile uploadedFile = library.RootFolder.Files.Add(fup_file.FileName, fup_file.PostedFile.InputStream, true);
                SPListItem uploadeditem = uploadedFile.Item;
                
                //TODO Set ContentType as Picture;
                SPContentTypeId contentypeId = new SPContentTypeId(ddl_contenttype.SelectedValue);
                SPContentType itemtype = library.ContentTypes[contentypeId];
                uploadeditem["ContentTypeId"] = itemtype.Id;
                uploadeditem["ContentType"] = itemtype.Name;

                uploadeditem["Title"] = tbx_title.Text;
                uploadeditem["Keywords"] = tbx_keywords.Text;

                uploadeditem.Update();

                if (web.IsRootWeb)
                {
                    return '/' + uploadedFile.Url.TrimStart('/');
                }
                else
                {
                    return web.ServerRelativeUrl.TrimEnd('/') + '/' + uploadedFile.Url.TrimStart('/');
                }
                
            }
            return String.Empty;
        }

        private void GetImageLists(SPWeb web)
        {
            foreach (SPList list in web.Lists)
            {
                if (list.IsSiteAssetsLibrary || ContainsPictureTypes(list))
                {
                    ListItem item = new ListItem(list.Title+"("+web.Title+")", list.ID.ToString()+"#;"+web.ID.ToString());

                    if (!this.ddl_library.Items.Contains(item))
                    {
                        this.ddl_library.Items.Add(item);
                    }
                }

            }
            if (!web.IsRootWeb)
            {
                GetImageLists(web.ParentWeb);
            }
        }

        private bool ContainsPictureTypes(SPList list)
        {
            bool found = false;

            SPContentTypeId picture = new SPContentTypeId("0x010102");

            foreach (SPContentType ct in list.ContentTypes)
            {
                if (ct.Id == picture || ct.Parent.Id == picture)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public override Common.Ribbon.Definitions.TabDefinition GetTabDefinition()
        {
            return null;
        }

        private void SetDefaultSelection()
        {
            if (this.ddl_library.Items.Count > 0)
            {
                this.ddl_library.SelectedValue = this.ddl_library.Items[0].Value;
                string[] value = ddl_library.SelectedValue.Split(new string[1] { "#;" }, StringSplitOptions.None);

                var web = SPContext.Current.Site.OpenWeb(new Guid(value[1]));
                var library = web.Lists[new Guid(value[0])];
                GetListContentTypes(library);
            }
        }

        private void GetListContentTypes(SPList list)
        {
            SPContentTypeId picture = new SPContentTypeId("0x010102");

            foreach (SPContentType ct in list.ContentTypes)
            {
                if (ct.Id == picture || ct.Id.IsChildOf(picture))
                {
                    ListItem item = new ListItem(ct.Name, ct.Id.ToString());

                    if (!this.ddl_contenttype.Items.Contains(item))
                    {
                        this.ddl_contenttype.Items.Add(item);
                    }
                }
            }

            if (this.ddl_contenttype.Items.Count > 0)
            {
                this.ddl_contenttype.SelectedValue = this.ddl_contenttype.Items[0].Value;
                Btn_Save.Enabled = true;
            }
            else
            {
                Btn_Save.Enabled = false;
            }
        }

        protected void ddl_library_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] value = ddl_library.SelectedValue.Split(new string[1] { "#;" }, StringSplitOptions.None);

            var web = SPContext.Current.Site.OpenWeb(new Guid(value[1]));
            var library = web.Lists[new Guid(value[0])];

            GetListContentTypes(library);
        }

    }
}
