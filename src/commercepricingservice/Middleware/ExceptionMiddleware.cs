using Microsoft.ApplicationInsights;
using System.Net;

namespace commercepricingservice.Middleware
{
    /// <summary>
    /// Exception Middleware. Used to catch all exceptions.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TelemetryClient _logger;

        /// <summary>
        /// Exception middleware constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionMiddleware(RequestDelegate next, TelemetryClient logger)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.TrackException(ex);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(exception.Message);
        }
    }
}
