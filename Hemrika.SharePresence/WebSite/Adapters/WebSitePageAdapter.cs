using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;

namespace Hemrika.SharePresence.WebSite.Adapters
{
    public class WebSitePageAdapter : PageAdapter
    {
        public WebSitePageAdapter()
        {
            //this.Page.PreInit += new EventHandler(Page_PreInit);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            /*
            if (SPContext.Current.FormContext.FormMode.Equals(SPControlMode.Display))
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (StringWriter stringWriter = new StringWriter(stringBuilder))
                {
                    using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
                    {
                        base.Render(htmlTextWriter);
                        stringBuilder.Replace("<SCRIPT LANGUAGE='JavaScript'", "<script");
                        stringBuilder.Replace("</SCRIPT>", "</script>");
                        stringBuilder.Replace("type=\"text/javascript\"", "");
                        stringBuilder.Replace("type=\"text/JavaScript\"", "");
                        stringBuilder.Replace("<script", "<script type=\"text/javascript\"");
                        stringBuilder.Replace("language=\"javascript\"", "");
                        stringBuilder.Replace("language=\"JavaScript\"", "");
                        stringBuilder.Replace("defer", "defer=\"defer\"");
                        stringBuilder.Replace("name=\"aspnetForm\" ", "");
                        writer.Write(stringBuilder.ToString());
                    }
                }
            }
            else
            */
                base.Render(writer);
        }
    }
}
