using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    class HTML5HeaderField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 6;

        public HTML5HeaderField() : base(6)
        {
            base[0] = Boolean.TrueString;
            base[1] = String.Empty;
            base[2] = String.Empty;
            base[3] = Boolean.TrueString;
            base[4] = Boolean.TrueString;
            base[5] = String.Empty;
            
        }

        public HTML5HeaderField(string value)
            : base(value)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal bool UseTitle
        {
            get
            {
                return Convert.ToBoolean(base[0]);
            }
            set
            {
                base[0] = Convert.ToString(value);
            }
        }


        internal string Heading
        {
            get
            {
                return base[1];
            }
            set
            {
                base[1] = value;
            }
        }

        internal string SubHeading
        {
            get
            {
                return base[2];
            }
            set
            {
                base[2] = value;
            }
        }

        internal bool IncludeAuthor
        {
            get
            {
                return  Convert.ToBoolean(base[3]);
            }
            set
            {
                base[3] = Convert.ToString(value);
            }
        }

        internal bool IncludeDate
        {
            get
            {
                return Convert.ToBoolean(base[4]);
            }
            set
            {
                base[4] = Convert.ToString(value);
            }
        }

        internal string Text
        {
            get
            {
                return base[5];
            }
            set
            {
                base[5] = value;
            }
        }
    }
}
