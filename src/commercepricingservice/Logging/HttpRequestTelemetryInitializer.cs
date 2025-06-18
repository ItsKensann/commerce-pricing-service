using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace commercepricingservice.Logging
{
    /// <summary>
    /// Adds HttpContext details to the request telemetry object sent to app insights
    /// </summary>
    public class HttpRequestTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string transactionIdKey = "transactionId";
        private const string operationNameKey = "operationName";
        private const string parentOperationNameKey = "parentOperationName";
        private const string messageSourceKey = "messageSource";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="accessor"></param>
        public HttpRequestTelemetryInitializer(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        /// <summary>
        /// initialize context
        /// </summary>
        /// <param name="telemetry"></param>
        public void Initialize(ITelemetry telemetry)
        {
            if (_httpContextAccessor.HttpContext == null)
                return;

            ISupportProperties propTelemetry = (ISupportProperties)telemetry;

            if (_httpContextAccessor.HttpContext.Items.TryGetValue(transactionIdKey, out var transactionId))
                propTelemetry.Properties.TryAdd(nameof(transactionId), transactionId?.ToString());
            // Log the incoming header operationName value as the parentOperationName
            if (_httpContextAccessor.HttpContext.Items.TryGetValue(operationNameKey, out var operationName))
                propTelemetry.Properties.TryAdd(parentOperationNameKey, operationName?.ToString());

            if (_httpContextAccessor.HttpContext.Items.TryGetValue(messageSourceKey, out var messageSource))
                propTelemetry.Properties.TryAdd(messageSourceKey, messageSource?.ToString());
        }
    }
}
