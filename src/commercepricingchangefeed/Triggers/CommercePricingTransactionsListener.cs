
namespace commercepricingchangefeed.Triggers
{
    public class CommercePricingTransactionsListener
    {
        //private readonly IUpserter<ICommercePricingMasterRepository, Price, string> _upserter;
        //private readonly ILogger<CommercePricingTransactionsListener> _logger;

        //public CommercePricingTransactionsListener(
        //    IUpserter<ICommercePricingMasterRepository, Price, string> upserter,
        //    ILogger<CommercePricingTransactionsListener> logger)
        //{
        //    _upserter = upserter;
        //    _logger = logger;
        //}

        //[Function("CommercePricingTransactionsListener")]
        //public async Task RunAsync(
        //    [CosmosDBTrigger(
        //        databaseName: "%CosmosDb:DatabaseId%",
        //        containerName: "%commercePricingTransactionsCollectionName%",
        //        Connection = "cosmosdb-connection",
        //        LeaseContainerName = "%leasesCollectionName%",
        //        LeaseContainerPrefix = "CommercePricingTransactionsListener")] IReadOnlyList<Transaction<Price, string>> documents)
        //{
        //    try
        //    {
        //        if (documents != null && documents.Any())
        //        {
        //            _logger.LogInformation($"Price documents to be modified: {documents.Count}");

        //            await Task.WhenAll(documents.Select(async transactionDocument =>
        //            {
        //                transactionDocument.Payload.EventType = transactionDocument.EventType;
        //                _logger.LogInformation($"Processing transaction {transactionDocument.Id} for Price {transactionDocument.Payload.Id}");

        //                await _upserter.UpsertAsync(transactionDocument.Payload,
        //                    price => price.Id,
        //                    (incoming, existing) =>
        //                    {
        //                        existing.Entity.Update(incoming);
        //                        return existing.Entity;
        //                    },
        //                    createModelFunc: price =>
        //                    {
        //                        price.CreatedDateTime = price.LastUpdatedDateTime;
        //                        return price;
        //                    },
        //                    shouldSkipCreatePredicate: price =>
        //                    {
        //                        return (!string.IsNullOrWhiteSpace(transactionDocument.EventType),
        //                            $"The transaction is an event, but Price {transactionDocument.Payload.Id} " +
        //                            $"has not yet been created in the repo. " +
        //                            $"An event-typed transaction can only update a resource that already exists.");
        //                    });
        //            }));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing commerce pricing transactions: {ErrorMessage}", ex.Message);
        //        throw;
        //    }
        //}
    }
}