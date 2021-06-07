using System;

namespace Jobs.Service.Common.Infrastructure.Exceptions
{
    public class JobException : Exception
    {
        public JobException()
        {
        }

        public JobException(string message) : base(message)
        {
        }

        public JobException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
