using System.Diagnostics;

namespace Vueling.XXX.WebUI
{
    [Vueling.Extensions.Library.DI.RegisterOnActivated]
    public class OnRegisteredTest
    {
        [Vueling.Extensions.Library.DI.RegisterActionOnActivated]
        public void PrintTrace()
        {
            Debug.WriteLine("Invoked PrintTrace in Debug mode.");
            Trace.TraceInformation("Invoked PrintTrace in Information mode.");
        }
    }
}