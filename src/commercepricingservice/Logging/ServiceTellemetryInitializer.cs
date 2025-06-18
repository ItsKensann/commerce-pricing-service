using commercepricingservice.Configurations;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Diagnostics;

namespace commercepricingservice.Logging
{
    /// <summary>
    /// Adds service details for all telemetry sent to app insights from this service
    /// </summary>
    public class ServiceTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ServiceDetails _serviceDetails;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceDetails"></param>
        public ServiceTelemetryInitializer(ServiceDetails serviceDetails)
        {
            _serviceDetails = serviceDetails;
        }
        /// <summary>
        /// initialize telemetry context
        /// </summary>
        /// <param name="telemetry"></param>
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.GlobalProperties.TryAdd("familyName", _serviceDetails.FamilyName);
            telemetry.Context.GlobalProperties.TryAdd("serviceName", _serviceDetails.ServiceName);
            var activity = Activity.Current;

            if (activity != null)
            {
                foreach (var tag in activity.Tags)
                {
                    telemetry.Context.GlobalProperties.TryAdd(tag.Key, tag.Value);
                }
            }
        }
    }
}
