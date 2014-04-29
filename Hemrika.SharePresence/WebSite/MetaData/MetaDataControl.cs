using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.MetaData.Keywords;
using Hemrika.SharePresence.WebSite.ContentTypes;
using System.Web.UI.HtmlControls;
using Hemrika.SharePresence.Common;
using System.Globalization;
using System.Web.UI.Adapters;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.WebSite.Page;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Administration;
using System.Text.RegularExpressions;
using Hemrika.SharePresence.WebSite.FieldTypes;
using Hemrika.SharePresence.WebSite.Fields;

namespace Hemrika.SharePresence.WebSite.MetaData
{
    public partial class MetaDataControl : ControlAdapter
    {
        private IEnumerable<SPField> metafields;
        private IEnumerable<SPField> contentfields;
        private MetaDataSettings settings;
        private FaceBookSettings facebook;
        private TwitterSettings twitter;
        private IEnumerable<Keyword> keywords;

        private SPListItem _listItem;

        public SPListItem listItem
        {
            get { return _listItem; }
            set { _listItem = value; }
        }

        /*
        private SPListItem _Author;

        public SPListItem Author
        {
            get { return _Author; }
            set { _Author = value; }
        }

        private SPListItem _Editor;

        public SPListItem Editor
        {
            get { return _Editor; }
            set { _Editor = value; }
        }
        */
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                SPContext _current = SPContext.Current;

                if (_current == null)
                {
                    _current = SPContext.GetContext(HttpContext.Current);
                }

                settings = new MetaDataSettings();
                settings = settings.Load(_current.Site);

                facebook = new FaceBookSettings();
                facebook = facebook.Load(_current.Site);

                twitter = new TwitterSettings();
                twitter = twitter.Load(_current.Site);

                listItem = _current.ListItem;
                /*
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        SPList userList = _current.Site.RootWeb.SiteUserInfoList;

                        if (userList != null && listItem != null)
                        {
                            SPFieldUserValue userValue = new SPFieldUserValue(_current.Web, listItem[SPBuiltInFieldId.Editor].ToString());
                            SPUser editor = userValue.User;
                            Editor = userList.Items.GetItemById(editor.ID);

                            userValue = new SPFieldUserValue(_current.Web, listItem[SPBuiltInFieldId.Author].ToString());
                            SPUser author = userValue.User;
                            Author = userList.Items.GetItemById(editor.ID);
                        }
                    });
                }
                catch { }
                */

                GetMetaFields();
                
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    UpdateMetaFields();
                }


