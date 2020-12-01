using System;

namespace Vueling.XXX.Library.Exceptions
{
    [Serializable]
    public class InvalidBookingOperationException : Exception
    {

        public InvalidBookingOperationException()
        {
        
        }
        public InvalidBookingOperationException(string message) : base(message)
        {

        }
        public InvalidBookingOperationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
