using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.MetaData;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageMetaDataSettings : UserControl
    {
        private IEnumerable<SPField> metafields;
        private IEnumerable<SPContentType> publishingtypes;
        private MetaDataSettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new MetaDataSettings();
            settings.Load(SPContext.Current.Site);
            btn_Save.Click += new EventHandler(btn_Save_Click);
            //btn_Update.Click += new EventHandler(btn_Update_Click);
            cbx_keywords.CheckedChanged += new EventHandler(cbx_keywords_CheckedChanged);
            rbl_author.SelectedIndexChanged += new EventHandler(cbx_author_SelectedIndexChanged);
            rbl_web_author.SelectedIndexChanged += new EventHandler(cbx_web_author_SelectedIndexChanged);
        }

        void cbx_web_author_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbl_web_author.Items[rbl_web_author.SelectedIndex].Selected = true;
        }

        void cbx_author_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbl_author.Items[rbl_author.SelectedIndex].Selected = true;
        }

        void cbx_keywords_CheckedChanged(object sender, EventArgs e)
        {
            rng_keywords.Enabled = cbx_keywords.Checked;
        }

        void btn_Save_Click(object sender, EventArgs e)
        {
            settings.UseMSNBotDMOZ = cbx_msnbot.Checked;
            settings.AutoKeywords = cbx_keywords.Checked;
            settings.NumberOfkeywords = int.Parse(rng_keywords.Value.ToString());// int.Parse(tbx_keywords.Text);
            settings.UseSiteLanguage = cbx_language.Checked;
            settings.WebAuthorOverride = FindAuthor(rbl_web_author.SelectedItem.Value);
            settings.AuthorOverride = FindAuthor(rbl_author.SelectedItem.Value);
            settings = settings.Save(SPContext.Current.Site);

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();

        }

        void btn_Update_Click(object sender, EventArgs e)
        {
            GetMetaFields();

            SPWebCollection webs = SPContext.Current.Site.AllWebs;

            foreach (SPWeb web in webs)
            {
                web.AllowUnsafeUpdates = true;
                web.Update();

                GetPublishingTypes(web);

                foreach (SPContentType content in publishingtypes)
                {
                    foreach (SPField oField in metafields)
                    {
                        if (!content.Fields.Contains(oField.Id))
                        {
                            string fieldname = SharePointWebControls.GetFieldName(oField);
                            try
                            {
                                SPFieldLink fieldRef = new SPFieldLink(oField);
                                fieldRef.DisplayName = fieldname;
                                fieldRef.Required = false;
                                fieldRef.ShowInDisplayForm = false;
                                fieldRef.Hidden = true;

                                content.FieldLinks.Add(fieldRef);
                                content.Update();
                            }
                            catch { };
                        }
                    }
                }

                SPListCollection lists = web.Lists;
                //foreach (SPList list in web.Lists)
                for (int i = 0; i < lists.Count; i++)
                {
                    GetPublishingTypes(lists[i]);
                    foreach (SPContentType content in publishingtypes)
                    {
                        foreach (SPField oField in metafields)
                        {

                            if (!content.Fields.Contains(oField.Id))
                            {
                                string fieldname = SharePointWebControls.GetFieldName(oField);
                                try
                                {
                                    SPFieldLink fieldRef = new SPFieldLink(oField);
                                    fieldRef.DisplayName = fieldname;
                                    fieldRef.Required = false;
                                    fieldRef.ShowInDisplayForm = false;
                                    fieldRef.Hidden = true;

                                    content.FieldLinks.Add(fieldRef);
                                    content.Update();
                                }
                                catch { };
                            }
                        }
                    }
                    lists[i].Update();
                }

                web.AllowUnsafeUpdates = false;
                web.Update();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbx_msnbot.Checked = settings.UseMSNBotDMOZ;
                cbx_keywords.Checked = settings.AutoKeywords;
                rng_keywords.Enabled = settings.AutoKeywords;
                rng_keywords.Value = float.Parse(settings.NumberOfkeywords.ToString());
                cbx_language.Checked = settings.UseSiteLanguage;

                foreach (Authors author in Enum.GetValues(typeof(Authors)))
                {
                    rbl_author.Items.Add(new ListItem(author.ToString(), author.ToString()));
                    rbl_web_author.Items.Add(new ListItem(author.ToString(), author.ToString()));
                }

                rbl_author.Items.FindByValue(settings.AuthorOverride.ToString()).Selected = true;
                rbl_web_author.Items.FindByValue(settings.WebAuthorOverride.ToString()).Selected = true;
            }
        }

        private Authors FindAuthor(string selected)
        {
            Authors found = Authors.NoOverride;

            foreach (Authors author in Enum.GetValues(typeof(Authors)))
            {
                if (author.ToString() == selected)
                {
                    found = author;
                    break;
                }
            }
            return found;
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
            metafields = fields.Cast<SPField>().Where<SPField>(metagroup);
        }

        private void GetPublishingTypes(SPWeb web)
        {
            Func<SPContentType, bool> contentgroup = null;

            if (contentgroup == null)
            {
                contentgroup = delegate(SPContentType f)
                {
                    return f.Id.Parent == Hemrika.SharePresence.WebSite.ContentTypes.ContentTypeId.PageTemplate;
                };
            }

            SPContentTypeCollection contentypes = web.ContentTypes;
            publishingtypes = contentypes.Cast<SPContentType>().Where<SPContentType>(contentgroup);
        }

        private void GetPublishingTypes(SPList list)
        {
            Func<SPContentType, bool> contentgroup = null;

            if (contentgroup == null)
            {
                contentgroup = delegate(SPContentType f)
                {
                    return f.Id.Parent == Hemrika.SharePresence.WebSite.ContentTypes.ContentTypeId.PageTemplate;
                };
            }

            SPContentTypeCollection contentypes = list.ContentTypes;
            publishingtypes = contentypes.Cast<SPContentType>().Where<SPContentType>(contentgroup);
        }
    }
}
