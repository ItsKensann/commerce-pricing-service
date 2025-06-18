using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.Filters;

namespace commercepricingservice.Logging
{
    /// <summary>
    /// Logging action filter
    /// </summary>
    public class LogEventFilter : IActionFilter
    {
        private readonly TelemetryClient _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public LogEventFilter(TelemetryClient logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Log when action is completed
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.TrackEvent($"Completed {context.HttpContext.Request.Path}", new Dictionary<string, string>
            {
                { "status" , "SUCCESS" }
            });
        }

        /// <summary>
        /// Log when action is starting
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.TrackEvent($"Start {context.HttpContext.Request.Path}");
        }
    }
}
