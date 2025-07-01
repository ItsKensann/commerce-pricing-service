using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricing.infrastructure.Models.Specifications;
using commercepricingservice.Common.Interfaces;
using Csc.Enterprise.Common.Dto;


namespace commercepricingservice.RequestHandlers.V1
{
    /// <summary>
    ///  
    /// </summary>
    public class GetTransactionsByPricingIdQuery
    {
        /// <summary>
        /// Id 
        /// </summary>
        public string Id {  get; set; }
    }

    /// <summary>
    /// Get transactos by Id handler 
    /// </summary>
    public class GetTransactionsByPricingIdHandler : IRequestHandler<GetTransactionsByPricingIdQuery, IEnumerable<TransactionDto>>
    {
        private readonly ICommercePricingTransactionsRepository _transactionsRepository;

        /// <summary>
        /// Handler constructor
        /// </summary>
        public GetTransactionsByPricingIdHandler(ICommercePricingTransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <summary>
        /// Handler 
        /// <param name="request">request is the unique Id that is formed with upc+region+locationId</param>
        /// </summary>
        public async Task<IEnumerable<TransactionDto>> HandleAsync(GetTransactionsByPricingIdQuery request)
        {
            var specification = new GetTransactionsByPayloadIdSpecification<RetailPricingDto, string>(request.Id);
            var response = await _transactionsRepository.GetItemsAsync(specification);

            var dtos = response.Results.Select(t => new TransactionDto
            {
                Id = t.Id,
                EventType = t.EventType,
                Payload = t.Payload,
            });

            return dtos;
        }
    }
}
