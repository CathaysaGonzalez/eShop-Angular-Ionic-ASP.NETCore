using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Exceptions
{
    public class BEConcurrencyException : BEException
    {
        public BEConcurrencyException() { }
        public BEConcurrencyException(string message) : base(message) { }
        public BEConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
