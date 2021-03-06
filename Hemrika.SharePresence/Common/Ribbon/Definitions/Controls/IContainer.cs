﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hemrika.SharePresence.Common.Ribbon.Definitions.Controls
{
    /// <summary>
    /// Interface for all controls with inner elements
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Child controls of a container
        /// </summary>
        IEnumerable<ControlDefinition> Controls { get; set; }

        /// <summary>
        /// Controls size. By default, 32x32
        /// </summary>
        ControlSize ControlsSize { get; set; }
    }
}
