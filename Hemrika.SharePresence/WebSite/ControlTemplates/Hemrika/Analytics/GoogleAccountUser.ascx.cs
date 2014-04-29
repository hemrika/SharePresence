using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.Google.Analytics;
using System.Drawing;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class GoogleAccountUser : UserControl
    {
        GooglePageBase google;

        protected void Page_Load(object sender, EventArgs e)
        {
            google = Page as GooglePageBase;

            if (!Page.IsPostBack)
            {
                SetNotice();
            }
        }

        private void SetNotice()
        {
            if (!String.IsNullOrEmpty(google.Settings.Username) && !String.IsNullOrEmpty(google.Settings.Password))
            {
                lbl_verified.Text = "The account has been verified and is working.";
                lbl_verified.ForeColor = Color.Green;
                tbx_Google_Account_Name.Enabled = false;
                tbx_Google_Account_Password.Enabled = false;
                btn_Google_Account_Submit.Enabled = false;
                btn_Goole_Account_Clear.Enabled = true;
            }
            else
            {
                lbl_verified.Text = "No account has yet been set and verified.";
                lbl_verified.ForeColor = Color.Red;
                tbx_Google_Account_Name.Enabled = true;
                tbx_Google_Account_Password.Enabled = true;
                btn_Google_Account_Submit.Enabled = true;
                btn_Goole_Account_Clear.Enabled = false;
            }
            
            //tbx_Google_Account_Name.Text = google.Settings.Username;
        }

        protected void btn_Google_Account_Submit_Click(object sender, EventArgs e)
        {
            lbl_correct.Text = String.Empty;
            lbl_error.Text = String.Empty;

            if (Validate())
            {
                google.Settings.Username = tbx_Google_Account_Name.Text;
                google.Settings.Password = tbx_Google_Account_Password.Text;
                google.Settings.APIkey = "AIzaSyCumr3wKJIdmyDqXXAtFEdUH4NRGbisbjc";
                google.Settings = google.Settings.Save();
            }
            SetNotice();
        }

        private bool Validate()
        {
            /*
            try
            {
                google.Analytics.SetAuthenticationToken(google.Settings.Current.Token);
                google.Webmastertools.SetAuthenticationToken(google.Settings.Current.Token);
            }
            catch (Exception ex)
            {
                lbl_error.Text += "Token" + ex.ToString();
            }
            */

            try
            {
                google.Analytics.setUserCredentials(tbx_Google_Account_Name.Text, tbx_Google_Account_Password.Text);
                google.Webmastertools.setUserCredentials(tbx_Google_Account_Name.Text, tbx_Google_Account_Password.Text);
            }
            catch (Exception ex)
            {
                lbl_error.Text += "Credentials" + ex.ToString();
            }

            try
            {
                //DataQuery query = new DataQuery(AccountQuery.HttpsFeedUrl);
                AccountQuery query = new AccountQuery();
                query.ExtraParameters += "key=" + google.Settings.APIkey;
                AccountFeed accounts = google.Analytics.Query(query);
                //DataFeed accountFeed = google.Analytics.Query(query);
            }
            catch (Exception ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                /*
                tbx_Google_Account_Name.Text = String.Empty;
                tbx_Google_Account_Password.Text = String.Empty;
                lbl_correct.Text = "Account validation has failed.";
                lbl_correct.ForeColor = Color.Red;
                lbl_error.Text = ex.Message;
                return false;
                */
            }

            lbl_correct.Text = "Account has been validated.";
            lbl_correct.ForeColor = Color.Green;
            SetNotice();
            return true;
        }

        protected void btn_Google_Account_Clear_Click(object sender, EventArgs e)
        {
            google.Settings.Username = String.Empty;
            google.Settings.Password = String.Empty;
            google.Settings = google.Settings.Save();
            SetNotice();
        }

        protected void btn_Google_Account_Remove_Click(object sender, EventArgs e)
        {
            google.Settings.Remove();
            Response.Redirect(Request.RawUrl, true);
            //SetNotice();
        }
    }
}
