using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Data;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Hemrika.SharePresence.Common.ListRepository;
using Hemrika.SharePresence.WebSite.ContentTypes;
using Hemrika.SharePresence.WebSite.Fields;
using System.IO;
using Hemrika.SharePresence.WebSite.FieldTypes.HTML5TypeField.Video;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    public class HTML5VideoQueryControl : PickerQueryControlBase
    {
        private HTML5VideoDataSet dataset;
        private SPSite site = null;
        private string searchOperator = null;

        public HTML5VideoQueryControl() : base()
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
        public HTML5VideoResultControl ResultControl
        {
            get { return (HTML5VideoResultControl)base.PickerDialog.ResultControl; }
        }

        /// <summary>
        /// Editor control
        /// </summary>
        public HTML5VideoPicker EditorControl
        {
            get { return (HTML5VideoPicker)base.PickerDialog.EditorControl; }
        }

        public new HTML5VideoPickerDialog PickerDialog
        {
            get { return (HTML5VideoPickerDialog)base.PickerDialog; }
        }

        private string[] columns = new string[3] { "Title", "Name", "Keywords" };

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            SPWeb web = SPContext.Current.Site.RootWeb;
            SPContentType video = web.AvailableContentTypes[ContentTypeId.Video];

            int percentage = (int)(100 / video.Fields.Count);

            foreach (SPField field in video.Fields)
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
            return GetEntity(dr as HTML5VideoDataSet.HTML5VideosRow);
        }

        public HTML5VideoPickerEntity GetEntity(HTML5VideoDataSet.HTML5VideosRow dr)
        {
            
            // No datarow provided
            if (dr == null)
                return null;

            // Create new picker
            HTML5VideoPickerEntity entity = new HTML5VideoPickerEntity();
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
            entity.ItemId = dr.VideoId;

            return entity;
        }

        protected override int IssueQuery(string search, string groupName, int pageIndex, int pageSize)
        {
            // Trim search
            search = (search != null) ? search.Trim() : null;

            #region No search provided error

            // Ensure search value
            if (string.IsNullOrEmpty(search))
            {
                PickerDialog.ErrorMessage = "No search provided";
                return 0;
            }

            #endregion

            dataset = new HTML5VideoDataSet();

            GetVideosForWeb(EditorControl.WebId, search, groupName);

            // Return results to dialog
            PickerDialog.Results = dataset.HTML5Videos;
            PickerDialog.ResultControl.PageSize = dataset.HTML5Videos.Rows.Count;

            // Return number of records
            return dataset.HTML5Videos.Rows.Count;

            //return base.IssueQuery(search, groupName, pageIndex, pageSize);
        }

        private void GetVideosForWeb(Guid guid,string search,string groupname)
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
                if (list.IsSiteAssetsLibrary || ContainsVideoTypes(list))
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

                        //Select the videos based upon search.
                        foreach (SPListItem item in items)
                        {
                            try
                            {
                                if (item.ContentTypeId == ContentTypeId.Video || item.ContentTypeId.Parent == ContentTypeId.Video)
                                {
                                    HTML5VideoDataSet.HTML5VideosRow row = dataset.HTML5Videos.NewHTML5VideosRow();
                                    row.VideoId = item.UniqueId;
                                    row.Name = item.Title;

                                    row.Preview = Convert.ToString(item["ows_EncodedAbsUrl"]);
                                    row.Keywords = Convert.ToString(item["ows_Keywords"]);
                                    row.Width = Convert.ToInt32(item[BuildFieldId.Content_Width]);
                                    row.Height = Convert.ToInt32(item[BuildFieldId.Content_Height]);
                                    row.Alt = "";// Convert.ToString(item["ows_Description"]);
                                    row.Web = item.Web.Title;
                                    row.List = item.ParentList.Title;

                                    if (rootweb)
                                    {
                                        row.Src = item.File.ParentFolder.ServerRelativeUrl;
                                    }
                                    else
                                    {
                                        row.Src = Path.Combine("/" + item.Web.ServerRelativeUrl, item.File.ParentFolder.Url);
                                    }

                                    dataset.HTML5Videos.AddHTML5VideosRow(row);
                                    row.AcceptChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }
                    }
                }
            }
            
            if(!web.IsRootWeb)
            {
                GetVideosForWeb(web.ParentWeb.ID,search,groupname);
            }
        }

        private bool ContainsVideoTypes(SPList list)
        {
            bool found = false;

            SPContentTypeId video = ContentTypeId.Video;

            foreach (SPContentType ct in list.ContentTypes)
            {
                if (ct.Id == video || ct.Parent.Id == video)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

    }
}
