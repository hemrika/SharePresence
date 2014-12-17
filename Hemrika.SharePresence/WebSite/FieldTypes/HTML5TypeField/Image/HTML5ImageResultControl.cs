using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Globalization;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using Microsoft.SharePoint.Utilities;
using System.Drawing;
using System.Data;
using System.Collections;
using System.Web;
using Hemrika.SharePresence.WebSite.FieldTypes.HTML5TypeField.Image;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class HTML5ImageResultControl : PickerResultControlBase
    {
        protected Table resultTable;
        protected HTML5ImageDataSet.HTML5ImagesDataTable dt;
        private string selectOnName = "return PickerResultsSingleSelectOnNameClick(this, event);";
        private const string ResultTableId = "resultTable";
        private string[] columns = new string[7] { "Image", "Name", "Dimension", "Alt", "Keywords", "Web", "List" };
        public HTML5ImageResultControl()
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        public new HTML5ImagePickerDialog PickerDialog
        {
            get
            {
                return (HTML5ImagePickerDialog)base.PickerDialog;
            }
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (resultTable == null)
            {
                CreateTable();
            }
            resultTable.RenderControl(writer);
        }

        public override string GenerateResults()
        {
            if (resultTable == null)
            {
                CreateTable();
            }
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            resultTable.RenderControl(writer2);
            return writer.ToString();


        }

        private void CreateTable()
        {
            resultTable = new Table
            {
                Width = Unit.Percentage(100.0),
                ID = "resultTable",
                CssClass = "ms-pickerresulttable",
                CellSpacing = 0,
                CellPadding = 0,
                BackColor = Color.White
            };
            resultTable.ID = "resultTable";
            resultTable.Attributes.Add("hideFocus", "true");
            resultTable.Attributes.Add("EditorControlClientId", PickerDialog.EditorControl.ClientID);

            //Create Table Header Row
            TableRow child = null;

            if (PickerDialog.Results != null)
            {
                dt = PickerDialog.Results;


                child = new TableRow
                {
                    CssClass = "ms-pickerresultheadertr"
                };

                foreach (string column in columns)
                {
                    TableHeaderCell cell = new TableHeaderCell
                    {
                        CssClass = "ms-ph"
                    };
                    cell.Attributes.Add("UNSELECTABLE", "on");
                    Literal literal = new Literal
                    {
                        Text = SPHttpUtility.HtmlEncode(column)
                    };

                    cell.Controls.Add(literal);
                    child.Controls.Add(cell);
                }

                resultTable.Controls.Add(child);


                if (dt.Rows.Count == 0)
                {
                    child = new TableRow
                    {
                        CssClass = "ms-pickeremptyresulttexttr"
                    };

                    TableCell cell2 = new TableCell
                    {
                        CssClass = "ms-descriptiontext",
                        ColumnSpan = columns.Length
                    };
                    Literal literal2 = new Literal
                    {
                        Text = SPHttpUtility.HtmlEncode("No search was given")
                    };
                    cell2.Controls.Add(literal2);
                    child.Controls.Add(cell2);
                    resultTable.Controls.Add(child);
                }
                else
                {
                    int num = 0;
                    string selectSingle = "PickerResultsSingleSelectOnClick(this);";
                    string selectDouble = "PickerResultsSingleSelectOnDblClick(this);";

                    foreach (HTML5ImageDataSet.HTML5ImagesRow row in dt.Rows)
                    {
                        HTML5ImagePickerEntity entity = PickerDialog.QueryControl.GetEntity(row);

                        if (entity != null)
                        {
                            child = new TableRow
                            {
                                ID = string.Format(CultureInfo.InvariantCulture, "row{0}", new object[] { num })
                            };

                            child.Attributes.Add("tabindex", "-1");
                            child.Attributes.Add("resultRow", "resultRow");

                            if ((num % 2) == 0)
                            {
                                child.CssClass = "ms-alternating";
                            }

                            ArrayList entities = new ArrayList();

                            entities.Add(entity);

                            string callback = PickerDialog.EditorControl.GenerateCallbackData(entities, false);
                            child.Attributes.Add("entityXml", callback);
                            child.Attributes.Add("onmousedown", "return singleselectevent(event);");
                            child.Attributes.Add("onclick", selectSingle);
                            child.Attributes.Add("ondblclick", selectDouble);
                            child.Attributes.Add("onkeydown", "PickerResultsNameOnKeyDown(this, event,false);");
                            child.Attributes.Add("onfocus", "PickerResultsNameOnFocus(this, event, false);");

                            child.Attributes.Add("key", entity.Key);

                            for (int i = 0; i < columns.Length; i++)
                            {
                                this.RenderRowColumn(row, i, child);
                            }
                            resultTable.Controls.Add(child);
                            num++;
                        }
                    }

                    if (base.PickerDialog.Results.Rows.Count > 1)
                    {
                        child = new TableRow
                        {
                            CssClass = "ms-pickersearchsummarytr",
                            ID = "PickerSearchResultSummaryRow"
                        };
                        TableCell cell3 = new TableCell
                        {
                            CssClass = "ms-descriptiontext",
                            ColumnSpan = dt.Columns.Count
                        };
                        cell3.Controls.Add(new LiteralControl(SPHttpUtility.HtmlEncode(String.Format("{0} results found.", new object[] { base.PickerDialog.Results.Rows.Count }))));
                        child.Controls.Add(cell3);
                        resultTable.Controls.Add(child);
                    }

                    this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "__SELECTION__HELPER__", "<script type=\"text/javascript\">\r\n                // <![CDATA[\r\n                function " + SPHttpUtility.EcmaScriptStringLiteralEncode(PickerDialog.GetSelectedClientSideFunctionName) + "()\r\n                {\r\n                    var selKeys=new Array(selected.length);\r\n                    for(i=0;i<selected.length;i++)\r\n                    {\r\n                        var uniquekey=selected[i].getAttribute('key').toString();\r\n                        selKeys[i]=uniquekey;\r\n                    }\r\n                    return selKeys;\r\n                }\r\n                // ]]>\r\n                </script>");
                }
            }
        }

        protected override void RenderRowColumn(DataRow row, int iColumn, Control parent)
        {
            base.RenderRowColumn(row as HTML5ImageDataSet.HTML5ImagesRow, iColumn, parent);
        }

        protected void RenderRowColumn(HTML5ImageDataSet.HTML5ImagesRow row, int iColumn, Control parent)
        {
            string str = string.Empty;
            TableCell child = new TableCell
            {
                CssClass = "ms-pb"
            };

            child.Attributes.Add("nowrap", "true");
            child.Attributes.Add("UNSELECTABLE", "on");
            child.Width = new Unit(10);
            Literal literal = new Literal();

            //image
            if ((iColumn == 0))
            {
                string str2 = "false";
                str = "<a class=\"thumbnail\" href='javascript:' onfocus='PickerResultsNameOnFocus(this, event, " + str2 + " );' onkeydown='PickerResultsNameOnKeyDown(this, event," + str2 + ");' onclick='" + this.selectOnName + "' id='resultTable_" + parent.ID + "_Link'>";
                str += "<img width=\"48\" height=\"48\" border='0' src=\"" + row.Preview + "\" alt=\"" + row.Alt + "\"/>";
                str += "<span><img src=\"" + row.Src + "\" alt=\"" + row.Alt + "\"/></span>";
                str += "</a>";
            }

            //Name
            if ((iColumn == 1))
            {
                str = row.Name;
            }

            //Dimension
            if ((iColumn == 2))
            {
                str = row.Height.ToString() + " x " + row.Width.ToString();
            }

            //Alt
            if ((iColumn == 3))
            {
                str = row.Alt;
            }

            //Keywords
            if ((iColumn == 4))
            {
                str = row.Keywords;
            }

            //Web
            if ((iColumn == 5))
            {
                str = row.Web;
            }

            //List
            if ((iColumn == 6))
            {
                str = row.List;
            }

            literal.Text = str;
            child.Controls.Add(literal);
            parent.Controls.Add(child);
        }
    }
}
 