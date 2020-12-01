using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.XXX.Impl.ServiceLibrary.Exceptions
{
    [Serializable]
    public class InvalidFlightDetailsException : Exception
    {
        public InvalidFlightDetailsException(string message)
            : base(message)
        {

        }
    }
}
