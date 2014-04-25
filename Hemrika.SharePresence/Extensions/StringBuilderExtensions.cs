// -----------------------------------------------------------------------
// <copyright file="StringBuilderExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static void AppendWhen(this StringBuilder sb, bool condition, string text)
        {
            if (condition)
                sb.Append(text);
        }
    }
}
