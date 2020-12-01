using System;
using System.Collections.Generic;
using System.Net;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.Helpers
{
    public static class Retry
    {
        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0) { System.Threading.Thread.Sleep(retryInterval); }

                    return action();
                }
                //TODO: capture custom exception or use Polly
                catch (WebException wex)
                {
                    FtpWebResponse response2 = (FtpWebResponse)wex.Response;
                    if (response2.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
