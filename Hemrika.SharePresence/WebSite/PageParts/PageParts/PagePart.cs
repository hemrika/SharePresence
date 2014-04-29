// -----------------------------------------------------------------------
// <copyright file="PagePart.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.PageParts
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using System.Web.UI;
    using Hemrika.SharePresence.Common;
    using Microsoft.SharePoint.WebControls;
    using System.ComponentModel;
    using Microsoft.SharePoint.WebPartPages;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using Hemrika.SharePresence.WebSite.Page;
    using System.IO;
    using System.Globalization;
    using System.Web;
    using Hemrika.SharePresence.WebSite.WebParts;
    using System.Reflection;
    using System.Web.UI.HtmlControls;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;

    [ToolboxItemAttribute(false)]
    [Guid("eb994ba6-e542-4f51-bdaf-9ab915d06bb2")]
    public class PagePart : WebSitePart, IWebEditable
    {
        protected SPField Field = null;
        //protected SPContentType ContentType = null;

        public PagePart()
        {
            this.ChromeState = PartChromeState.Normal;
            this.ChromeType = PartChromeType.None;
        }

        public PagePart(string name)
        {
            FieldName = name;
        }

        public PagePart(SPField field) //: this(field,null)
        {
            Field = field;
            FieldName = field.Title;
            this.Title = field.Title;
        }

        /*
        public PagePart(SPField field, SPContentType contenttype)
        {
            Field = field;
            //ContentType = contenttype;
            FieldName = field.Title;
        }
        */

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override void CreateWebPartChildControls()
        {
            bool authenticated = HttpContext.Current.User.Identity.IsAuthenticated;
            SPListItem item = SPContext.Current.ListItem;

            try
            {
                int versionId = 0;
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                foreach (string key in queryString.AllKeys)
                {
                    if (key != null && key == "PageVersion")
                    {
                        versionId = int.Parse(queryString[key]);
                        break;
                    }
                }
                if (versionId > 0 && !authenticated)
                {
                    SPListItemVersionCollection versions = item.Versions;

                    foreach (SPListItemVersion version in versions)
                    {
                        if (version.VersionId == versionId)
                        {
                            Field = version.Fields[FieldName];
                        }
                    }
                }
            }
            catch { };

            if (item != null)
            {
                try
                {
                    if (Field == null)
                    {
                        Field = item.Fields[FieldName];
                    }

                    if (Field != null)
                    {
                        BaseFieldControl FieldColumnControl = Field.FieldRenderingControl;
                        if (FieldColumnControl != null)
                        {
                            if (FieldColumnControl.ItemContext.FormContext.FormMode != SPControlMode.Invalid)
                            {
                                FieldColumnControl.ItemContext.FormContext.EnableInputFieldLabels = false;
                                FieldColumnControl.InputFieldLabel = string.Empty;
                                FieldColumnControl.ID = Field.InternalName;
                            }

                            if (inEditMode)
                            {
                                FieldColumnControl.ControlMode = SPControlMode.Edit;
                            }
                            else
                            {
                                FieldColumnControl.ControlMode = SPControlMode.Display;

                            }
                        }
                        this.Controls.Add(FieldColumnControl);
                        this.Title = Field.Title;
                    }
                }
                catch (ArgumentException)
                {
                    if (this.DesignMode)
                    {
                        this.Title = "Unknown PagePart";
                        this.Controls.Add(new LiteralControl("The Field for this PagePart is not found"));
                    }
                }
            }

            //base.CreateWebPartChildControls();
        }

        public override void RenderWebPart(HtmlTextWriter writer)
        {
            bool authenticated = HttpContext.Current.User.Identity.IsAuthenticated;
            SPListItem item = SPContext.Current.ListItem;

            try
            {
                int versionId = 0;
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                foreach (string key in queryString.AllKeys)
                {
                    if (key != null && key == "PageVersion")
                    {
                        versionId = int.Parse(queryString[key]);
                        break;
                    }
                }
                if (versionId > 0 && !authenticated)
                {
                    SPListItemVersionCollection versions = item.Versions;

                    foreach (SPListItemVersion version in versions)
                    {
                        if (version.VersionId == versionId)
                        {
                            Field = version.Fields[FieldName];
                        }
                    }
                }
            }
            catch { };

            if (item != null)
            {
                try
                {
                    if (Field == null)
                    {
                        Field = item.Fields[FieldName];
                    }

                    var builder = new StringBuilder();
                    using (var baseWriter = new HtmlTextWriter(new StringWriter(builder, CultureInfo.InvariantCulture)))
                    {
                        base.RenderWebPart(baseWriter);
                    }

                    string output = builder.ToString();

                    MatchCollection divs = Regex.Matches(output, "<div[^>]*>");
                    Match inputMatch = Regex.Match(output, "<input[^>]*/>");
                    Match navBegin = Regex.Match(output, "<nav[^>]*>");
                    Match navEnd = Regex.Match(output, "</nav>");

                    string input = string.Empty;
                    if (inputMatch.Success)
                    {
                        input = inputMatch.Value;
                    }

                    string nav = string.Empty;
                    if (navBegin.Success && navEnd.Success)
                    {
                        nav = output.Substring(navBegin.Index, ((navEnd.Index + navEnd.Length) - navBegin.Index));
                        //nav = navBegin.Value;
                    }

                    string div = string.Empty;
                    foreach (Match match in divs)
                    {
                       Match name = Regex.Match(match.ToString(), @"(?<=\bid="")[^""]*");
                       if (name.Success)
                       {
                           div = match.Value;
                       }
                    }

                    StringBuilder cleanInput = new StringBuilder();
                    if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(nav) && !string.IsNullOrEmpty(div))
                    {
                        cleanInput.AppendLine(div);
                        cleanInput.AppendLine(input);
                        cleanInput.AppendLine(nav);
                        cleanInput.AppendLine("</div>");
                    }

                    string clean = cleanInput.ToString();

                    if (!string.IsNullOrEmpty(clean) && (Field.Type == SPFieldType.Note || Field.Type == SPFieldType.Text))
                    {
                        writer.Write(clean);
                    }
                    else
                    {
                        writer.Write(output);
                    }
                    /*
                    foreach (var match in divs)
                    {
                        string div = match.ToString();
                        var name = Regex.Match(match.ToString(), @"(?<=\bname="")[^""]*");
                        var internalname = Regex.Match(match.ToString(), @"(?<=\binternalname="")[^""]*");
                        var staticname = Regex.Match(match.ToString(), @"(?<=\bstaticname="")[^""]*");
                    }
                    */

                    /*
                    if (Field.Type == SPFieldType.Note || Field.Type == SPFieldType.Text)
                    {
                        string id = this.Field.FieldRenderingControl.ID;
                    }
                    */
                    /*
                    StringBuilder labelbuilder = new StringBuilder();
                    labelbuilder.Append("<div class=\"ms-formfieldlabelcontainer\" nowrap=\"nowrap\"><span class=\"ms-formfieldlabel\" nowrap=\"nowrap\">");
                    string inputFieldLabel = this.Field.FieldRenderingControl.InputFieldLabel;

                    if (string.IsNullOrEmpty(inputFieldLabel) && (Field != null))
                    {
                        inputFieldLabel = Field.Title;
                    }
                    if (string.IsNullOrEmpty(inputFieldLabel))
                    {
                        inputFieldLabel = Field.FieldRenderingControl.FieldName;
                    }
                    labelbuilder.Append(inputFieldLabel);
                    labelbuilder.Append("</span></div>");
                    output = output.Replace(labelbuilder.ToString(), String.Empty);
                    */

                    
                }
                catch (ArgumentException)
                {
                    if (this.DesignMode)
                    {
                        base.RenderWebPart(writer);//.Render(writer);
                    }
                }
            }
        }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        [WebPartStorage(Storage.Shared)]
        [DisplayName("FieldName")]
        public String FieldName { get; set; }

        EditorPartCollection IWebEditable.CreateEditorParts()
        {

            List<EditorPart> editors = new List<EditorPart>();

            EditorPartCollection baseParts = base.CreateEditorParts();

            foreach (EditorPart basePart in baseParts)
            {
                editors.Add(basePart);
            }

            PageEditorPart editor = new PageEditorPart
            {
                ID = ID + "_Editor",
                Title = "Page Editor Part"
            };

            editors.Add(editor);

            return new EditorPartCollection(editors);

        }

        object IWebEditable.WebBrowsableObject
        {

            get { return this; }

        }
    }
}