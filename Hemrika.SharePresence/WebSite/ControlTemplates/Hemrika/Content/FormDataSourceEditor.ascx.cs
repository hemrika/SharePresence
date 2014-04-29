using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Common.UI;
using Hemrika.SharePresence.WebSite.WebParts;
using Hemrika.SharePresence.Common.TemplateEngine;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using Microsoft.SharePoint;
using System.Collections.Generic;


namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class FormDataSourceEditor : UserControl
    {
        protected static DesignDataSource source;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Read Query for DataSource

            if (!IsPostBack)
            {
                try
                {
                    StringBuilder result = new StringBuilder();

                    result.Append("{");
                    NameValueCollection parameters = Request.QueryString;

                    foreach (string key in parameters.AllKeys)
                    {
                        if (!string.IsNullOrEmpty(parameters[key]))
                        {
                            string value = parameters[key];
                            bool done = false;

                            int intresult;
                            if (int.TryParse(value, out intresult))
                            {
                                result.AppendFormat("{0}:{1},", key, Utilities.ConvertToJs(intresult));// parameters[key]);
                                done = true;
                            }

                            bool boolresult;
                            if (bool.TryParse(value, out boolresult))
                            {
                                result.AppendFormat("{0}:{1},", key, Utilities.ConvertToJs(boolresult));// parameters[key]);
                                done = true;
                            }

                            if (!done)
                            {
                                result.AppendFormat("{0}:{1},", key, Utilities.ConvertToJs(value));// parameters[key]);
                            }
                        }
                    }
                    if (parameters.Count > 0)
                    {
                        result.Remove(result.Length - 1, 1);
                    }

                    result.Append("}");

                    source = Utilities.DeserializeObject<DesignDataSource>(result.ToString());

                    PopulateAndSync();
                }
                catch (Exception)
                {
                    source = new DesignDataSource();
                }

                
            }
            #endregion
        }

        private void PopulateAndSync()
        {
            SPSite site = SPContext.Current.Site;
            SPWeb web = site.RootWeb;
            SPWebCollection webs = site.AllWebs;
            SPList list = null;
            //SPListCollection lists = null;
            List<SPList> lists = new List<SPList>();
            SPView view = null;
            SPViewCollection views = null;
            //SPFieldCollection fields = null;
            SPViewFieldCollection viewfields = null;

            if (!string.IsNullOrEmpty(source.Title))
            {

                tbx_name.Text = source.Title;
            }
            #region Webs
            
            ddl_webs.DataSource = webs;
            ddl_webs.DataTextField = "Title";
            ddl_webs.DataValueField = "ID";

            if (!string.IsNullOrEmpty(source.WebId))
            {
                try
                {
                    Guid WebId = new Guid(source.WebId);
                    web = site.OpenWeb(WebId);
                    ddl_webs.SelectedValue = web.ID.ToString();
                    
                }
                catch (Exception)
                {
                    web = site.RootWeb;
                }
                
            }

            //lists = web.Lists;

            foreach (SPList _list in web.Lists)
            {
                if (!_list.Hidden)
                {
                    if (_list.BaseType == SPBaseType.DocumentLibrary)
                    {
                        if (!((SPDocumentLibrary)_list).IsCatalog)
                        {
                            lists.Add(_list);
                        }
                    }
                    else
                    {
                        lists.Add(_list);
                    }
                }
            }

            ddl_webs.DataBind();

            #endregion
            
            #region Lists

            ddl_lists.DataSource = lists;
            ddl_lists.DataTextField = "Title";
            ddl_lists.DataValueField = "ID";

            if (!string.IsNullOrEmpty(source.ListId))
            {
                try
                {
                    Guid ListId = new Guid(source.ListId);
                    list = web.Lists.GetList(ListId, true);
                    views = list.Views;
                    ddl_lists.SelectedValue = list.ID.ToString();
                }
                catch (Exception)
                {
                    list = lists[0];
                    ddl_lists.SelectedValue = list.ID.ToString();
                    views = lists[0].Views;
                }
            }

            if (list == null)
            {
                list = lists[0];
            }
            if (views == null)
            {
                views = list.Views;
            }
            ddl_lists.DataBind();
            ddl_lists.SelectedValue = list.ID.ToString();

            #endregion

            #region Views

            ddl_views.DataSource = views;
            ddl_views.DataTextField = "Title";
            ddl_views.DataValueField = "ID";

            if (!string.IsNullOrEmpty(source.ViewId) && list != null)
            {

                try
                {
                    Guid ViewId = new Guid(source.ViewId);
                    view = list.Views[ViewId];
                    viewfields = view.ViewFields;
                    ddl_views.SelectedValue = view.ID.ToString();
                }
                catch (Exception)
                {                 
                    view = list.Views[0];
                }

                
            }

            if (view == null)
            {
                view = list.Views[0];
            }

            if (viewfields == null)
            {
                viewfields = view.ViewFields;
            }

            ddl_views.DataBind();
            ddl_views.SelectedValue = view.ID.ToString();

            #endregion

            #region Fields

            cbl_fields.DataSource = viewfields;

            if (!string.IsNullOrEmpty(source.ViewFields) && view != null)
            {
                string[] _fields = source.ViewFields.Split(new char[1] { ';' });
                
            }

            cbl_fields.DataBind();

            #endregion

            #region CAML

            if (!string.IsNullOrEmpty(source.Query) && view != null)
            {
                tbx_caml.Text = view.Query;
            }
            else
            {
                if (view != null)
                {
                    tbx_caml.Text = view.Query;
                }
            }

            #endregion
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(source.Id))
            {
                source.Id = DateTime.Now.Ticks.ToString();
            }

            source.Title = tbx_name.Text;
            if (string.IsNullOrEmpty(source.Title))
            {
                source.Title = ddl_lists.SelectedItem.Text + " - " + ddl_views.SelectedItem.Text;
            }

            source.ListId = ddl_lists.SelectedValue;
            source.Query = tbx_caml.Text;
            source.ViewId = ddl_views.SelectedValue;
            source.WebId = ddl_webs.SelectedValue;
            source.Query = tbx_caml.Text;
            source.RecursiveScope = true;
            source.CacheOnClient = true;
            source.RowLimit = 5;

            StringBuilder fields = new StringBuilder();
            foreach (ListItem item in cbl_fields.Items)
            {
                if (item.Selected)
                {
                    fields.Append(item.Text + ";");
                }
            }
            if (fields.Length > 1)
            {
                fields = fields.Remove(fields.Length - 1, 1);
            }
            source.ViewFields = fields.ToString();

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, source.ConvertToJson());
            Context.Response.Flush();
            Context.Response.End();
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.Cancel);
        }

        protected void ddl_webs_SelectedIndexChanged(object sender, EventArgs e)
        {
            source.WebId = ddl_webs.SelectedValue;
            PopulateAndSync();
        }

        protected void ddl_lists_SelectedIndexChanged(object sender, EventArgs e)
        {
            source.ListId = ddl_lists.SelectedValue;
            PopulateAndSync();
        }

        protected void ddl_views_SelectedIndexChanged(object sender, EventArgs e)
        {
            source.ViewId = ddl_views.SelectedValue;
            PopulateAndSync();
        }

        protected void cbl_fields_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
