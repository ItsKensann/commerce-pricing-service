using System;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.domain.Models;

namespace commercepricing.domain.Interfaces
{
    // temp price DTO for now
    public interface ICommercePricingTransactionsRepository : IRepository<Transaction<Price, string> ,Guid> {}
}