using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hemrika.SharePresence.WebSite.SiteMap;
using Microsoft.SharePoint;
using Hemrika.SharePresence.Common.UI;
using Hemrika.SharePresence.WebSite.Company;

namespace Hemrika.SharePresence.WebSite.ControlTemplates
{
    public partial class ManageCompanySettings : UserControl
    {
        private CompanySettings settings;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            settings = new CompanySettings();
            settings = settings.Load();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbx_name.Text = settings.Name;
                tbx_street.Text = settings.Street;
                tbx_postalcode.Text = settings.PostalCode;
                tbx_city.Text = settings.City;
                tbx_state.Text = settings.State;
                tbx_country.Text = settings.Country;
                tbx_phone.Text = settings.Phone;
                tbx_email.Text = settings.Email;
                tbx_linkedin.Text = settings.LinkedIn;
                tbx_facebook.Text = settings.FaceBook;
                tbx_twitter.Text = settings.Twitter;
                tbx_stock.Text = settings.Stock;
            }
        }

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            settings.Name = tbx_name.Text;
            settings.Street = tbx_street.Text;
            settings.PostalCode = tbx_postalcode.Text;
            settings.City = tbx_city.Text;
            settings.State = tbx_state.Text;
            settings.Country = tbx_country.Text;
            settings.Phone = tbx_phone.Text;
            settings.Email = tbx_email.Text;
            settings.LinkedIn = tbx_linkedin.Text;
            settings.FaceBook = tbx_facebook.Text;
            settings.Twitter = tbx_twitter.Text;
            settings.Stock = tbx_stock.Text;

            settings = settings.Save();

            var eventArgsJavaScript = String.Format("{{Message:'{0}',controlIDs:window.frameElement.dialogArgs}}", "The Properties have been updated.");

            ((EnhancedLayoutsPage)Page).EndOperation(ModalDialogResult.OK, eventArgsJavaScript);
            Context.Response.Flush();
            Context.Response.End();
        }
    }
}
