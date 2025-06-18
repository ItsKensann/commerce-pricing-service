using Columbia.Cosmos.Common;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using Microsoft.Azure.Cosmos;

namespace commercepricing.infrastructure.Repository
{
    public class CommercePricingMasterRepository : BaseCosmosRepository<RetailPricingDto, string>, ICommercePricingMasterRepository
    {
        public CommercePricingMasterRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory, new CosmosLinqQuery()) { }

        public override string ContainerName => "commercepricingmaster";

        public override PartitionKey ResolvePartitionKey(RetailPricingDto item)
        {
            return new PartitionKey(item.UPC);
        }
        public override PartitionKey ResolvePartitionKey(string id)
        {
            return new PartitionKey(id);
        }

    }

}
