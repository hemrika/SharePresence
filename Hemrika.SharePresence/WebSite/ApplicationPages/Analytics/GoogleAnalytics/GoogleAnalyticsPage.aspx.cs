using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;

namespace Hemrika.SharePresence.WebSite
{
    public partial class GoogleAnalyticsPage : GooglePageBase
    {
        private string _report = "AccountInfo";
        private const string _reportpath = @"~/_CONTROLTEMPLATES/Hemrika/Analytics/Google{0}.ascx";

        protected override void CreateChildControls()
        {
            string reportPath =string.Empty;
            if (Report != null)
            {
                ltl_Title.Text = String.Format(ltl_Title.Text, Report);
                reportPath = String.Format(_reportpath, Report);
            }
            else
            {
                ltl_Title.Text = String.Format(ltl_Title.Text, _report);
                reportPath = String.Format(_reportpath, _report);
            }

            this.Title = ltl_Title.Text;

            try
            {
                Control report = Page.LoadControl(reportPath);
                pnl_GoogleManage.Controls.Add(report);
            }
            catch (Exception ex)
            {
                lbl_error.Text = ex.Message;
            }
        }
    }
}
