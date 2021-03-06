﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hemrika.SharePresence.Common.Ribbon.Definitions
{
    /// <summary>
    /// Control group template definition.
    /// </summary>
    public class GroupTemplateDefinition : RibbonDefinition
    {
        /// <summary>
        /// Id for size tags. You will need this id when creating <see cref="GroupDefinition"/> instances.
        /// </summary>
        public string SizeId;

        /// <summary>
        /// Section ids for placing controls in. You should use this ids when creating <see cref="ControlDefinition"/> descendent classes instances.
        /// </summary>
        public IEnumerable<string> SectionIds;

        public Func<GroupTemplateDefinition, string> GetXMLFunc;
        internal string XML
        {
            get
            {
                return GetXMLFunc(this);
            }
        }
    }
}
