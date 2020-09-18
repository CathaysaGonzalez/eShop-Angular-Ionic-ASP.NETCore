using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Exceptions
{
    public class BEException : Exception
    {
        public BEException() { }
        public BEException(string message) : base(message) { }
        public BEException(string message, Exception innerException) : base(message, innerException) { }
    }
}
