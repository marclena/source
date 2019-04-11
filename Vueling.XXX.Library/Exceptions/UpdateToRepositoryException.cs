using System;
using System.Runtime.Serialization;

namespace Vueling.XXX.Library.Exceptions
{
    [Serializable]
    public class UpdateToRepositoryException : Exception
    {

        public UpdateToRepositoryException()
        {
        
        }
        public UpdateToRepositoryException(string message) : base(message)
        {

        }
        public UpdateToRepositoryException(string message, Exception innerException) : base(message, innerException)
        {

        }
        protected UpdateToRepositoryException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {
        
        }

    }
}
