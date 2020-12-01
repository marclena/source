using System;
using System.Runtime.Serialization;

namespace Vueling.XXX.DB.Infrastructure.Exceptions
{
    [Serializable]
    public class AircraftAlreadyExistingOnDatabaseException : Exception
    {

        public AircraftAlreadyExistingOnDatabaseException()
        {
        
        }
        public AircraftAlreadyExistingOnDatabaseException(string message) : base(message)
        {

        }
        public AircraftAlreadyExistingOnDatabaseException(string message, Exception innerException) : base(message, innerException)
        {

        }
        protected AircraftAlreadyExistingOnDatabaseException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {
        
        }

    }
}
