using System;
using System.Runtime.Serialization;

namespace Vueling.XXX.DB.Infrastructure.Exceptions
{
    [Serializable]
    public class AircraftNotFoundOnDatabaseException : Exception
    {

        public AircraftNotFoundOnDatabaseException()
        {
        
        }
        public AircraftNotFoundOnDatabaseException(string message) : base(message)
        {

        }
        public AircraftNotFoundOnDatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        protected AircraftNotFoundOnDatabaseException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {
        
        }

    }
}
