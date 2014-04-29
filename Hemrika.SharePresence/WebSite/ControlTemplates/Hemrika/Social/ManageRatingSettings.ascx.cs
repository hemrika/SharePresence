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
using Hemrika.SharePresence.WebSite.Rating;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageRatingSettings : UserControl
    {
        //private IEnumerable<SPField> metafields;
        //private IEnumerable<SPContentType> publishingtypes;
        private RatingSettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new RatingSettings();
            settings = settings.Load();
            btn_Save.Click += new EventHandler(btn_Save_Click);
        }

        void btn_Save_Click(object sender, EventArgs e)
        {
            settings.Maximum = int.Parse(rtg_maximum.Value.ToString());
            settings.Minimum = int.Parse(rtg_minimum.Value.ToString());
            settings = settings.Save();// (SPContext.Current.Site);

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            rtg_maximum.Value = settings.Maximum;
            rtg_minimum.Value = settings.Minimum;
        }
    }
}
