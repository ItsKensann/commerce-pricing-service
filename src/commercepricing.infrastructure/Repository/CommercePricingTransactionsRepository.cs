using Columbia.Cosmos.Common;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricing.infrastructure.Models.V1;
using Microsoft.Azure.Cosmos;

namespace commercepricing.infrastructure.Repository
{
    public class CommercePricingTransactionsRepository : BaseCosmosRepository<Transaction<RetailPricingTransactionDto, string>, Guid>, ICommercePricingTransactionsRepository
    {
        public CommercePricingTransactionsRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory, new CosmosLinqQuery()) { }

        public override string ContainerName => "commercepricingtransactions";

        public override PartitionKey ResolvePartitionKey(Transaction<RetailPricingTransactionDto, string> item)
        {
            return new PartitionKey(item.Payload!.Id);
        }
    }
}