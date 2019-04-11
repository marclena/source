using ATC.Taskling.Client.Core.Extensions.Logger;
using System.Diagnostics;

namespace Vueling.XXX.WCF.WebService.Helpers
{
    public class TasklingLogger : ITasklingEventLogger
    {
        public void TraceError(string error)
        {
            Trace.TraceError(error);
        }
    }
}