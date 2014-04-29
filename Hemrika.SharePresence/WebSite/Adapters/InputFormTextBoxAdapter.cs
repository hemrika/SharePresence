// -----------------------------------------------------------------------
// <copyright file="InputFormTextBoxAdapter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.WebSite.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.Adapters;
    using System.Web.UI.WebControls.Adapters;
    using System.Web.UI;
    using System.Web;
    using Microsoft.SharePoint.Security;
    using System.Security.Permissions;
    using System.Web.UI.WebControls;
    using System.Collections.Specialized;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Utilities;
    using System.Globalization;
    using Microsoft.SharePoint;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ValidationProperty("Text"), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
    public class InputFormTextBoxAdapter : WebControlAdapter, IValidator
    {
        // Fields
        //private bool m_bSetFocus;
        //private ContentDirection m_direction;
        private bool m_fIsValid = true;
        
        internal InputFormTextBox _InputFormTextBox;
        internal InputFormTextBox webControl
        {
            get
            {
                if (_InputFormTextBox == null)
                {
                    _InputFormTextBox = (InputFormTextBox)Control;
                }
                return _InputFormTextBox;
            }
        }

        public InputFormTextBoxAdapter()
        {

        }

        // Methods
        
        protected override void BeginRender(HtmlTextWriter writer)
        {
            base.BeginRender(writer);
        }

        protected override void CreateChildControls()
        {
            /*
            this.Control.Controls.Clear();

            HiddenField hiddenField = new HiddenField();
            hiddenField.ID = this.Control.ID+"_hidden";
            this.Control.Parent.Controls.Add(hiddenField);
            Control parent = this.Control.Parent;
            parent.Controls.Remove(this.Control);
            //Control.Controls.Add(hiddenField);
            */
            base.CreateChildControls();
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void OnInit(EventArgs e)
        {
            this.Page.Validators.Add(this);
            base.OnInit(e);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void OnPreRender(EventArgs e)
        {
            /*
            if ((webControl.TextMode == TextBoxMode.MultiLine) && webControl.RichText)
            {
                ScriptLink.Register(this.Page, "form.js", true);
            }
            this.Page.ClientScript.RegisterClientScriptBlock(typeof(InputFormTextBox), "MultiLineTextBoxSupportSrcipt", SPHttpUtility.NoEncode("\r\n                <script type=\"text/javascript\">\r\n                // <![CDATA[\r\n                function GetEffectTiveTextLengthInControl(ThisControl, SelectedText)\r\n                {\r\n                    var  EffectTiveTextLengthInControl = ThisControl.innerText.length;\r\n\r\n                    if (SelectedText != null && SelectedText.parentElement() == ThisControl)\r\n                    {\r\n                        EffectTiveTextLengthInControl -= SelectedText.text.length;\r\n                    }\r\n                    return EffectTiveTextLengthInControl;\r\n                }\r\n\r\n                function MultiLineTextBoxOnKeyPress(ThisControl, MaxLength)\r\n                {\r\n                    if (ThisControl.isMultiLine && MaxLength > 0) \r\n                    {\r\n                        var  SelectedText = document.selection.createRange();\r\n                        var  EffectTiveTextLengthInControl = GetEffectTiveTextLengthInControl(ThisControl, SelectedText);\r\n\r\n                        if (EffectTiveTextLengthInControl >= MaxLength)\r\n                        {\r\n                            event.returnValue = false;\r\n                        }\r\n                    }\r\n                }\r\n\r\n                function MultiLineTextBoxOnPaste(ThisControl, MaxLength)\r\n                {\r\n                    if (ThisControl.isMultiLine && MaxLength > 0)\r\n                    {\r\n                        var  SelectedText = document.selection.createRange();\r\n                        var  EffectTiveTextLengthInControl = GetEffectTiveTextLengthInControl(ThisControl, SelectedText);\r\n\r\n                        var  CanPasteLength = MaxLength - EffectTiveTextLengthInControl;\r\n\r\n                        if (CanPasteLength <= 0) \r\n                        {\r\n                            event.returnValue = false;\r\n                        }\r\n                        else \r\n                        {\r\n                            var TextToPaste = window.clipboardData.getData('Text');\r\n\r\n                            if (TextToPaste != null && TextToPaste.length > CanPasteLength)\r\n                            {\r\n                                TextToPaste = TextToPaste.substr(0, CanPasteLength);\r\n                                SelectedText.text = TextToPaste;\r\n                                event.returnValue = false;\r\n                            }\r\n                        }\r\n                    }\r\n                }\r\n                // ]]>\r\n                </script>"));
            if (webControl.RichText)
            {
                CssRegistration.Register("forms.css");
            }
            if ((webControl.Visible && (webControl.TextMode == TextBoxMode.MultiLine)) && webControl.RichText)
            {
                SPUtility.RegisterRichTextOnSubmitClientScript(this.Page, webControl.ClientID);
            }
            */
            base.OnPreRender(e);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void Render(HtmlTextWriter output)
        {
            /*
            webControl.Attributes.Add("alwaysenablesilent", "true");
            if ((webControl.TextMode == TextBoxMode.MultiLine) && (webControl.MaxLength > 0))
            {
                webControl.Attributes.Add("onkeypress", "MultiLineTextBoxOnKeyPress(this, " + SPHttpUtility.NoEncode(webControl.MaxLength) + ")");
                webControl.Attributes.Add("onpaste", "MultiLineTextBoxOnPaste(this, " + SPHttpUtility.NoEncode(webControl.MaxLength) + ")");
            }
            if (webControl.Direction == ContentDirection.NotSet)
            {
                base.Render(output);
            }
            else
            {
                output.Write("<span dir=\"");
                output.Write((webControl.Direction == ContentDirection.LeftToRight) ? "ltr" : "rtl");
                output.Write("\">");
                base.Render(output);
                output.Write("</span>");
            }
            if (!webControl.IsValid)
            {
                Label label = null;
                if (webControl.ErrorMessageLabelControl.Length > 0)
                {
                    label = webControl.FindControl(webControl.ErrorMessageLabelControl) as Label;
                }
                if (label == null)
                {
                    output.Write("<br/>");
                    output.Write("<span class='ms-error'>");
                    SPHttpUtility.HtmlEncode(webControl.ErrorMessage, output);
                    output.Write("</span>");
                }
            }
            if ((webControl.TextMode == TextBoxMode.MultiLine) && webControl.RichText)
            {
                string valueToEncode = SPUtility.TextAreaToRichTextClientScript(webControl.ClientID, true, webControl.AllowHyperlink, null, (uint)CultureInfo.CurrentUICulture.LCID, webControl.RichTextMode, this.m_bSetFocus, SPContext.Current.Web.ServerRelativeUrl, webControl.Enabled && !webControl.ReadOnly);
                output.Write(SPHttpUtility.NoEncode(valueToEncode));
                output.Write("<input type='hidden' id='{0}' name='{1}'/>", SPHttpUtility.NoEncode(webControl.ClientID + "_spSave"), SPHttpUtility.NoEncode(webControl.UniqueID + "_spSave"));
            }
            */

            base.Render(output);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public virtual void Validate()
        {
            this.m_fIsValid = true;
            Label label = null;
            if (webControl.ErrorMessageLabelControl.Length > 0)
            {
                label = webControl.FindControl(webControl.ErrorMessageLabelControl) as Label;
            }
            if ((webControl.Enabled && (webControl.MaxLength > 0)) && ((webControl.Text != null) && (webControl.Text.Length > webControl.MaxLength)))
            {
                webControl.ErrorMessage = SPHttpUtility.NoEncode(SPResource.GetString("InputLengthGreaterThanMaxLength", new object[] { webControl.MaxLength }));
                this.m_fIsValid = false;
            }
            if (label != null)
            {
                if (this.m_fIsValid)
                {
                    label.Text = string.Empty;
                }
                else
                {
                    label.Text = SPHttpUtility.HtmlEncode(webControl.ErrorMessage);
                }
            }
        }

        // Properties

        public string ErrorMessage
        {
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            get
            {
                return webControl.ErrorMessage;
                /*
                string str = this.ViewState["ErrorMessage"] as string;
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
                */
            }
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            set
            {
                webControl.ErrorMessage = value;
                /*
                this.ViewState["ErrorMessage"] = value;
                */
            }
        }

        public bool IsValid
        {
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            get
            {
                if (webControl.Enabled)
                {
                    return this.m_fIsValid;
                }
                return true;
            }
            [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
            set
            {
                this.m_fIsValid = value;
            }
        }
    }
}
