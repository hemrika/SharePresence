using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hemrika.SharePresence.Common.Ribbon.Definitions
{
    /// <summary>
    /// Control group
    /// </summary>
    public class GroupDefinition : RibbonDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        internal override void Validate()
        {
            base.Validate();
            ValidationHelper.Current.CheckNotNull(this, "Title");
            ValidationHelper.Current.CheckNotNull(this, "Template");
            ValidationHelper.Current.CheckArrayHasElements(this, "Controls");
            ValidationHelper.Current.CheckNotNull(this, "Sequence");
        }
        
        /// <summary>
        /// Title. Will be displayed below the controls with small gray font. Required.
        /// </summary>
        public string Title;

        /// <summary>
        /// Group template. For standard templates, see <see cref="Libraries.GroupTemplateLibrary"/>. Required.
        /// </summary>
        public GroupTemplateDefinition Template;
        
        /// <summary>
        /// Collection of inner controls
        /// </summary>
        public ControlDefinition[] Controls;

        /// <summary>
        /// 
        /// </summary>
        public string Sequence;

    }
}
