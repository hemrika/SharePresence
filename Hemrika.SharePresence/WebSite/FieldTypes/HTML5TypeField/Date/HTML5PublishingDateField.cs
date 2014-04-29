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
    class HTML5PublishingDateField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 1;

        public HTML5PublishingDateField() : base(1)
        {
            base[0] = string.Empty;
        }

        public HTML5PublishingDateField(string value)
            : base(value)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        internal DateTime PublishingDate
        {
            get
            {
                return DateTime.Parse(base[0]);
            }
            set
            {
                base[0] = value.ToString();
            }
        }
    }
}
