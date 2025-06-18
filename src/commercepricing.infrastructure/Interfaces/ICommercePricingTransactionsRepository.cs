using Columbia.Cosmos.Common.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricing.infrastructure.Models.V1;

namespace commercepricing.infrastructure.Interfaces
{
    // temp price DTO for now
    public interface ICommercePricingTransactionsRepository : IRepository<Transaction<RetailPricingTransactionDto, string>, Guid> { }
}