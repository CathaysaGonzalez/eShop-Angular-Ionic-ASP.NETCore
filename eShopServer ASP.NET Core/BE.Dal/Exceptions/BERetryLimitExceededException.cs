using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Dal.Exceptions
{
    public class BERetryLimitExceededException : BEException
    {
        public BERetryLimitExceededException() { }
        public BERetryLimitExceededException(string message) : base(message){ }
        public BERetryLimitExceededException(string message, Exception innerException) : base(message, innerException) { }
    }
}
