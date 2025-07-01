using commercepricing.infrastructure.Interfaces;
using commercepricing.infrastructure.Models;
using commercepricingservice.Common.Interfaces;

namespace commercepricingservice.RequestHandlers.V1
{
    /// <summary>
    /// Get Pricing by Id handler
    /// </summary>
    public class GetPricingByIdHandler : IRequestHandler<string, RetailPricingDto>
    {
        private readonly ICommercePricingMasterRepository _masterRepository;

        /// <summary>
        /// Handler constructor
        /// </summary>
        public GetPricingByIdHandler(ICommercePricingMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }

        /// <summary>
        /// Handler 
        /// <param name="request">request is the unique Id that is formed with upc+region+locationId</param>
        /// </summary>
        public async Task<RetailPricingDto> HandleAsync(string request)
        {
            var result = await _masterRepository.GetItemAsync(request);

            if (result == null)
                return null;

            var dto = result.Entity;
            return dto;
        }
    }
}
