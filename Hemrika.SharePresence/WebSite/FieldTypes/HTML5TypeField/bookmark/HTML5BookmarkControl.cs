using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using System.ComponentModel;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.Fields;
using Hemrika.SharePresence.WebSite.Bookmark;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5BookmarkControl runat=server></{0}:HTML5BookmarkControl>")]
    public class HTML5BookmarkControl : BaseFieldControl
    {
        private NumberInput input_Bookmark;
        private HTML5BookmarkField BookmarkField;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Bookmark";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5BookmarkDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (base.ControlMode == SPControlMode.Display && !Page.IsPostBack)
            {
                if (input_Bookmark != null)
                {
                    Page.RegisterRequiresPostBack(this.input_Bookmark);
                }
            }

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;
            base.CreateChildControls();

            BookmarkField = (HTML5BookmarkField)ItemFieldValue;

            if (BookmarkField == null)
            {
                BookmarkField = new HTML5BookmarkField();
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {

            }
            else
            {
                if(!Page.ClientScript.IsClientScriptIncludeRegistered("JQuery"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(HTML5BookmarkControl), "JQuery", "/_layouts/Hemrika/Content/jquery.min.js");
                }

                BookmarkSettings settings = new BookmarkSettings();
                settings.Load();

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("BookMark"))
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(HTML5BookmarkControl), "BookMark", "/_layouts/Hemrika/Content/jquery.bookmark.js");
                }

                if (!Page.ClientScript.IsClientScriptIncludeRegistered("BookMarkExt") && settings.UseExtended)
                {
                    Page.ClientScript.RegisterClientScriptInclude(typeof(HTML5BookmarkControl), "BookMarkExt", "/_layouts/Hemrika/Content/jquery.bookmark.ext.js");
                }

                CssRegistration.Register("/_layouts/Hemrika/Content/bookmark.css");

                input_Bookmark = (NumberInput)this.TemplateContainer.FindControl("BookmarkInput");

                if (input_Bookmark != null)
                {
                    input_Bookmark.TextChanged += new EventHandler(input_Bookmark_TextChanged);

                    input_Bookmark.Value = BookmarkField.Bookmarks;
                    input_Bookmark.Text = BookmarkField.Bookmarks.ToString();


                    HtmlGenericControl Bookmark = new HtmlGenericControl("div");
                    Bookmark.Attributes.Add("class", "hasBookmark");
                    Bookmark.ID = "selectBookmark";
                    this.Controls.Add(Bookmark);

                    if (!Page.ClientScript.IsClientScriptBlockRegistered(Bookmark.ClientID))
                    {
                        StringBuilder script = new StringBuilder();
                        script.Append("$(function () {");
                        script.Append("$(\"" + Bookmark.ClientID + "\").bookmark({");
                        //script.Append("onSelect: customBookmark");

                        if (settings.UseCommon)
                        {
                            script.Append("Sites: $.bookmark.commonSites");
                        }
                        else
                        {
                            if (settings.Sites != null && settings.Sites.Count > 0)
                            {
                                script.Append("Sites:[");

                                foreach (string site in settings.Sites)
                                {
                                    script.Append("'" + site + "',");
                                }
                                script.Append("]");
                                script = script.Replace(",]", "]");
                            }
                        }

                        if (settings.AddEmail)
                        {
                            script.Append(",addEmail: true");

                            if (!string.IsNullOrEmpty(settings.EmailSubject))
                            {
                                script.Append(",emailSubject:'" + settings.EmailSubject + "'");
                            }

                            if (!string.IsNullOrEmpty(settings.EmailBody))
                            {
                                script.Append(",emailBody:'" + settings.EmailBody + "'");
                            }
                        }

                        if (settings.AddFavorite)
                        {
                            script.Append(",addFavorite: true");
                        }
                        if (settings.AddAnalytics)
                        {
                            script.Append(",addAnalytics : true");

                            if (!string.IsNullOrEmpty(settings.AnalyticsName))
                            {
                                script.Append(",analyticsName:'" + settings.AnalyticsName + "'");
                            }
                        }

                        script.Append("});");

                        //script = script.Replace("{}","{sites: $.bookmark.commonSites}");
                        script = script.Replace("{}", "");
                        script.Append("});");

                        Page.ClientScript.RegisterClientScriptBlock(typeof(HTML5BookmarkControl), Bookmark.ClientID, script.ToString(), true);
                    }
                }
            }
        }

        void input_Bookmark_TextChanged(object sender, EventArgs e)
        {
            if ((sender.GetType() == typeof(NumberInput)) && base.ControlMode == SPControlMode.Display)
            {
                input_Bookmark = sender as NumberInput;
                //SPListItem item = this.ListItem;

                url = SPContext.Current.Web.Url;
                //OpenElevatedWeb();

                if (web != null)
                {
                    BookmarkField.Bookmarks += 1;

                    web.AllowUnsafeUpdates = true;
                    SPListItem item = web.GetFile(this.ListItem.UniqueId).Item;
                    item[BuildFieldId.Bookmark] = BookmarkField;
                    item.SystemUpdate(false);
                    web.AllowUnsafeUpdates = false;
                }

                input_Bookmark.Value = BookmarkField.Bookmarks;
                input_Bookmark.Text = BookmarkField.Bookmarks.ToString();
            }
        }

        private SPWeb web = null;
        private string url = null;

        /*
        private void OpenElevatedWeb()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(url, SPUserToken.SystemAccount))
                {
                    site.AllowUnsafeUpdates = true;
                    web = site.OpenWeb();
                }
            });
        }
        */

        public override object Value
        {
            get
            {
                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    this.EnsureChildControls();

                    return BookmarkField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                BookmarkField = (HTML5BookmarkField)value;
                if (input_Bookmark != null)
                {
                        input_Bookmark.Value = BookmarkField.Bookmarks;
                        input_Bookmark.Text = BookmarkField.Bookmarks.ToString();
                }
            }
        }

        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }

        protected int Bookmark
        {
            get
            {
                return BookmarkField.Bookmarks;
            }
        }
    }
}
