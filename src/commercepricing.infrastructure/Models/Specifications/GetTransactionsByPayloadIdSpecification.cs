using System;
using Ardalis.Specification;
using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models.V1;

namespace commercepricing.infrastructure.Models.Specifications
{
    public class GetTransactionsByPayloadIdSpecification<T, U> : Specification<Transaction<T,U>>
        where T : IHasId<U>
        where U : class
    {
        public GetTransactionsByPayloadIdSpecification(U id)
        {
            Query.Where(transaction => transaction.Payload.Id == id);
        }
    }
}
