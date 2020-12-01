using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.Activitites.DB.Infrastructure.Exceptions
{
    [Serializable]
    public class ConfigurationInitializationException : Exception
    {

        public ConfigurationInitializationException()
        {

        }
        public ConfigurationInitializationException(string message)
            : base(message)
        {

        }
        public ConfigurationInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
