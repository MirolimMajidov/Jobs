using System;

namespace Service.SharedModel.Exceptions
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
