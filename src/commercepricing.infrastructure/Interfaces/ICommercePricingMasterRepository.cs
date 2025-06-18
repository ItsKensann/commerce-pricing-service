using Columbia.Cosmos.Common.Interfaces;
using commercepricing.infrastructure.Models;

namespace commercepricing.infrastructure.Interfaces
{
    // temp price DTO for now
    public interface ICommercePricingMasterRepository : IRepository<RetailPricingDto, string> {}
}