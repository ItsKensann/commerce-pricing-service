using Columbia.Logging.Core;
using commercepricing.infrastructure.Configuration;
using commercepricing.infrastructure.Models;
using commercepricingservice.Common.Interfaces;
using commercepricingservice.Middleware;
using commercepricingservice.RequestHandlers.V1;
using Csc.Enterprise.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;

namespace commercepricingservice.Controllers.V1
{
    /// <summary>
    /// Commerce Pricing controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/commercepricing")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CommercePricingController : PricingControllerBase
    {
        private readonly ILogger<CommercePricingController> _logger;
        private readonly IActivityTagger _activityTagger;
        private readonly IOptions<ServiceDetails> _serviceDetails;
        private readonly IRequestHandler<CreateOrUpdateCommercePricingQuery, Guid> _createOrUpdateCommercePricingHandler;
        private readonly IRequestHandler<GetTransactionsByPricingIdQuery, IEnumerable<TransactionDto>> _getTransactionsByIdHandler;
        private readonly IRequestHandler<string, RetailPricingDto> _getRetailPricingHandler;

        /// <summary>
        /// Commerce Pricing controller constructor
        /// </summary>
        public CommercePricingController(TransactionState transactionState, ILogger<CommercePricingController> logger,
            IActivityTagger activityTagger, IOptions<ServiceDetails> serviceDetails,
            IRequestHandler<CreateOrUpdateCommercePricingQuery, Guid> createOrUpdateCommercePricingHandler,
            IRequestHandler<GetTransactionsByPricingIdQuery, IEnumerable<TransactionDto>> getTransactionsByIdHandler,
            IRequestHandler<string, RetailPricingDto> getRetailPricingHandler)
            : base(transactionState, serviceDetails)
        {
            _logger = logger;
            _activityTagger = activityTagger;
            _serviceDetails = serviceDetails;
            _createOrUpdateCommercePricingHandler = createOrUpdateCommercePricingHandler;
            _getTransactionsByIdHandler = getTransactionsByIdHandler;
            _getRetailPricingHandler = getRetailPricingHandler;
        }

        /// <summary>
        /// Creates or updates a commerce pricing resource
        /// </summary>
        [HttpPut("{upc}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult> CreateOrUpdateCommercePricingByUpc([FromRoute] string upc, [FromBody] RetailPricingDto dto, [FromQuery] string eventType = null!)
        {
            var response = new ResponseDto();

            // validation
            if (dto == null)
            {
                var message = $"Missing {nameof(dto)}";
                _logger.LogWarning(message);

                response.Errors.Add($"Missing {nameof(dto)}");
                return BadRequest(response);
            }
    
            if (string.IsNullOrEmpty(upc))
            {
                var message = $"Missing {nameof(upc)}";
                _logger.LogWarning(message);

                response.Errors.Add($"Missing {nameof(upc)}");
                return BadRequest(response);
            }

            if (upc != dto.UPC)
            {
                var message = $"UPC does not match the payload";
                _logger.LogWarning(message);
                response.Errors.Add($"UPC does not match the payload");
                return BadRequest(response);
            }

            _activityTagger.AddTag("UPC", dto.UPC!);
            _logger.LogInformation("Endpoint was called: {Endpoint} for Retail Pricing: {upc}", nameof(CreateOrUpdateCommercePricingByUpc), dto.UPC);

            // handle the request
            var payloadId = $"{dto.UPC} + {dto.Region} + {dto.LocationId}";
            var queryObj = new CreateOrUpdateCommercePricingQuery
            {
                Id = payloadId,
                EventType = eventType,
                Dto = dto
            };
            
            var result = await _createOrUpdateCommercePricingHandler.HandleAsync(queryObj);

            _activityTagger.AddTag("TransactionId", result.ToString());
            response.ResourceCallbackUrl = $"{_serviceDetails.Value.BaseGatewayPath}/{dto.UPC}";
            return Accepted(response);
        }

        /// <summary>
        /// Get all transactions for a single item
        /// </summary>
        [HttpGet("transactions/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsById ([FromRoute] string id)
        {
            try
            {
                _activityTagger.AddTag("Id", id);
                _logger.LogInformation("Endpoint was called: {Endpoint} for Retail Price Id: {id}", nameof(GetTransactionsById), id);

                var retailPrice = await _getTransactionsByIdHandler.HandleAsync(new GetTransactionsByPricingIdQuery { Id = id });
                Console.WriteLine($"Handler returned: {retailPrice?.Count()} items");
                if (retailPrice == null)
                {
                    _logger.LogWarning("Retail Price Not Found: {id}", id);
                    return NotFound();
                }
                return Ok(retailPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Retail price by id
        /// </summary>
        [HttpGet("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<RetailPricingDto>> GetPricingById([FromRoute] string id)
        {
            try
            {
                _activityTagger.AddTag("Id", id);
                _logger.LogInformation("Endpoint was called: {Endpoint} for Retail Price Id: {id}", nameof(GetTransactionsById), id);

                var retailPrice = await _getRetailPricingHandler.HandleAsync(id);

                if (retailPrice == null)
                {
                    _logger.LogWarning("Retail Price Not Found: {id}", id);
                    return NotFound();
                }
                return Ok(retailPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}