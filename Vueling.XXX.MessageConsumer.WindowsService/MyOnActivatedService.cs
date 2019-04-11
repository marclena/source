using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Vueling.XXX.MessageConsumer.WindowsService
{
    [Vueling.Extensions.Library.DI.RegisterOnActivated]
    public class MyOnActivatedService
    {
        [Vueling.Extensions.Library.DI.RegisterActionOnActivated]
        public void PrintTrace()
        {
            Trace.TraceInformation("Invoked MyStatefulService.PrintTrace");
        }
    }
}
