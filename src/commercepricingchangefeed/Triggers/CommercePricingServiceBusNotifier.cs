using Columbia.ServiceBus.Common;
using commercepricing.infrastructure.Configuration;
using commercepricing.infrastructure.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace commercepricingchangefeed.Triggers
{
    public class CommercePricingServiceBusNotifier
    {
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceDetails _configuration;
        private readonly ILogger<CommercePricingServiceBusNotifier> _logger;

        public CommercePricingServiceBusNotifier(IServiceBusClient serviceBusClient, 
                                                 IOptions<ServiceDetails> configuration, 
                                                 ILogger<CommercePricingServiceBusNotifier> logger)
        {
            _serviceBusClient = serviceBusClient;
            _configuration = configuration.Value;
            _logger = logger;
        }

        [Function("CommercePricingServiceBusNotifier")]
        public async Task RunAsync(
            [CosmosDBTrigger(
            databaseName: "%CosmosDb:DatabaseId%",
            containerName: "%commercepricingMasterCollectionName%",
            Connection = "cosmosdb-connection",
            LeaseContainerName = "%leasesCollectionName%",
            LeaseContainerPrefix = "CommercePricingServiceBusNotifier")]
            IReadOnlyList<RetailPricingDto> documents)
            
        {
            try
            {
                if (documents != null && documents.Any())
                {
                    await Task.WhenAll(documents.Select(async pricingdocument =>
                    {
                        _logger.LogInformation($"Sending notification of Commerce Pricing chage for id {pricingdocument.Id} to service bus...");

                        var message = new
                        {
                            CallbackUrl = $"{_configuration.BaseGatewayPath}/commercepricing/{pricingdocument.Id}",
                            UPC = $"{pricingdocument.UPC}",
                        };

                        var userProperties =
                            new
                            {
                                EventType = pricingdocument.EventType,
                                Channel = pricingdocument.Channel,
                                Region = pricingdocument.Region,
                                Style = pricingdocument.Style,
                                LocationId = pricingdocument.LocationId,
                                HasPromotions = pricingdocument.Promotions?.Any() ?? false,
                                HasDiscounts = pricingdocument.Discounts?.Any() ?? false
                            };

                        await _serviceBusClient.SendJsonMessageAsync(message, userProperties, pricingdocument.Id);
                        _logger.LogInformation($"CommercePricingServiceBusNotifier Sent Service Bus Message {message}, userProperties {userProperties}");
                    })); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
