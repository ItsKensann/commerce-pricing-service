using Microsoft.AspNetCore.Mvc;
using commercepricing.domain.Models;

namespace commercepricingservice.Controllers.V1
{
    /// <summary>
    ///   Commerce Pricing controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        [HttpGet("{productId}")]
        public ActionResult<Price> GetCommercePricingById(string priceId)
        {
            return Ok(new Price
            {
                PriceId = priceId,
                Product = "Sample Product Name",
                Amount = 99.99m,
                Currency = "USD",
                PriceType = "Regular",
                PriceGroup = "Standard",
                Store = "Store001",
                Promotion = "Winter Sale"
            });

        }

        [HttpPut("{productId}")]
        public ActionResult<Price> CreateOrUpdatePrice(string productId, [FromBody] Price price)
        {
            price.PriceId = productId;
            price.LastUpdated = DateTime.UtcNow;

            return Ok(price);
        }
    }
}