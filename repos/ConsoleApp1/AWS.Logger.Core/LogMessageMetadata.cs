using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.Logger.Core
{
    public class LogMessageMetadata
    {
        public string Environment { get; set; }
        public string Ami { get; set; }
        public string MachineName { get; set; }
        public string AtcLog { get; set; }
    }
}
