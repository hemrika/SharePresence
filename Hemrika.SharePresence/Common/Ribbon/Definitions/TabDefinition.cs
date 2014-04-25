using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Hemrika.SharePresence.Common.Ribbon.Libraries;

namespace  Hemrika.SharePresence.Common.Ribbon.Definitions
{
    /// <summary>
    /// Definition for a ribbon tab
    /// </summary>
    public class TabDefinition : RibbonDefinition
    {
        internal override void Validate()
        {
            base.Validate();
            ValidationHelper.Current.CheckNotNull(this, "Title");
            ValidationHelper.Current.CheckArrayHasElements(this, "GroupTemplates");
            ValidationHelper.Current.CheckArrayHasElements(this, "Groups");
            ValidationHelper.Current.CheckNotNull(this, "Sequence");
        }

        /// <summary>
        /// Title, it will be displayed as the tab caption.
        /// </summary>
        public string Title;

        /// <summary>
        /// <para>Templates for control groups within the tab.</para>
        /// <para>By default, it equals to <see cref="GroupTemplateLibrary.AllTemplates"/>.</para>
        /// </summary>
        public GroupTemplateDefinition[] GroupTemplates = GroupTemplateLibrary.AllTemplates;

        /// <summary>
        /// Groups of controls. Required at least one group.
        /// </summary>
        public GroupDefinition[] Groups;

        public string Sequence;
    }
}
