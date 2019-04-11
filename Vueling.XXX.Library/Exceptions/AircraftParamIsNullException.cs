using System;
using System.Runtime.Serialization;

namespace Vueling.XXX.Library.Exceptions
{
    [Serializable]
    public class AircraftParamIsNullException : Exception
    {

        public AircraftParamIsNullException()
        {
        
        }
        public AircraftParamIsNullException(string message) : base(message)
        {

        }
        public AircraftParamIsNullException(string message, Exception innerException) : base(message, innerException)
        {

        }
        protected AircraftParamIsNullException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {

        }

    }
}
