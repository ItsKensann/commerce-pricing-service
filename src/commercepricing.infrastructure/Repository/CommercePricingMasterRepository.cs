using Columbia.Cosmos.Common;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.domain.Interfaces;
using commercepricing.domain.Models;
using Microsoft.Azure.Cosmos;

namespace commercepricing.infrastructure.Repository
{
    public class CommercePricingMasterRepository : BaseCosmosRepository<Price, string>, ICommercePricingMasterRepository
    {
        public CommercePricingMasterRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory, new CosmosLinqQuery()) { }

        public override string ContainerName => "commercepricingmaster";

        public override PartitionKey ResolvePartitionKey(Price item)
        {
            return new PartitionKey(item.Id);
        }
        public override PartitionKey ResolvePartitionKey(string id)
        {
            return new PartitionKey(id);
        }

    }

}
