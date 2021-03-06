﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing;
using System.Data;
using System.Web.UI.WebControls;

namespace Hemrika.SharePresence.Google.Visualization
{
    [DefaultProperty("GviShowTip")]
    [ToolboxData("<{0}:GVPieChart runat=server></{0}:GVPieChart>")]
    [ToolboxBitmap(typeof(GVPieChart))]
    public class GVPieChart : BaseWebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public new string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description("Text to display above the chart.")]
        [DefaultValue("Pie Chart")]
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

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description("The background color for the chart.")]
        [DefaultValue("")]
        public Color? GVIBackgroundColor
        {
            get
            {
                Color? s = (Color?)ViewState["GVIBackgroundColor"];
                return s;
            }

            set
            {
                ViewState["GVIBackgroundColor"] = value;
            }
        }

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description("Specifies a base color to use for all series; each series will be a gradation of the color specified. Colors are specified in the Chart API color format. Ignored if colors is specified.")]
        [DefaultValue("")]
        public Color? GviColor
        {
            get
            {
                Color? s = (Color?)ViewState["GviColor"];
                return s;
            }

            set
            {
                ViewState["GviColor"] = value;
            }
        }

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description("Use this to assign specific colors to each data series. Colors are specified in the Chart API color format. Color i is used for data column i, wrapping around to the beginning if there are more data columns than colors. If variations of a single color is acceptable for all series, use the color option instead.")]
        [DefaultValue("")]
        public Color?[] GviColors
        {
            get
            {
                Color?[] s = (Color?[])ViewState["GviColors"];
                return s;
            }

            set
            {
                ViewState["GviColors"] = value;
            }
        }


        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description("Causes charts to throw user-triggered events such as click or mouse over. Supported only for specific chart types. See Events below.")]
        [DefaultValue(false)]
        public bool? GviEnableEvents
        {
            get
            {
                bool? s = (bool?)ViewState["GviEnableEvents"];
                return s;
            }

            set
            {
                ViewState["GviEnableEvents"] = value;
            }
        }

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description("If set to true, displays a three-dimensional chart.")]
        [DefaultValue(false)]
        public bool? GviIs3D
        {
            get
            {
                bool? s = (bool?)ViewState["GviIs3D"];
                return s;
            }

            set
            {
                ViewState["GviIs3D"] = value;
            }
        }

        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description(@"What label, if any, to show for each slice. Choose from the following values:
            'none' - No labels.
            'value' - Use the slice value as a label.
            'name' - Use the slice name (the column name).")]
        [DefaultValue("")]
        public string GviLabels
        {
            get
            {
                string s = (string)ViewState["GviLabels"];
                return s;
            }

            set
            {
                ViewState["GviLabels"] = value;
            }
        }


        [GviConfigOption]
        [Bindable(true)]
        [Category("GoogleOptions")]
        [Description(@"The location of the legend on the chart. Choose from one of the following values: 'top', 'bottom', 'left', 'right', 'none'.")]
        [DefaultValue("")]
        public string GviLegend
        {
            get
            {
                string s = (string)ViewState["GviLegend"];
                return s;
            }

            set
            {
                ViewState["GviLegend"] = value;
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

            this.dt.Rows.Add(new object[] { Name, (decimal) Value });
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
            this.gvi.RegisterGVIScriptsEx(this, this.dt, BaseGVI.GOOGLECHART.PIECHART);
            output.Write(Text);
        }

    }
}
