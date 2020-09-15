using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Exceptions
{
    public class BEInvalidProductException : BEException
    {
        public BEInvalidProductException() { }
        public BEInvalidProductException(string message) : base(message) { }
        public BEInvalidProductException(string message, Exception innerException): base(message, innerException) { }
    }
}
