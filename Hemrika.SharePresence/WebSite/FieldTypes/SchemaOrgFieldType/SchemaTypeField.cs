using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Hemrika.SharePresence.WebSite.FieldTypes;

namespace Hemrika.SharePresence.WebSite.FieldTypes
{
    class SchemaTypeField : SPFieldMultiColumnValue
    {
        private const int NumberOfFields = 1;

        public SchemaTypeField()
            : base(1)
        {
            base[0] = "";
        }

        public SchemaTypeField(string value)
            : base(value)
        {
        }

        internal string Type
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
