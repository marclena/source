using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.Logger.Core
{
    public class LogMessage
    {
       
        public string @timestamp { get; set; }
        public string Level { get; set; }
        public string LogEventType { get; set; }
        public string Application { get; set; }

       public LogMessageMetadata Metadata { get; set; }
        public string Message { get; set; }
    }
}
