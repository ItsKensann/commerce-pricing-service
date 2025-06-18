using commercepricingservice.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace commercepricingservice.Swagger
{
    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    /// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
    /// Once they are fixed and published, this class can be removed.</remarks>
    public class SwaggerDefaultValues : IOperationFilter
    {
        private readonly ServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Constructor for default values
        /// </summary>
        /// <param name="serviceConfiguration"></param>
        public SwaggerDefaultValues(ServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ApiDescription apiDescription = context.ApiDescription;

            operation.Deprecated = apiDescription.IsDeprecated();

            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (description.RouteInfo == null)
                {
                    continue;
                }

                parameter.Required |= description.RouteInfo.IsOptional;

                // Set the default version for the DeleteInvoices endpoint
                if (context.ApiDescription.ActionDescriptor.RouteValues["action"] == "DeleteInvoices" && parameter.Name == "version")
                {
                    parameter.Schema.Default = new OpenApiString("2");
                }
            }

            // Determine if this is a v3+ endpoint by checking the API version in the document
            bool isV3OrHigherEndpoint = false;
            var apiVersionModel = context.ApiDescription.GetApiVersion();
            if (apiVersionModel != null && apiVersionModel.MajorVersion >= 3)
            {
                isV3OrHigherEndpoint = true;
            }

            // Also check path for version indicators as a fallback
            var path = context.ApiDescription.RelativePath?.ToLower();
            if (!isV3OrHigherEndpoint && path != null && path.Contains("/v3/"))
            {
                isV3OrHigherEndpoint = true;
            }

            // Exclude DELETE /invoices endpoint or v3+ endpoints from required headers in Swagger
            var method = context.ApiDescription.HttpMethod?.ToUpper();
            bool isDeleteInvoices = method == "DELETE" && path != null && path.Contains("/invoices");

            if (isDeleteInvoices || isV3OrHigherEndpoint)
            {
                // For v3+ or DELETE endpoints, add the headers as optional
                foreach (var header in _serviceConfiguration.RequiredHeaders!)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = header.Name,
                        Description = isV3OrHigherEndpoint
                            ? $"{header.Description} (Optional for v3+ endpoints)"
                            : header.Description,
                        In = ParameterLocation.Header,
                        Required = false
                    });
                }
            }
            else
            {
                // For v1 and v2 endpoints, add the headers as required
                foreach (var header in _serviceConfiguration.RequiredHeaders!)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = header.Name,
                        Description = header.Description,
                        In = ParameterLocation.Header,
                        Required = true
                    });
                }
            }
        }
    }
}
