using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Hemrika.SharePresence.Html5.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using System.ComponentModel;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.Fields;
using Hemrika.SharePresence.WebSite.Rating;
using System.Web;
using System.Globalization;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [ToolboxData("<{0}:HTML5RatingControl runat=server></{0}:HTML5RatingControl>")]
    public class HTML5RatingControl : BaseFieldControl, IPostBackEventHandler, IPostBackDataHandler
    {
        protected RatingInput input_rating;
        private HTML5RatingField RatingField;
        private RatingSettings settings;

        protected override string DefaultTemplateName
        {
            get
            {
                if (base.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                return "HTML5Rating";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "HTML5RatingDisplay";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            /*
            if (base.ControlMode == SPControlMode.Display && !Page.IsPostBack)
            {
                if (input_rating != null)
                {
                    Page.RegisterRequiresPostBack(this.input_rating);
                }
            }
            */
            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
            this.DisableInputFieldLabel = true;
            base.ControlMode = SPContext.Current.FormContext.FormMode;

            if (base.ControlMode == SPControlMode.Display || base.ControlMode == SPControlMode.Invalid)
            {
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(this);
                //Page.RegisterRequiresPostBack(this);
                //Page.RegisterRequiresRaiseEvent(this);
            }

            base.CreateChildControls();
            RatingField = (HTML5RatingField)ItemFieldValue;

            if (RatingField == null)
            {
                try
                {
                    url = SPContext.Current.Web.Url;
                    Guid listId = this.ListId;
                    Guid itemId = this.ListItem.UniqueId;

                    using (SPSite site = new SPSite(url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPListItem item = web.Lists[listId].Items[itemId];
                            //SPFile file = web.GetFile(itemId);
                            //SPListItem item = file.ListItemAllFields;

                            RatingField = item[BuildFieldId.Rating] as HTML5RatingField;
                            ItemFieldValue = RatingField;
                        }
                    }
                }
                catch (Exception)
                {
                    RatingField = new HTML5RatingField();
                }
                if (RatingField == null)
                {
                    RatingField = new HTML5RatingField();
                }
            }

            if (settings == null)
            {
                settings = new RatingSettings();
                settings = settings.Load();
            }

            if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
            {
                RatingField.Maximum = settings.Maximum;
                RatingField.Minimum = settings.Minimum;
                RatingField.Step = settings.Step;
            }
            else
            {
                input_rating = (RatingInput)this.TemplateContainer.FindControl("RatingInput");

                if (input_rating != null)
                {

                    if (!Page.ClientScript.IsClientScriptIncludeRegistered("jQuery"))
                    {
                        Page.ClientScript.RegisterClientScriptInclude(typeof(HTML5RatingControl), "jQuery", "/_layouts/Hemrika/Content/js/jquery.min.js");
                    }

                    if (!Page.ClientScript.IsClientScriptIncludeRegistered("RateIt"))
                    {
                        Page.ClientScript.RegisterClientScriptInclude(typeof(HTML5RatingControl), "RateIt", "/_layouts/Hemrika/Content/js/jquery.rateit.js");
                    }

                    CssRegistration.Register("/_layouts/Hemrika/Content/css/rateit.css");

                    input_rating.TextChanged += new EventHandler(input_rating_TextChanged);
                    input_rating.Step = RatingField.Step;
                    input_rating.Maximum = settings.Maximum;
                    input_rating.Minimum = settings.Minimum;
                    input_rating.Votes = RatingField.Votes;

                    if (RatingField.Votes > 0)
                    {
                        input_rating.Value = float.Parse((RatingField.Rating / RatingField.Votes).ToString());
                        input_rating.Text = float.Parse((RatingField.Rating / RatingField.Votes).ToString()).ToString("F2", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        input_rating.Value = 0.0f;
                        input_rating.Text = "0.0";
                    }

                    //input_rating.Value = 0.0f;
                    //input_rating.Text = "0.0f";

                    HtmlGenericControl rating = new HtmlGenericControl("div");
                    rating.Attributes.Add("class", "rateit");
                    rating.Attributes.Add("data-rateit-backingfld", "#" + input_rating.ClientID);
                    rating.Attributes.Add("data-rateit-resetable", "false");

                    if (Page.IsPostBack || HasVote())
                    {
                        rating.Attributes.Add("data-rateit-readonly", "true");
                    }

                    this.Controls.Add(rating);
                    //ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(rating);

                }

                MicroDate();
            }
        }

        private void MicroDate()
        {
            if (settings == null)
            {
                settings = new RatingSettings();
                settings = settings.Load();
            }

            HtmlGenericControl itemprop = new HtmlGenericControl("div");
            itemprop.Attributes.Add("itemprop", "aggregateRating");
            itemprop.Attributes.Add("itemscope", "itemscope");
            itemprop.Attributes.Add("itemtype", "http://schema.org/AggregateRating");

            HtmlGenericControl worst = new HtmlGenericControl("meta");
            worst.Attributes.Add("itemprop", "worstRating");
            worst.Attributes.Add("datatype", "integer");
            worst.Attributes.Add("content", settings.Minimum.ToString());
            itemprop.Controls.Add(worst);

            HtmlGenericControl best = new HtmlGenericControl("meta");
            best.Attributes.Add("itemprop", "bestRating");
            best.Attributes.Add("datatype", "integer");
            best.Attributes.Add("content", settings.Maximum.ToString());
            itemprop.Controls.Add(best);

            HtmlGenericControl value = new HtmlGenericControl("meta");
            value.Attributes.Add("itemprop", "ratingValue");
            value.Attributes.Add("content", Rating.ToString("F2", CultureInfo.InvariantCulture));
            value.Attributes.Add("datatype", "float");
            itemprop.Controls.Add(value);

            HtmlGenericControl count = new HtmlGenericControl("meta");
            count.Attributes.Add("itemprop", "ratingCount");
            count.Attributes.Add("content", RatingField.Votes.ToString());
            count.Attributes.Add("datatype", "integer");
            itemprop.Controls.Add(count);

            this.Controls.Add(itemprop);
        }

        private bool HasVote()
        {
                bool voted = false;
                try
                {
                    HttpCookie cookie = Page.Request.Cookies.Get(ListItem.UniqueId.ToString() + "_" + input_rating.ClientID);
                    if (cookie != null && cookie.Value != null)
                    {
                        bool parsed = bool.TryParse(cookie.Value, out voted);
                        if (parsed)
                        {
                            return voted;
                        }
                    }
                }
                catch { };

                return voted;
        }

        protected void input_rating_TextChanged(object sender, EventArgs e)
        {
            if ((sender.GetType() == typeof(RatingInput)) && base.ControlMode == SPControlMode.Display)
            {
                if (!HasVote())
                {
                    input_rating = sender as RatingInput;
                    //SPListItem item = this.ListItem;

                    url = SPContext.Current.Web.Url;
                    //OpenElevatedWeb();
                    Guid listId = this.ListId;
                    Guid itemId = this.ListItem.UniqueId;

                    SPUtility.ValidateFormDigest();

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        RatingField.Votes += 1;
                        RatingField.Rating += input_rating.Value;

                        using (SPSite site = new SPSite(url))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPListItem item = web.Lists[listId].Items[itemId];
                                //SPFile file = web.GetFile(itemId);
                                //SPListItem item = file.ListItemAllFields;

                                item[BuildFieldId.Rating] = RatingField;
                                item.SystemUpdate(false);
                            }
                        }
                    });
                }

                input_rating.Attributes.Add("data-rateit-readonly", "true");
                input_rating.Votes = RatingField.Votes;

                if (RatingField.Votes > 0)
                {
                    input_rating.Value = float.Parse((RatingField.Rating / RatingField.Votes).ToString());
                    input_rating.Text = float.Parse((RatingField.Rating / RatingField.Votes).ToString()).ToString("F2", CultureInfo.InvariantCulture);
                }
                else
                {
                    input_rating.Value = 0.0f;
                    input_rating.Text = "0.0";
                }

                Page.Response.Cookies[ListItem.UniqueId.ToString() + "_" + input_rating.ClientID].Value = Boolean.TrueString;
            }
        }

        //private SPWeb web = null;
        private string url = null;

        /*
        private void OpenElevatedWeb()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(url, SPUserToken.SystemAccount))
                {
                    site.AllowUnsafeUpdates = true;
                    web = site.OpenWeb();
                }
            });
        }
        */

        public override object Value
        {
            get
            {
                if (settings == null)
                {
                    settings = new RatingSettings();
                    settings = settings.Load();
                }

                if (base.ControlMode == SPControlMode.Edit || base.ControlMode == SPControlMode.New)
                {
                    //this.EnsureChildControls();

                    if (base.ControlMode == SPControlMode.New)
                    {
                        RatingField.Maximum = settings.Maximum;
                        RatingField.Minimum = settings.Minimum;
                        RatingField.Step = settings.Step;
                    }

                    return RatingField;
                }
                else
                {
                    return ItemFieldValue;
                }
            }
            set
            {
                this.EnsureChildControls();
                RatingField = (HTML5RatingField)value;

                if (settings == null)
                {
                    settings = new RatingSettings();
                    settings = settings.Load();
                }

                if (input_rating != null)
                {
                    input_rating.Step = RatingField.Step;
                    input_rating.Maximum = settings.Maximum;
                    input_rating.Minimum = settings.Minimum;
                    input_rating.Votes = RatingField.Votes;

                    if (RatingField.Votes > 0)
                    {
                        input_rating.Value = float.Parse((RatingField.Rating / RatingField.Votes).ToString());
                        input_rating.Text = float.Parse((RatingField.Rating / RatingField.Votes).ToString()).ToString("F2", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        input_rating.Value = 0.0f;
                        input_rating.Text = "0.0";
                    }
                }
            }
        }

        public override void UpdateFieldValueInItem()
        {
            base.UpdateFieldValueInItem();
        }

        protected int Minimum
        {
            get
            {
                return RatingField.Minimum;
            }
        }

        protected int Maximum
        {
            get
            {
                return RatingField.Maximum;
            }
        }

        protected double Rating
        {
            get
            {
                if (RatingField.Votes > 0)
                {                    
                    return (RatingField.Rating / RatingField.Votes);
                }
                return 0.0f;
            }
        }

        protected int Votes
        {
            get
            {
                return RatingField.Votes;
            }
        }

        /*
        public void RaisePostBackEvent(string eventArgument)
        {
            string what = eventArgument;
            //throw new NotImplementedException();
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void RaisePostDataChangedEvent()
        {

            //throw new NotImplementedException();
        }
        */
        
        public void RaisePostBackEvent(string eventArgument)
        {
            //throw new NotImplementedException();
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void RaisePostDataChangedEvent()
        {
            //throw new NotImplementedException();
        }

    }
}
