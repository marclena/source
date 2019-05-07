using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Vueling.XXX.WebApi.Logging
{
    /// <summary>
    /// Log all unhandled exceptions
    /// </summary>
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// Log error sync
        /// </summary>
        /// <param name="context"></param>
        public override void Log(ExceptionLoggerContext context)
        {
            Trace.TraceError("Error: {0}, StackTrace: {1}", context.Exception.Message, context.Exception.StackTrace);
        }

        /// <summary>
        /// Determine if an exception must be logged
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return true;
        }

        /// <summary>
        /// Log error async
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            Log(context);
            await Task.FromResult(0);
        }
    }
}