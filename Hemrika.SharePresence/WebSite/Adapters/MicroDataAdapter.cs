// -----------------------------------------------------------------------
// <copyright file="MicroDataAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Hemrika.SharePresence.WebSite.Fields;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MicroDataAdapter : System.Web.UI.Adapters.ControlAdapter
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (SPContext.Current != null && SPContext.Current.FormContext.FormMode == SPControlMode.Display)
            {

            }

            try
            {
                base.Render(writer);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            try
            {
                if (SPContext.Current != null)
                {
                    if (base.Control is HtmlHead)
                    {
                        this.AddMicrodataItemtypeToHeader();
                    }
                    else if (base.Control is HtmlGenericControl)
                    {
                        this.AddMicrodataItemtypeToBodyTag();
                    }
                }
            }
            catch
            {
            }
        }

        private void AddMicrodataItemtypeToBodyTag()
        {
            if ((((HtmlGenericControl)base.Control).TagName.ToLower() == "body"))
            {
                
                HtmlGenericControl control = base.Control as HtmlGenericControl;

                if (SPContext.Current.FormContext.FormMode == SPControlMode.Display )
                {
                    string pageMicrodataItemtype = "http://schema.org/WebPage";

                    if (SPContext.Current.ListItem != null)
                    {
                        SPListItem current = SPContext.Current.ListItem;
                        try
                        {
                            if (current.Fields.Contains(BuildFieldId.SchemaType))
                            {
                                object temp = current[BuildFieldId.SchemaType]; 

                                if(!String.IsNullOrEmpty(temp as string))
                                {
                                    pageMicrodataItemtype = current[BuildFieldId.SchemaType].ToString();
                                    pageMicrodataItemtype = pageMicrodataItemtype.Trim(new char[2] { ';', '#' });
                                }
                            }
                        }
                        catch { };
                    }

                    control.Attributes.Add("itemscope", "itemscope");
                    control.Attributes.Add("itemtype", pageMicrodataItemtype);
                }

                control.Attributes.Add("onload", "if (typeof(_spBodyOnLoadWrapper) != 'undefined') _spBodyOnLoadWrapper();");
            }
        }

        private void AddMicrodataItemtypeToHeader()
        {

        }
    }
}
