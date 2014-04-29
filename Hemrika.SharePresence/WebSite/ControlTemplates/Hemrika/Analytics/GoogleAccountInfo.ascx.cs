using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleAccountInfo : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GooglePageBase google = Page as GooglePageBase;

            //ltl_ProfileID.Text = google.Settings.Current.ProfileId;
            ltl_AccountID.Text = google.Settings.Current.AccountId;
            //ltl_Token.Text = google.Settings.Current.Token;
        }
    }
}
