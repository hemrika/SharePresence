using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Hemrika.SharePresence.WebSite.Modules.LanguageModule
{
    [ToolboxData("<{0}:LanguageDropDown runat=server></{0}:LanguageDropDown>")]
    public class LanguageDropDown : DropDownList
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            System.Collections.Specialized.StringDictionary dic = new System.Collections.Specialized.StringDictionary();
            System.Collections.Generic.List<string> col = new System.Collections.Generic.List<string>();

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures))
            {
                RegionInfo ri = new RegionInfo(ci.LCID);
                if (!dic.ContainsKey(ri.EnglishName))
                    dic.Add(ri.EnglishName, ri.TwoLetterISORegionName.ToLowerInvariant());

                if (!col.Contains(ri.EnglishName))
                    col.Add(ri.EnglishName);
            }

            col.Sort();

            this.Items.Add(new ListItem("[Not specified]", ""));
            foreach (string key in col)
            {
                this.Items.Add(new ListItem(key, dic[key]));
            }

        }
    }
}