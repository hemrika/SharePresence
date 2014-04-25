using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing;
using System.Data;

namespace Hemrika.SharePresence.Google.Visualization
{
    [DefaultProperty("GviShowTip")]
    [ToolboxData("<{0}:GVTable runat=server></{0}:GVTable>")]
    [ToolboxBitmap(typeof(GVTable))]
    public class GVTable : BaseWebControl
    {

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description(@"Text to display above the table.")]
        [DefaultValue("")]
        public string GviTitle
        {
            get
            {
                string s = (string)ViewState["GviTitle"];
                return s;
            }

            set
            {
                ViewState["GviTitle"] = value;
            }
        }

        public void ChartData(string Name, int Value)
        {

            if ((this.dt == null) || (this.dt.Columns.Count == 0))
            {
                this.dt = new DataTable();
                this.dt.Columns.Add("Name", typeof(String));
                this.dt.Columns.Add("Value", typeof(decimal));
            }

            this.dt.Rows.Add(new object[] { Name, (decimal)Value });
        }
        public void ChartData(string Name, decimal Value)
        {
            if ((this.dt == null) || (this.dt.Columns.Count == 0))
            {
                this.dt = new DataTable();
                this.dt.Columns.Add("Name", typeof(String));
                this.dt.Columns.Add("Value", typeof(decimal));
            }

            this.dt.Rows.Add(new object[] { Name, Value });
        }
        protected override void RenderContents(HtmlTextWriter output)
        {
            this.GviTitle = string.IsNullOrEmpty(this.GviTitle) ? this.dt.TableName : this.GviTitle;
            this.gvi.RegisterGVIScriptsEx(this, this.dt, BaseGVI.GOOGLECHART.TABLE);
            output.Write(Text);
        }
    }
}
