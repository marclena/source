using System;
using System.Runtime.Serialization;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary.Exceptions
{
    [Serializable]
    public class MessagePublishedException : Exception
    {

        public MessagePublishedException()
        {

        }
        public MessagePublishedException(string message)
            : base(message)
        {

        }
        public MessagePublishedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        protected MessagePublishedException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {
        
        }

    }
}
