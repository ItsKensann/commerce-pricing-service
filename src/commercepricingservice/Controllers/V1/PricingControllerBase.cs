using commercepricing.infrastructure.Configuration;
using commercepricingservice.Logging;
using commercepricingservice.Middleware;
using commercepricingservice.Models.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Transactions;

namespace commercepricingservice.Controllers.V1
{
    /// <summary>
    /// Base class for the controller
    /// </summary>
    /// [ApiController]
    [ApiVersion("1.0")]
    [TypeFilter(typeof(LogEventFilter))]
    public abstract class PricingControllerBase : ControllerBase
    {
        private readonly TransactionState _transactionState;
        private readonly IOptions<ServiceDetails>? _serviceDetails;

        /// <summary>
        /// Base class for the controller constructor
        /// </summary>
        protected PricingControllerBase(TransactionState transactionState, IOptions<ServiceDetails> serviceDetails)
        {
            _transactionState = transactionState;
            _serviceDetails = serviceDetails;
        }

        /// <summary>
        /// Base class for the controller
        /// </summary>
        protected PricingControllerBase(TransactionState transactionState, ServiceDetails serviceDetails)
        {
            _transactionState = transactionState;
        }

        /// <summary>
        /// Create dictionary to add to the message 
        /// </summary>
        /// <param name="primaryId"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        protected internal Dictionary<string, object> CreateMessageUserProperties(string primaryId, string objectType)
        {
            Dictionary<string, object> userProperties = new Dictionary<string, object>
            {
                {"entityType", ControllerContext.RouteData.Values["entityType"]!.ToString()!},
                {"objectType", objectType},
                {"operationName", ControllerContext.RouteData.Values["action"]!.ToString()!},
                {"primaryId", primaryId},
                {"messageSource", _transactionState.MessageSource!},
                {"timestamp", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)},
                {"transactionId", _transactionState.TransactionId!},
                {"version", "v1"},
                {"parentOperationName", _transactionState.OperationName!},
                {"verb", ControllerContext.HttpContext.Request.Method},
                {"sourceType", "ServiceBus"}
            };

            return userProperties;
        }

        /// <summary>
        /// Create api response 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected internal IActionResult CreateApiResponse(HttpStatusCode statusCode, List<string> errors = null!)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(new ApiResponse
                    {
                        OperationName = ControllerContext.RouteData.Values["action"]!.ToString(),
                        ServiceName = _serviceDetails!.Value.ServiceName,
                        StatusCode = StatusCodes.Status200OK,
                        StatusDescription = nameof(HttpStatusCode.OK),
                        TransactionId = _transactionState.TransactionId,
                        Errors = new List<string>()
                    });
                case HttpStatusCode.NotFound:
                    return NotFound(new ApiResponse
                    {
                        OperationName = ControllerContext.RouteData.Values["action"]!.ToString(),
                        ServiceName = _serviceDetails!.Value.ServiceName,
                        StatusCode = StatusCodes.Status404NotFound,
                        StatusDescription = nameof(HttpStatusCode.NotFound),
                        TransactionId = _transactionState.TransactionId,
                        Errors = new List<string>()
                    });
                case HttpStatusCode.BadRequest:
                    return BadRequest(new ApiResponse
                    {
                        OperationName = ControllerContext.RouteData.Values["action"]!.ToString(),
                        ServiceName = _serviceDetails!.Value.ServiceName,
                        StatusCode = StatusCodes.Status400BadRequest,
                        StatusDescription = nameof(HttpStatusCode.BadRequest),
                        TransactionId = _transactionState.TransactionId,
                        Errors = errors
                    });
                case HttpStatusCode.Conflict:
                    return Conflict(new ApiResponse
                    {
                        OperationName = ControllerContext.RouteData.Values["action"]!.ToString(),
                        ServiceName = _serviceDetails!.Value.ServiceName,
                        StatusCode = StatusCodes.Status409Conflict,
                        StatusDescription = nameof(HttpStatusCode.Conflict),
                        TransactionId = _transactionState.TransactionId,
                        Errors = errors
                    });
                default:
                    throw new InvalidOperationException("Attempting to create undefined ApiResponse type");
            }
        }
    }
}
