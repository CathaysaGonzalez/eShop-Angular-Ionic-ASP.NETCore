using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Exceptions
{
    public class BEInvalidQuantityException : BEException
    {
        public BEInvalidQuantityException() { }
        public BEInvalidQuantityException(string message) : base(message) { }
        public BEInvalidQuantityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
