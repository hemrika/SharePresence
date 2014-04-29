using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using Hemrika.SharePresence.Google;
using System.Web;
using System.Diagnostics;
using Hemrika.SharePresence.WebSite.Page;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class MicroDataConnector : UserControl
    {

        private bool _IsSystemPage;

        public bool IsSystemPage
        {
            get { return _IsSystemPage; }
            set { _IsSystemPage = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IsSystemPage = true;

            if (!Request.Url.ToString().Contains("_layouts") && !Request.Url.ToString().Contains("Forms"))
            {
                IsSystemPage = false;
                
            }
        }

        protected string MicroData
        {
            get
            {
                StringBuilder meta = new StringBuilder();

                if (SPContext.Current.ListItem != null)
                {
                }

                return meta.ToString();
            }
        }
    }
}