                if (_current.List == null)
                {
                    HtmlMeta compatible = new HtmlMeta();
                    compatible.HttpEquiv = "X-UA-Compatible";
                    compatible.Content = "IE=edge,chrome=1";
                    this.Control.Controls.Add(compatible);
                    this.Control.Controls.Add(new LiteralControl(Environment.NewLine));

                }
                else
                {
                    string editform = _current.List.Forms[PAGETYPE.PAGE_EDITFORM].ServerRelativeUrl;
                    //string editdialogform = SPContext.Current.List.Forms[PAGETYPE.PAGE_EDITFORMDIALOG].ServerRelativeUrl;
                    string newform = _current.List.Forms[PAGETYPE.PAGE_NEWFORM].ServerRelativeUrl;
                    //string newdialogform = SPContext.Current.List.Forms[PAGETYPE.PAGE_NEWFORMDIALOG].ServerRelativeUrl;

                    bool isEdit = Page.Request.Url.OriginalString.Contains(editform);
                    bool isNew = Page.Request.Url.OriginalString.Contains(newform);

                    bool isSharePresence = false;
                    if (listItem != null && listItem.ContentType != null && listItem.ContentType.Id.IsChildOf(ContentTypeId.PageTemplate))
                    {
                        isSharePresence = true;
                    }

                    if (isSharePresence)
                    {
                        this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                        this.Control.Controls.Add(new LiteralControl("<!-- Managed MetaData -->"));
                        this.Control.Controls.Add(new LiteralControl(Environment.NewLine));

                        HtmlMeta compatible = new HtmlMeta();
                        compatible.HttpEquiv = "X-UA-Compatible";

                        DisplayControlModes modes = WebSitePage.DetermineDisplayControlModes();

                        if (modes.displayMode == SPControlMode.Display && (!isNew && !isEdit))
                        {
                            //compatible.Content = "IE=10";
                            compatible.Content = "IE=edge,chrome=1";
                        }
                        else
                        {
                            if (isNew && isEdit)
                            {
                                compatible.Content = "IE=8";
                            }
                            else
                            {
                                compatible.Content = "IE=edge,chrome=1";
                            }
                        }


                        this.Control.Controls.Add(compatible);
                        this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                        /*
                        string googleId = Editor["Google_x0020_Id"] as string;
                        string twitterId = Editor["Twitter_x0020_Id"] as string;
                        string linkedinId = Editor["LinkedIn_x0020_Id"] as string;
                        string facebookId = Editor["FaceBook_x0020_Id"] as string;
                        */

                        if (SPContext.Current.FormContext.FormMode == SPControlMode.Display)
                        {
                            if (settings.UseMSNBotDMOZ)
                            {
                                HtmlMeta msnbot = new HtmlMeta();
                                msnbot.Name = "msnbot";
                                msnbot.Content = "NOODP";
                                this.Control.Controls.Add(msnbot);
                                this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                            }

                            /*
                            if (!string.IsNullOrEmpty(Editor["Google_x0020_Id"] as string))
                            {
                                HtmlLink googleplus = new HtmlLink();
                                googleplus.Attributes.Add(HtmlTextWriterAttribute.Rel.ToString().ToLower(), "author");
                                googleplus.Attributes.Add(HtmlTextWriterAttribute.Href.ToString().ToLower(), string.Format("https://plus.google.com/{0}/posts", Editor["Google_x0020_Id"] as string));
                                this.Control.Controls.Add(googleplus);
                                this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                            }
                            */

                            Regex mapping = new Regex(@"\[(.*?)\]", RegexOptions.Compiled);

                            foreach (SPField oField in metafields)
                            {
                                string fieldname = SharePointWebControls.GetFieldName(oField);

                                if (listItem.Fields.ContainsField(fieldname))
                                {
                                    string value = listItem.GetFormattedValue(fieldname);// field.Title);
                                    //string value = listItem[fieldname] as string;
                                    HtmlMeta meta = new HtmlMeta();
                                    meta.Name = fieldname;

                                    if (String.IsNullOrEmpty(value))
                                    {
                                        value = oField.DefaultValue;
                                        //meta.Content = oField.DefaultValue;
                                    }
                                    else
                                    {
                                        value = value.Replace(";#",",");
                                        //meta.Content = value;
                                    }

                                    if (!string.IsNullOrEmpty(value) && mapping.IsMatch(value))
                                    {
                                        string shallow = value.ToString();

                                        foreach (Match match in mapping.Matches(shallow))
                                        {
                                            string field = match.Value;
                                            string sfield = field.Trim(new char[2] { '[', ']' });

                                            if (listItem.Fields.ContainsField(sfield))
                                            {
                                                //string fieldvalue = listItem.GetFormattedValue(sfield);
                                                string fieldvalue = listItem[sfield] as string;
                                                

                                                if (string.IsNullOrEmpty(fieldvalue))
                                                {
                                                    try
                                                    {
                                                        SPField internalfield = listItem.Fields.GetFieldByInternalName(sfield);
                                                        if(internalfield != null)
                                                        {
                                                            fieldvalue = internalfield.DefaultValue as string;
                                                        }
                                                        
                                                        //fieldvalue = listItem.Fields[sfield].DefaultValue;
                                                    }
                                                    catch { }
                                                }

                                                if (sfield.ToLower().Contains("author") || sfield.ToLower().Contains("editor") || sfield.ToLower().Contains("publisher"))
                                                {
                                                    /*
                                                    if (sfield.ToLower().Contains("author"))
                                                    {
                                                        fieldvalue = Author["Title"] as string;
                                                    }
                                                    else
                                                    {
                                                        fieldvalue = Editor["Title"] as string;
                                                    }
                                                    */
                                                    /*
                                                    string[] sections = fieldvalue.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                                                    if (sections.Length > 1)
                                                    {
                                                        fieldvalue = sections[1];
                                                    }
                                                    */
                                                    //meta.Content = value;
                                                }

                                                if (sfield.ToLower().Contains("html5"))
                                                {
                                                    if (sfield.ToLower().Contains("image"))
                                                    {
                                                        HTML5Image image = listItem.Fields.GetFieldByInternalName(sfield) as HTML5Image;
                                                        HTML5ImageField ifield = new HTML5ImageField(image.FieldRenderingControl.Value.ToString());
                                                        fieldvalue = listItem.Web.Site.Url + "/" + ifield.Src;
                                                    }

                                                    if (sfield.ToLower().Contains("header"))
                                                    {
                                                        HTML5Header header = listItem.Fields.GetFieldByInternalName(sfield) as HTML5Header;
                                                        HTML5HeaderField ifield = new HTML5HeaderField(header.FieldRenderingControl.Value.ToString());
                                                        fieldvalue = ifield.Text;
                                                    }

                                                    if (sfield.ToLower().Contains("footer"))
                                                    {
                                                        HTML5Footer footer = listItem.Fields.GetFieldByInternalName(sfield) as HTML5Footer;
                                                        HTML5FooterField ifield = new HTML5FooterField(footer.FieldRenderingControl.Value.ToString());
                                                        fieldvalue = ifield.Text;
                                                    }

                                                    if (sfield.ToLower().Contains("video"))
                                                    {
                                                        HTML5Video video = listItem.Fields.GetFieldByInternalName(sfield) as HTML5Video;
                                                        HTML5VideoField ifield = new HTML5VideoField(video.FieldRenderingControl.Value.ToString());
                                                        fieldvalue = listItem.Web.Site.Url + "/" + ifield.Src;
                                                    }

                                                    //fieldvalue = fieldvalue.Replace("//", "/");
                                                }

                                                value = value.Replace(field, fieldvalue);
                                            }
                                            /*
                                            if (sfield.Contains("Twitter_x0020_Id"))
                                            {
                                                value = Editor["Twitter_x0020_Id"] as string;
                                            }

                                            if (sfield.Contains("Google_x0020_Id"))
                                            {
                                                value = Editor["Google_x0020_Id"] as string;
                                            }

                                            if (sfield.Contains("FaceBook_x0020_Id"))
                                            {
                                                value = Editor["FaceBook_x0020_Id"] as string;
                                            }

                                            if (sfield.Contains("LinkedIn_x0020_Id"))
                                            {
                                                value = Editor["LinkedIn_x0020_Id"] as string;
                                            }
                                            */

                                            if (sfield.ToLower().Contains("site"))
                                            {
                                                value = listItem.Web.Site.Url + "/";
                                            }

                                            if (sfield.ToLower().Contains("url"))
                                            {
                                                value = HttpContext.Current.Request.Url.OriginalString;
                                                //value = listItem.Web.Site.Url + "/" + listItem.Url;
                                            }

                                        }
                                    }
                                    
                                    meta.Content = value;

                                    if (fieldname == "keywords")
                                    {
                                        if (settings.AutoKeywords)
                                        {
                                            if (String.IsNullOrEmpty(value))
                                            {
                                                value = String.Empty;
                                            }

                                            meta.Content += GetAutoKeywords(value);
                                        }
                                    }

                                    if (fieldname == "language" && settings.UseSiteLanguage)
                                    {
                                        CultureInfo cult = new CultureInfo((int)SPContext.Current.Web.RegionalSettings.LocaleId);
                                        //CultureInfo cult = new CultureInfo((int)SPContext.Current.Web.Language);
                                        meta.Content = cult.DisplayName.ToLower();

                                        HtmlMeta equiv = new HtmlMeta();
                                        equiv.HttpEquiv = "content-language";
                                        equiv.Content = cult.Name;

                                        this.Control.Controls.Add(equiv);
                                        this.Control.Controls.Add(new LiteralControl(Environment.NewLine));

                                        equiv = new HtmlMeta();
                                        equiv.HttpEquiv = "language";
                                        equiv.Content = cult.Name;

                                        this.Control.Controls.Add(equiv);
                                        this.Control.Controls.Add(new LiteralControl(Environment.NewLine));

                                    }

                                    /*
                                    if (fieldname == "author" && settings.AuthorOverride != Authors.NoOverride)
                                    {
                                        value = listItem[settings.AuthorOverride.ToString()] as string;
                                        meta.Content = value;
                                    }

                                    if (fieldname == "web_author" && settings.WebAuthorOverride != Authors.NoOverride)
                                    {
                                        value = listItem[settings.AuthorOverride.ToString()] as string;
                                        meta.Content = value;
                                    }
                                    */

                                    this.Control.Controls.Add(meta);
                                    this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                                }
                            }
                            this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                        }
                        else
                        {

                            HtmlMeta pragma = new HtmlMeta();
                            pragma.HttpEquiv = "Pragma";
                            pragma.Content = "no-cache";
                            this.Control.Controls.Add(pragma);
                            this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                            HtmlMeta expires = new HtmlMeta();
                            expires.HttpEquiv = "Expires";
                            expires.Content = "-1";
                            this.Control.Controls.Add(expires);
                            this.Control.Controls.Add(new LiteralControl(Environment.NewLine));
                            HtmlMeta store = new HtmlMeta();
                            store.HttpEquiv = "Cache-Control";
                            store.Content = "no-store, no-cache, must-revalidate";
                            this.Control.Controls.Add(store);
                            this.Control.Controls.Add(new LiteralControl(Environment.NewLine));

                            HtmlMeta post = new HtmlMeta();
                            post.HttpEquiv = "Cache-Control";
                            post.Content = "post-check=0, pre-check=0";
                            this.Control.Controls.Add(post);
                            this.Control.Controls.Add(new LiteralControl(Environment.NewLine));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private void UpdateMetaFields()
        {
            try
            {
                bool needsupdate = false;

                if (SPContext.Current.FormContext.FormMode != SPControlMode.Display)
                {

                    foreach (SPField oField in metafields)
                    {
                        string fieldname = SharePointWebControls.GetFieldName(oField);

                        if (listItem != null && !listItem.Fields.ContainsField(fieldname))
                        {
                            needsupdate = true;
                            break;
                        }
                    }

                    if (needsupdate)
                    {
                        SPContext.Current.Web.AllowUnsafeUpdates = true;
                        SPFile currentFile = listItem.File;
                        bool systemcheckout = false;

                        if (currentFile != null && currentFile.Exists)
                        {
                            if (currentFile.CheckOutType == SPFile.SPCheckOutType.None && currentFile.RequiresCheckout)
                            {
                                currentFile.CheckOut();
                                systemcheckout = true;
                            }
                        }

                        foreach (SPField oField in metafields)
                        {
                            string fieldname = SharePointWebControls.GetFieldName(oField);

                            if (!listItem.Fields.ContainsField(fieldname))
                            {
                                listItem.Fields.Add(oField);
                            }

                        }

                        if (currentFile != null && currentFile.Exists)
                        {
                            listItem.Update();
                            currentFile.Update();

                            if (systemcheckout)
                            {
                                currentFile.CheckIn("MetaData Creation Checkin", SPCheckinType.OverwriteCheckIn);
                            }
                        }

                        SPContext.Current.Web.AllowUnsafeUpdates = false;
                    }
                }
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private string GetAutoKeywords(string uservalue)
        {
            StringBuilder content = new StringBuilder();

            GetContentFields();

            foreach (SPField oField in contentfields)
            {
                string fieldname = SharePointWebControls.GetFieldName(oField);
                
                if (listItem.Fields.ContainsField(fieldname))
                {
                    try
                    {
                        SPField field = listItem.Fields.GetField(fieldname);
                        string value = field.GetFieldValueAsText(listItem[fieldname]);
                        //string value = listItem[fieldname] as string;
                        content.Append(value + " ");
                    }
                    catch { };
                }
            }

            WebPartManager webPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);

            if (webPartManager != null)
            {
                foreach (WebPart webpart in webPartManager.WebParts)
                {
                    try
                    {
                        IWebPartMetaData data = (IWebPartMetaData)webpart;
                        if (data != null)
                        {
                            content.Append(data.MetaData() + " ");
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                }
            }

            int providednumber = 0;
            if (!String.IsNullOrEmpty(uservalue))
            {
                providednumber = uservalue.Split(new char[] { ',' }).Length;
            }
            string clean = HtmlRemoval.StripTagsCharArray(content.ToString());
            KeywordAnalyzer analyzer = new KeywordAnalyzer();
            KeywordAnalysis analysis = analyzer.Analyze(clean);
            int numberleft = (settings.NumberOfkeywords - providednumber);

            StringBuilder builder = new StringBuilder();

            if (numberleft > 0)
            {
                keywords = analysis.Keywords.Take(numberleft);

                if (!uservalue.EndsWith(","))
                {
                    builder.Append(", ");
                }

                foreach (Keyword keyword in keywords)
                {
                    builder.AppendFormat("{0}, ", keyword.Word);
                }
            }

            string returnvalue = builder.ToString();
            returnvalue = returnvalue.TrimEnd(new char[] { ',', ' ' });
            returnvalue = returnvalue.TrimStart(new char[] { ',', ' ' });

            return returnvalue;
        }

        private void GetMetaFields()
        {
            Func<SPField, bool> metagroup = null;

            if (metagroup == null)
            {
                metagroup = delegate(SPField f)
                {
                    return f.Group == settings.GroupName;
                };
            }

            SPFieldCollection fields = SPContext.Current.Site.RootWeb.Fields;
            metafields = fields.Cast<SPField>().Where<SPField>(metagroup).OrderBy(field => field.Title);
        }

        private void GetContentFields()
        {
            Func<SPField, bool> contentgroup = null;

            if (contentgroup == null)
            {
                contentgroup = delegate(SPField f)
                {
                    return f.Group != settings.GroupName && f.Hidden == false;
                };
            }

            SPFieldCollection fields = listItem.Fields;
            contentfields = fields.Cast<SPField>().Where<SPField>(contentgroup);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            try
            {

            //writer.Write("<!-- Managed MetaData by http://www.SharePresence.nl -->");
            foreach (Control control in Control.Controls)
            {
                control.RenderControl(writer);
            }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            /*
            SPWeb web = SPContext.Current.Web;
            web.ASPXPageIndexMode = WebASPXPageIndexMode.
            if (web.ASPXPageIndexed == false)
                writer.Write("<META NAME=\"ROBOTS\" CONTENT=\"NOHTMLINDEX,NOINDEX\"/>");
            else
                writer.Write("");     // updated April 11th to fix a WebControls/Controls cast problem.
            */
        }
    }
}
