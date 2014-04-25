using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;
using Hemrika.SharePresence.Common.ServiceLocation;

namespace Hemrika.SharePresence.Common.UI
{
    public class EnhancedEditorPart : EditorPart
    {

        private string[] PropertyNames;

        public EnhancedEditorPart(params string[] propertyNames)
            : base()
        {
            this.PropertyNames = propertyNames;
        }

        protected override void CreateChildControls()
        {
            WebPart webPart = this.WebPartToEdit;

            int i = 0;


            foreach (string propertyName in PropertyNames)
            {
                PropertyInfo propertyInfo = webPart.GetType().GetProperty(propertyName);
                Type propertyType = propertyInfo.PropertyType;
                object[] attributes = propertyInfo.GetCustomAttributes(false);

                IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));

                Control control = displayClass.CreateControl();
                control.ID = "control" + i.ToString();

                Panel sectionHead = new Panel();
                sectionHead.CssClass = "UserSectionHead";

                WebDisplayNameAttribute displayNameAttribute = attributes.OfType<WebDisplayNameAttribute>().FirstOrDefault();
                if (displayNameAttribute != null)
                {
                    if (displayClass.IsControlInHeaderSection())
                    {
                        sectionHead.Controls.Add(control);
                        sectionHead.Controls.Add(new LiteralControl(" "));
                    }
                    sectionHead.Controls.Add(new LiteralControl(displayNameAttribute.DisplayName));
                }

                this.Controls.Add(sectionHead);

                if (!displayClass.IsControlInHeaderSection())
                {
                    Panel sectionBody = new Panel();
                    sectionBody.CssClass = "UserSectionBody";
                    sectionBody.Style.Add("padding-bottom", "8px");

                    sectionBody.Controls.Add(control);

                    this.Controls.Add(sectionBody);
                }

                if (i < PropertyNames.Length - 1)
                {

                    Panel sectionDottedLine = new Panel();
                    sectionDottedLine.CssClass = "UserDottedLine";
                    sectionDottedLine.Style.Add("width", "100%");
                    sectionDottedLine.Style.Add("margin-bottom", "5px");
                    this.Controls.Add(sectionDottedLine);

                }

                i++;
            }


            SyncChanges();

            base.CreateChildControls();
        }

        public override bool ApplyChanges()
        {
            WebPart webPart = this.WebPartToEdit;

            int i = 0;

            foreach (string propertyName in this.PropertyNames)
            {
                PropertyInfo propertyInfo = webPart.GetType().GetProperty(propertyName);
                Type propertyType = propertyInfo.PropertyType;
                IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));

                Control control = this.FindControl("control" + i.ToString());
                propertyInfo.SetValue(webPart, displayClass.GetControlValue(control), null);

                i++;
            }

            return true;
        }


        public override void SyncChanges()
        {
            WebPart webPart = this.WebPartToEdit;

            int i = 0;

            foreach (string propertyName in PropertyNames)
            {
                PropertyInfo propertyInfo = webPart.GetType().GetProperty(propertyName);
                Type propertyType = propertyInfo.PropertyType;
                object propertyValue = propertyInfo.GetValue(webPart, null);
                IDisplayableClass displayClass = WebPartServiceLocator.Current.DisplayableClasses.FirstOrDefault(c => c.IsAppliable(propertyType));

                Control control = (Control)this.FindControl("control" + i.ToString());
                displayClass.SetControlValue(control, propertyValue);

                i++;
            }

        }
    }

}
