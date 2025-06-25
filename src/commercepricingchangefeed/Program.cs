using Columbia.Cosmos.Common.Extensions;
using Columbia.Cosmos.Common.FaultTolerance;
using Columbia.Logging.ApplicationInsights.Worker.Extensions;
using commercepricing.infrastructure.Configuration;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricing.infrastructure.Repository;
using commercepricingchangefeed.Configurations;
using csc.azure.datalake.client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;


namespace commercepricingchangefeed
{
    public class Program
    {
        public static void Main()
        {
           var host = new HostBuilder()
                .ConfigureFunctionsWorkerWithUserCodeExceptions()
                .ConfigureServices(services =>
                {
                    IConfigurationRoot config = new ConfigurationBuilder()
                                            .SetBasePath(Environment.CurrentDirectory)
                                            .AddJsonFile("local.settings.json")
                                            .AddEnvironmentVariables()
                                            .Build();
                    services.AddLoggingDependencies();

                    services.AddCosmosDb(
                        config.GetValue<string>("cosmosdb-connection"),
                        options =>
                        {
                            options.Containers = config["CosmosDb:Containers"].Split(',').ToList();
                            options.DatabaseId = config["CosmosDb:DatabaseId"];
                        },
                        cosmosResiliencySettings: config.GetValue<CosmosResiliencySettings>("CosmosResiliencySettings")
                    );

                    services.AddDatalakeClient(options => config.Bind(options));
                    services.Configure<DataLakeConfiguration>(config);
                    services.Configure<ServiceDetails>(config.GetSection("ServiceDetails"));

                    services.AddSingleton<ICommercePricingMasterRepository, CommercePricingMasterRepository>();
                    services.AddUpserterAsSingleton<ICommercePricingMasterRepository, RetailPricingDto, string>();
                })
                .Build();
            host.Run();
        }
        
    }
}

