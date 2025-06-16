using System;
using System.Linq;
using Columbia.Cosmos.Common.Extensions;
using Columbia.Cosmos.Common.FaultTolerance;
using Columbia.Logging.ApplicationInsights.Worker.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using commercepricing.domain.Interfaces;
using commercepricing.domain.Models;
using commercepricing.infrastructure.Repository;


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

                    // service bus logic omitted for now

                    //services.AddCosmosDb(
                    //    config.GetValue<string>("cosmosdb-connection"),
                    //    options =>
                    //    {
                    //        options.Containers = config["CosmosDb:Containers"].Split(',').ToList();
                    //        options.DatabaseId = config["CosmosDb:DatabaseId"];
                    //    },
                    //    cosmosResiliencySettings: config.GetValue<CosmosResiliencySettings>("CosmosResiliencySettings")
                    //);

                    services.AddSingleton<ICommercePricingMasterRepository, CommercePricingMasterRepository>();
                    services.AddUpserterAsSingleton<ICommercePricingMasterRepository, Price, string>();


                    // logic for service details & data lake configuration
                })
                .Build();
            host.Run();
        }
        
    }
}

