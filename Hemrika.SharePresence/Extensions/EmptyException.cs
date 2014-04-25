// -----------------------------------------------------------------------
// <copyright file="EmptyException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hemrika.SharePresence.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when the object is empty.
    /// </summary>
    [Serializable]
    public class EmptyException : Exception
    {
        public EmptyException() { }
        public EmptyException(string message)
            : base(message) { }
        public EmptyException(string message, Exception inner)
            : base(message, inner) { }
        protected EmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
