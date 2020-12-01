using System;
using System.Runtime.Serialization;

namespace Vueling.XXX.Library.Exceptions
{
    [Serializable]
    public class RowOrColumnParamsAreNullOrEmptyException : Exception
    {

        public RowOrColumnParamsAreNullOrEmptyException()
        {

        }
        public RowOrColumnParamsAreNullOrEmptyException(string message) : base(message)
        {

        }
        public RowOrColumnParamsAreNullOrEmptyException(string message, Exception innerException) : base(message, innerException)
        {

        }
        protected RowOrColumnParamsAreNullOrEmptyException(SerializationInfo info,
         StreamingContext context)
            : base(info, context)
        {

        }

    }
}
