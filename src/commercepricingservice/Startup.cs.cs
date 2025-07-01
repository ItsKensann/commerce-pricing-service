using Columbia.Cosmos.Common.Extensions;
using Columbia.Cosmos.Common.FaultTolerance;
using Columbia.Logging.ApplicationInsights.Web.Extensions;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Repository;
using commercepricingservice.Common.Caching;
using commercepricingservice.Common.Interfaces;
using commercepricingservice.Configurations;
using commercepricingservice.Middleware;
using commercepricingservice.RequestHandlers.V1;
using commercepricingservice.Swagger;
using commercepricingservice.SwaggerConfigurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text.Json.Serialization;
using Csc.Enterprise.Common.Dto;
using commercepricing.infrastructure.Models;

namespace commercepricingservice
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Startup constructor
        /// </summary>
        public Startup(IWebHostEnvironment hostingHostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingHostingEnvironment;
            _configuration = configuration;
        }

        /// <summary>
        /// Service Configuration
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = true)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problems = new BadRequestResponse(context);

                        return new BadRequestObjectResult(problems);
                    };
                });

            services.AddRouting(options => { options.LowercaseQueryStrings = true; options.LowercaseUrls = true; });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;

                // Accept version from multiple sources
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("api-version")
                );
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, _hostingEnvironment.ApplicationName + ".xml"));
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(x => x.FullName!.Replace("+", "."));
                options.OperationFilter<SwaggerDefaultValues>();
                options.SchemaFilter<EnumSchemaFilter>();
            });

            services.AddScoped<TransactionState>();

            services.AddSingleton(provider =>
            {
                var details = new ServiceConfiguration();
                _configuration.Bind("ServiceConfiguration", details);

                return details;
            });

            services.AddSingleton(provider =>
            {
                var apiConfiguration = new ApiConfiguration();
                _configuration.Bind("ApiConfiguration", apiConfiguration);

                return apiConfiguration;
            });

            // Commerce Pricing
            services.AddTransient<ICommercePricingMasterRepository, CommercePricingMasterRepository>();
            services.AddScoped<IRequestHandler<CreateOrUpdateCommercePricingQuery, Guid>, CreateOrUpdatePricingHandler>();
            services.AddScoped<IRequestHandler<string, RetailPricingDto>, GetPricingByIdHandler>();

            // Transaction 
            services.AddTransient<ICommercePricingTransactionsRepository, CommercePricingTransactionsRepository>();
            services.AddScoped<IRequestHandler<GetTransactionsByPricingIdQuery, IEnumerable<TransactionDto>>, GetTransactionsByPricingIdHandler>();

            services.AddHealthChecks();

            // caching
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _configuration.GetValue<string>("redis-connection");
            });
            services.AddSingleton<IDistributedCacheWrapper, DistributedCacheService>();

            // cosmos db
            services.AddCosmosDb(
                _configuration.GetValue<string>("cosmosdb-connection"),
                options => _configuration.GetSection("CosmosDb").Bind(options),
                cosmosResiliencySettings: _configuration.GetValue<CosmosResiliencySettings>("CosmosResiliencySettings"));

            //Logging
            services.AddOptions();
            services.Configure<ServiceDetails>(_configuration.GetSection("ServiceDetails"));
            services.AddColumbiaTelemetry();

        }

        /// <summary>
        /// Configure the startup            
        /// </summary>
        /// <param name="app">Application settings</param>
        /// <param name="env">Environment configuration</param>
        /// <param name="provider">Provider description</param>
        /// <returns>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</returns>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}
