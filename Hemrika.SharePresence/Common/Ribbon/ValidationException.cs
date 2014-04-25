using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Hemrika.SharePresence.Common.Ribbon
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
