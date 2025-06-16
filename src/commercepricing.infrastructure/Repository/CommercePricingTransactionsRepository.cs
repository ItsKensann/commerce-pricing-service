

using System;
using Columbia.Cosmos.Common;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.domain.Models;
using commercepricing.domain.Interfaces;
using Microsoft.Azure.Cosmos;

namespace commercepricing.infrastructure.Repository
{
    public class CommercePricingTransactionsRepository : BaseCosmosRepository<Transaction<Price, string>, Guid>, ICommercePricingTransactionsRepository
    {
        public CommercePricingTransactionsRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory, new CosmosLinqQuery()) { }

        public override string ContainerName => "commercepricingtransactions";

        public override PartitionKey ResolvePartitionKey(Transaction<Price, string> item)
        {
            throw new NotImplementedException();
        }
    }
}