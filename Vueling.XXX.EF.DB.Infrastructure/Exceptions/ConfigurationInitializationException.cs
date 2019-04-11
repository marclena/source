using System;

namespace Vueling.XXX.EF.DB.Infrastructure.Exceptions
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
