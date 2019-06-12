using System;

namespace Vueling.XXX.Impl.ServiceLibrary.Exceptions
{
    [Serializable]
    public class InvalidFlightDetailsException : Exception
    {
        public InvalidFlightDetailsException(string message)
            : base(message)
        {

        }

        public InvalidFlightDetailsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        protected InvalidFlightDetailsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
