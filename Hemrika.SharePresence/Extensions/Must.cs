// -----------------------------------------------------------------------
// <copyright file="Must.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Must
    {
        public static T NotBeNull<T>(T value, string name = "") where T : class
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }
    }
}
