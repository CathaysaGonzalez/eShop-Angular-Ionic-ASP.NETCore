using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Exceptions
{
    public class BEInvalidCustomerException : BEException
    {
        public BEInvalidCustomerException() { }
        public BEInvalidCustomerException(string message) : base(message) { }
        public BEInvalidCustomerException(string message, Exception innerException): base(message, innerException) { }
    }
}
