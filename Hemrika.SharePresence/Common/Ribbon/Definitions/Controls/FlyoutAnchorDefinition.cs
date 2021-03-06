﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hemrika.SharePresence.Common.Ribbon.Definitions.Controls
{
    /// <summary>
    /// Anchor for dropdown menu.
    /// Does not have any action on click.
    /// </summary>
    public class FlyoutAnchorDefinition : VisualControlBaseDefinition, IContainer
    {
        internal override void Validate()
        {
            base.Validate();
            ValidationHelper.Current.CheckArrayHasElements(this, "Controls");
        }

        internal override string Tag
        {
            get { return "FlyoutAnchor"; }
        }

        /// <summary>
        /// Menu controls. At least one is required.
        /// </summary>
        public IEnumerable<ControlDefinition> Controls { get; set; }



        /// <summary>
        /// Controls size. By default, 32x32
        /// </summary>
        public ControlSize ControlsSize { get; set; }
    }
}
