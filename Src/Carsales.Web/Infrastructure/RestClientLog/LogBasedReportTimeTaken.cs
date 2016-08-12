using System;
using System.Diagnostics;
using Bolt.Logger;
using Bolt.RestClient.Dto;
using Carsales.Web.Infrastructure.Attributes;

namespace Carsales.Web.Infrastructure.RestClientLog
{
    public class LogBasedReportTimeTaken : Bolt.RestClient.IReportTimeTaken
    {
        private readonly ILogger logger;

        public LogBasedReportTimeTaken(ILogger logger)
        {
            this.logger = logger;
        }

        [DebuggerStepThrough]
        public void Notify(RestRequest request, TimeSpan timeTaken)
        {
            logger.Trace("{0} : {1} took {2}ms", request.Method, request.Url, timeTaken.TotalMilliseconds);
        }
    }
}