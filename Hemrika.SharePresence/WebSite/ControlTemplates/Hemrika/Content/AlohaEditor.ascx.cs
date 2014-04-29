using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Text;
using Hemrika.SharePresence.Common;
using Hemrika.SharePresence.Common.Configuration;
using Hemrika.SharePresence.WebSite.Layout;
using Hemrika.SharePresence.Common.ServiceLocation;
using System.Collections.Generic;
using Hemrika.SharePresence.WebSite.ContentTypes;
using Hemrika.SharePresence.WebSite.Fields;
using Hemrika.SharePresence.Common.UI;
using Hemrika.SharePresence.WebSite.Editor;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class AlohaEditor : UserControl
    {
        public AlohaEditor()
        {
        }

        private EditorSettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new EditorSettings();
            settings = settings.Load();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string commonPath = SPUtility.GetGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\common");
                string commonPath = SPUtility.GetVersionedGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\common",15);
                //string extraPath = SPUtility.GetGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\extra");
                string extraPath = SPUtility.GetVersionedGenericSetupPath(@"Template\Layouts\Hemrika\Editor\plugins\extra",15);
                string[] commons = Directory.GetDirectories(commonPath);
                string[] extras = Directory.GetDirectories(extraPath);

                foreach (string common in commons)
                {
                    DirectoryInfo info = new DirectoryInfo(common);
                    ListItem item = new ListItem(info.Name, info.Name);
                    bool Ischecked = false;
                    if (settings.Common.ContainsKey(info.Name))
                    {
                        bool Succes = bool.TryParse(settings.Common[info.Name].ToString(), out Ischecked);
                        if (!Succes)
                        {
                            Ischecked = Succes;
                        }
                        item.Selected = Ischecked;
                    }

                    item.Attributes.Add("data-role", "none");
                    cbl_Common.Items.Add(item);
                }

                foreach (string extra in extras)
                {
                    DirectoryInfo info = new DirectoryInfo(extra);
                    ListItem item = new ListItem(info.Name, info.Name);
                    bool Ischecked = false;
                    if (settings.Extra.ContainsKey(info.Name))
                    {
                        bool Succes = bool.TryParse(settings.Extra[info.Name].ToString(), out Ischecked);
                        if (!Succes)
                        {
                            Ischecked = Succes;
                        }
                        item.Selected = Ischecked;
                    }

                    item.Attributes.Add("data-role", "none");
                    cbl_Extra.Items.Add(item);
                }
            }
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;
            SPWeb currentWeb = SPContext.Current.Web;

            foreach (ListItem item in cbl_Common.Items)
            {
                if (settings.Common.ContainsKey(item.Value))
                {
                    settings.Common[item.Value] = item.Selected;
                }
                else
                {
                    settings.Common.Add(item.Value, item.Selected);
                }
            }

            foreach (ListItem item in cbl_Extra.Items)
            {
                if (settings.Extra.ContainsKey(item.Value))
                {
                    settings.Extra[item.Value] = item.Selected;
                }
                else
                {
                    settings.Extra.Add(item.Value, item.Selected);
                }
            }

            settings = settings.Save();

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();

        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            SPUtility.Redirect(string.Empty, SPRedirectFlags.UseSource, this.Context);
        }
    }
}