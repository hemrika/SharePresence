// -----------------------------------------------------------------------
// <copyright file="SharePointWebControls.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;

    public static class SharePointWebControls
    {
        public static Control GetSharePointControls(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            // check if the field is a buildIn field, or a can be rendered by a SharePoint Control
            if (field.FieldRenderingControl == null)
            {
                return null;
            }

            switch (field.FieldRenderingControl.ToString())
            {
                case "Microsoft.SharePoint.WebControls.TextField":
                    return CreateTextFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.NoteField":
                    return CreateNoteFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.RichTextField":
                    return CreateRichTextFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.DropDownChoiceField":
                    return CreateDropDownChoiceField(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.RadioButtonChoiceField":
                    return CreateRadioButtonChoiceField(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.NumberField":
                    return CreateNumberFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.CurrencyField":
                    return CreateCurrencyFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.DateTimeField":
                    return CreateDateTimeFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.LookupField":
                    return CreateLookupFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.MultipleLookupField":
                    return CreateMultipleLookupFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.BooleanField":
                    return CreateBooleanField(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.UserField":
                    return CreateUserFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.UrlField":
                    return CreateUrlFieldControl(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.ComputedField":
                    return CreateComputedFieldCotnrol(field, list, item, mode);

                case "Microsoft.SharePoint.WebControls.AttachmentsField":
                    return CreateAttachmentsFieldControl(field, list, item, mode);

                default:
                    break;
            }

            return null;
        }

        public static object GetSharePointControlValue(ControlCollection Controls, SPField field)
        {
            string fieldName = GetFieldName(field);
            Control returnObject = null;

            if (Controls != null)
            {
                foreach (Control control in Controls)
                {
                    returnObject = FindControlRecursive(control, fieldName);
                    if (returnObject != null)
                        break;
                }

            }

            if (returnObject != null)
            {
                return GetValueFromObject(field, returnObject);
            }

            return returnObject;
        }

        public static object GetValueFromObject(SPField field, Control returnObject)
        {
            switch (field.FieldRenderingControl.ToString())
            {
                case "Microsoft.SharePoint.WebControls.TextField":
                    return ((TextField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.NoteField":
                    return ((NoteField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.RichTextField":
                    return ((RichTextField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.DropDownChoiceField":
                    return ((DropDownChoiceField)returnObject).ItemFieldValue;

                case "Microsoft.SharePoint.WebControls.RadioButtonChoiceField":
                    return ((RadioButtonChoiceField)returnObject).ItemFieldValue;

                case "Microsoft.SharePoint.WebControls.NumberField":
                    return ((NumberField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.CurrencyField":
                    return ((CurrencyField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.DateTimeField":
                    return ((DateTimeField)returnObject).ItemFieldValue;

                case "Microsoft.SharePoint.WebControls.LookupField":
                    return ((LookupField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.MultipleLookupField":
                    return ((MultipleLookupField)returnObject).Value;

                case "Microsoft.SharePoint.WebControls.BooleanField":
                    return ((BooleanField)returnObject).ItemFieldValue;

                case "Microsoft.SharePoint.WebControls.UserField":
                    return ((UserField)returnObject).ItemFieldValue;

                case "Microsoft.SharePoint.WebControls.UrlField":
                    return ((UrlField)returnObject).ItemFieldValue;

                default:
                    return null;
            }
        }

        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }

            return null;
        }

        public static string GetFieldName(SPField field)
        {
            string fieldName = field.Title;
            
            if (field.FieldRenderingControl != null)
            {
                if (field.FieldRenderingControl.ToString() == "Microsoft.SharePoint.WebControls.DateTimeField" ||
                    field.FieldRenderingControl.ToString() == "Microsoft.SharePoint.WebControls.UserField")
                {
                    fieldName.Replace(' ', '_');
                    if (field.FieldRenderingControl.ToString() == "Microsoft.SharePoint.WebControls.UserField")
                    {
                        fieldName.Replace('@', '_');
                    }
                }
            }

            return fieldName;
        }

        #region Create SharePoint Controls
        private static Control CreateTextFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            TextField tf = new TextField();
            tf.ListId = list.ID;
            tf.ItemId = item.ID;
            tf.FieldName = field.Title;
            tf.ID = field.Title;
            tf.ControlMode = mode;
            return tf;
        }

        private static Control CreateNoteFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            NoteField nf = new NoteField();
            nf.ListId = list.ID;
            nf.ItemId = item.ID;
            nf.FieldName = field.Title;
            nf.ID = field.Title;
            nf.ControlMode = mode;
            return nf;
        }

        private static Control CreateRichTextFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            RichTextField rtf = new RichTextField();
            rtf.ListId = list.ID;
            rtf.ItemId = item.ID;
            rtf.FieldName = field.Title;
            rtf.ID = field.Title;
            rtf.ControlMode = mode;
            return rtf;
        }

        private static Control CreateDropDownChoiceField(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            DropDownChoiceField ddcf = new DropDownChoiceField();
            ddcf.ListId = list.ID;
            ddcf.ItemId = item.ID;
            ddcf.FieldName = field.Title;
            ddcf.ID = field.Title;
            ddcf.ControlMode = mode;
            return ddcf;
        }

        private static Control CreateRadioButtonChoiceField(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            RadioButtonChoiceField rbcf = new RadioButtonChoiceField();
            rbcf.ListId = list.ID;
            rbcf.ItemId = item.ID;
            rbcf.FieldName = field.Title;
            rbcf.ID = field.Title;
            rbcf.ControlMode = mode;
            return rbcf;
        }

        private static Control CreateNumberFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            NumberField nf = new NumberField();
            nf.ListId = list.ID;
            nf.ItemId = item.ID;
            nf.FieldName = field.Title;
            nf.ID = field.Title;
            nf.ControlMode = mode;
            return nf;
        }

        private static Control CreateCurrencyFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            CurrencyField cf = new CurrencyField();
            cf.ListId = list.ID;
            cf.ItemId = item.ID;
            cf.FieldName = field.Title;
            cf.ID = field.Title;
            cf.ControlMode = mode;
            return cf;
        }

        private static Control CreateDateTimeFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            DateTimeField dtc = new DateTimeField();
            dtc.ListId = list.ID;
            dtc.ItemId = item.ID;
            dtc.FieldName = field.Title;
            // Replace blanks with _ so that the control still works
            dtc.ID = field.Title.Replace(' ', '_');
            dtc.ControlMode = mode;
            return dtc;
        }

        private static Control CreateLookupFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            LookupField lf = new LookupField();
            lf.ListId = list.ID;
            lf.ItemId = item.ID;
            lf.FieldName = field.Title;
            lf.ID = field.Title;
            lf.ControlMode = mode;
            return lf;
        }

        private static Control CreateMultipleLookupFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            MultipleLookupField mlf = new MultipleLookupField();
            mlf.ListId = list.ID;
            mlf.ItemId = item.ID;
            mlf.FieldName = field.Title;
            mlf.ID = field.Title;
            mlf.ControlMode = mode;
            return mlf;
        }

        private static Control CreateBooleanField(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            BooleanField bf = new BooleanField();
            bf.ListId = list.ID;
            bf.ItemId = item.ID;
            bf.FieldName = field.Title;
            bf.ID = field.Title;
            bf.ControlMode = mode;
            return bf;
        }

        private static Control CreateUserFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            UserField uf = new UserField();
            uf.ListId = list.ID;
            uf.ItemId = item.ID;
            uf.FieldName = field.Title;
            // Replace blanks/@ with _ so that the control still works
            uf.ID = field.Title.Replace(' ', '_').Replace('@', '_');
            uf.ControlMode = mode;
            return uf;
        }

        private static Control CreateUrlFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            UrlField urlf = new UrlField();
            urlf.ListId = list.ID;
            urlf.ItemId = item.ID;
            urlf.FieldName = field.Title;
            urlf.ID = field.Title;
            urlf.ControlMode = mode;
            return urlf;
        }

        private static Control CreateComputedFieldCotnrol(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            ComputedField cf = new ComputedField();
            cf.ListId = list.ID;
            cf.ItemId = item.ID;
            cf.FieldName = field.Title;
            cf.ID = field.Title;
            cf.ControlMode = mode;
            return cf;
        }

        private static Control CreateAttachmentsFieldControl(SPField field, SPList list, SPListItem item, SPControlMode mode)
        {
            AttachmentsField af = new AttachmentsField();
            af.ListId = list.ID;
            af.ItemId = item.ID;
            af.FieldName = field.Title;
            af.ID = field.Title;
            af.ControlMode = mode;
            return af;
        }
        #endregion
    }

}
