using Columbia.Cosmos.Common.Interfaces;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricing.infrastructure.Models.V1;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace commercepricingchangefeed
{
    public class CommercePricingTransactionsListener
    {
        private readonly IUpserter<ICommercePricingMasterRepository, RetailPricingDto, string> _upserter;
        private readonly ILogger<CommercePricingTransactionsListener> _logger;

        public CommercePricingTransactionsListener(
            IUpserter<ICommercePricingMasterRepository, RetailPricingDto, string> upserter,
            ILogger<CommercePricingTransactionsListener> logger)
        {
            _upserter = upserter;
            _logger = logger;
        }

        [Function("CommercePricingTransactionsListener")]
        public async Task RunAsync(
            [CosmosDBTrigger(
                databaseName: "%CosmosDb:DatabaseId%",
                containerName: "%commercePricingTransactionsCollectionName%",
                Connection = "cosmosdb-connection",
                LeaseContainerName = "%leasesCollectionName%",
                LeaseContainerPrefix = "CommercePricingTransactionsListener")] IReadOnlyList <Transaction<RetailPricingDto, string>> documents)
        {
            try
            {
                if (documents != null && documents.Any())
                {
                    _logger.LogInformation($"Price documents to be modified: {documents.Count}");

                    await Task.WhenAll(documents.Select(async transactionDocument =>
                    {
                        // Set the event type from the transaction document
                         transactionDocument.Payload.EventType = transactionDocument.EventType;

                        _logger.LogInformation($"Processing transaction {transactionDocument.Id} for Price {transactionDocument.Payload.Id}");

                        await _upserter.UpsertAsync(transactionDocument.Payload,
                            price => price.Id,
                            (incoming, existing) =>
                            {
                                existing.Entity.Update(incoming);
                                return existing.Entity;
                            },
                            createModelFunc: price =>
                            {
                                return price;
                            },
                            shouldSkipCreatePredicate: price =>
                            {
 
                                bool shouldSkip = string.IsNullOrWhiteSpace(transactionDocument.Payload.Id);

                                return (shouldSkip, $"Skipping creation for transaction {transactionDocument.Id} - " +
                                        $"Price {transactionDocument.Payload.Id} does not exist and this is an event-type transaction. " +
                                        $"Event transactions can only update existing resources.");
                            });
                    }));
                }
                else
                {
                    _logger.LogInformation("No documents to process in this batch");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing commerce pricing transactions: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}