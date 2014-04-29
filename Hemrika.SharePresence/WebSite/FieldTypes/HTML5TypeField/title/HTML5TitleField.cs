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
    class HTML5TitleField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 1;

        public HTML5TitleField() : base(1)
        {
            base[0] = String.Empty;
        }

        public HTML5TitleField(string value)
            : base(value)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal string Title
        {
            get
            {
                return base[0];
            }
            set
            {
                base[0] = value;
            }
        }
    }
}
