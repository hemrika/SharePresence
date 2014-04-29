using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.WebSite.SiteMap;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Common.UI;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageSiteMapSettings : UserControl
    {
        private SiteMapSettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new SiteMapSettings();
            settings = settings.Load();//SPContext.Current.Site);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbx_index.Checked = settings.UseIndex;
                cbx_mobile.Checked = settings.UseMobile;
                cbx_news.Checked = settings.UseNews;
                cbx_video.Checked = settings.UseVideo;
                cbx_image.Checked = settings.UseImage;
                cbx_image_include.Checked = settings.IncludeImages;

                foreach (string engine in settings.SearchEngines)
                {
                    tbx_ping.Text += engine + ";"+Environment.NewLine;
                }
            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            settings.UseIndex = cbx_index.Checked;
            settings.UseMobile = cbx_mobile.Checked;
            settings.UseNews = cbx_news.Checked;
            settings.UseVideo = cbx_video.Checked;
            settings.UseImage = cbx_image.Checked;
            settings.IncludeImages = cbx_image_include.Checked;

            string[] engines = tbx_ping.Text.Split(new char[]{';'});

            if (engines.Length > 0)
            {
                settings.SearchEngines.Clear();
                foreach (string engine in engines)
                {
                    string seangine = engine.Replace(Environment.NewLine, String.Empty);

                    if (!String.IsNullOrEmpty(seangine))
                    {
                        settings.SearchEngines.Add(seangine);
                    }
                }
            }

            settings = settings.Save();// (SPContext.Current.Site);

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();
        }
    }
}
