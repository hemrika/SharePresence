using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Data;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.Common.ListRepository;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5ImageQueryControl : PickerQueryControlBase
    {
        private HTML5ImageDataSet dataset;
        private SPSite site = null;
        private string searchOperator = null;

        public HTML5ImageQueryControl() : base()
        {
        }

        /// <summary>
        /// OnPreRender method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        /// <summary>
        /// Result Control
        /// </summary>
        public HTML5ImageResultControl ResultControl
        {
            get { return (HTML5ImageResultControl)base.PickerDialog.ResultControl; }
        }

        /// <summary>
        /// Editor control
        /// </summary>
        public HTML5ImagePicker EditorControl
        {
            get { return (HTML5ImagePicker)base.PickerDialog.EditorControl; }
        }

        public new HTML5ImagePickerDialog PickerDialog
        {
            get { return (HTML5ImagePickerDialog)base.PickerDialog; }
        }

        private string[] columns = new string[3] { "Title", "Name", "Keywords" };

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            SPWeb web = SPContext.Current.Site.RootWeb;
            SPContentType picture = web.AvailableContentTypes[SPBuiltInContentTypeId.Picture];

            int percentage = (int)(100 / picture.Fields.Count);

            foreach (SPField field in picture.Fields)
            {
                string title = field.Title;

                if (columns.Contains(title) && (ColumnList.Items.FindByText(field.Title) ==null))
                {
                    ColumnList.Items.Add(new ListItem(field.Title, field.Id.ToString()));
                }
            }
        }

        public override PickerEntity GetEntity(DataRow dr)
        {
            return GetEntity(dr as HTML5ImageDataSet.HTML5ImagesRow);
        }

        public HTML5ImagePickerEntity GetEntity(HTML5ImageDataSet.HTML5ImagesRow dr)
        {
            
            // No datarow provided
            if (dr == null)
                return null;

            // Create new picker
            HTML5ImagePickerEntity entity = new HTML5ImagePickerEntity();
            entity.Key = dr.Src;
            entity.DisplayText = dr.Src;
            entity.Alt = dr.Alt;
            entity.Src = dr.Src;
            entity.Height = dr.Height;           
            entity.Width = dr.Width;
            entity.Description = dr.Alt;
            entity.Web = dr.Web;
            entity.List = dr.List;
            entity.Preview = dr.Preview;
            entity.IsResolved = true;
            entity.ItemId = dr.ImageId;

            return entity;
        }

        protected override int IssueQuery(string search, string groupName, int pageIndex, int pageSize)
        {
            SPWeb web = SPContext.Current.Site.OpenWeb(EditorControl.WebId);
            string group = web.Fields[new Guid(groupName)].Title;
            // Trim search
            search = (search != null) ? search.Trim() : null;
            List<string> searches = new List<string>();

            //If the Search has Slashes and groupName is Title or Name
            if (search.Contains('/') && (group.Equals("Title") || group.Equals("Name")))
            {
                searches.AddRange(search.Split(new char[1] {'/'}));
            }

            if (search.Contains(',') && group.Equals("Keywords"))
            {
                searches.AddRange(search.Split(new char[1] { ',' }));
            }

            if (searches.Count == 0)
            {
                searches.Add(search);
            }

            #region No search provided error

            // Ensure search value
            if (string.IsNullOrEmpty(search))
            {
                PickerDialog.ErrorMessage = "No search provided";
                return 0;
            }

            #endregion

            dataset = new HTML5ImageDataSet();

            foreach (string term in searches)
            {
                string noextension = term.Split(new char[1] { '.' })[0];
                GetImagesForWeb(EditorControl.WebId, noextension.Trim(), groupName);    
            }
            

            // Return results to dialog
            PickerDialog.Results = dataset.HTML5Images;
            PickerDialog.ResultControl.PageSize = dataset.HTML5Images.Rows.Count;

            // Return number of records
            return dataset.HTML5Images.Rows.Count;

            //return base.IssueQuery(search, groupName, pageIndex, pageSize);
        }

        private void GetImagesForWeb(Guid guid,string search,string groupname)
        {
            if(site == null)
            {
                site = SPContext.Current.Site;
            }

            SPWeb web = site.RootWeb;

            if (guid != Guid.Empty)
            {
                web = site.OpenWeb(guid);
            }
            bool rootweb = web.IsRootWeb;

            foreach (SPList list in web.Lists)
            {
                if (list.IsSiteAssetsLibrary || ContainsPictureTypes(list))
                {
                    SPField searchField = list.Fields[new Guid(groupname)];
                    SPListItemCollection items = null;

                    if (!string.IsNullOrEmpty(search))
                    {
                        SPQuery query = new SPQuery();

                        string valueType = searchField.TypeAsString;
                        if (searchField.Type == SPFieldType.Calculated)
                            valueType = "Text";

                        query.ViewAttributes = "Scope=\"Recursive\"";
                        query.Query = string.Format("<Where><{0}><FieldRef ID=\"{1}\"/><Value Type=\"{2}\">{3}</Value></{0}></Where>"
                            , searchOperator ?? "Contains"
                            , searchField.Id.ToString()
                            , valueType
                            , search);

                        items = list.GetItems(query);

                        //Select the images based upon search.
                        foreach (SPListItem item in items)
                        {
                            if (!dataset.HTML5Images.Rows.Contains(item.UniqueId))
                            {
                                HTML5ImageDataSet.HTML5ImagesRow row = dataset.HTML5Images.NewHTML5ImagesRow();
                                row.ImageId = item.UniqueId;
                                row.Name = item.Name;

                                row.Preview = Convert.ToString(item["ows_EncodedAbsThumbnailUrl"]);
                                row.Keywords = Convert.ToString(item["ows_Keywords"]);
                                row.Width = Convert.ToInt32(item["ows_ImageWidth"]);
                                row.Height = Convert.ToInt32(item["ows_ImageHeight"]);
                                row.Alt = Convert.ToString(item["ows_Description"]);
                                row.Web = item.Web.Title;
                                row.List = item.ParentList.Title;

                                if (rootweb)
                                {
                                    row.Src = "/" + item.File.Url;
                                }
                                else
                                {
                                    row.Src = "/" + item.Web.ServerRelativeUrl + "/" + item.File.Url;
                                }

                                dataset.HTML5Images.AddHTML5ImagesRow(row);
                                row.AcceptChanges();
                            }
                        }
                    }
                }
            }
            
            if(!web.IsRootWeb)
            {
                GetImagesForWeb(web.ParentWeb.ID,search,groupname);
            }
        }

        private bool ContainsPictureTypes(SPList list)
        {
            bool found = false;

            SPContentTypeId picture = new SPContentTypeId("0x010102");

            foreach (SPContentType ct in list.ContentTypes)
            {
                if (ct.Id == picture || ct.Id.IsChildOf(picture))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

    }
}
