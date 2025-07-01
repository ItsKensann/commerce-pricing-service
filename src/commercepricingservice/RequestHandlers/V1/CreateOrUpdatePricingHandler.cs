using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricingservice.Common.Interfaces;

namespace commercepricingservice.RequestHandlers.V1
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateOrUpdateCommercePricingQuery
    {
        /// <summary>
        /// Id 
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Event Type
        /// </summary>
        public string? EventType { get; set; }

        /// <summary>
        /// Retail Pricing Transaction Dto
        /// </summary>
        public RetailPricingDto? Dto { get; set; }
    }

    /// <summary>
    /// create and update handler
    /// </summary>
    public class CreateOrUpdatePricingHandler : IRequestHandler<CreateOrUpdateCommercePricingQuery, Guid>
    {
        private readonly ICommercePricingTransactionsRepository _transactionRepository;

        /// <summary>
        /// Handler constructor
        /// </summary>
        public CreateOrUpdatePricingHandler(ICommercePricingTransactionsRepository repository)
        {
            _transactionRepository = repository;
        }

        /// <summary>
        /// Handler
        /// </summary>
        public async Task<Guid> HandleAsync(CreateOrUpdateCommercePricingQuery request)
        {


            //As it stands if we don't pass the purchase order Id in via the DTO the upsert will break because
            //the transaction collection uses it as the partition key.
            var transaction = new Transaction<RetailPricingDto, string>
            {
                Id = Guid.NewGuid(),
                EventType = request.EventType,
                Payload = request.Dto!
            };

            await _transactionRepository.UpsertItemAsync(transaction);

            return transaction.Id;
        }
    }
}

