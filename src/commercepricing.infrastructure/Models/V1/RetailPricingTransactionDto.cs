using commercepricing.infrastructure.Interfaces;
using System.Runtime.Serialization;

namespace commercepricing.infrastructure.Models.V1
{
    /// <summary>
    /// Retail Pricing DTO
    /// </summary>
    [DataContract]
    public class RetailPricingTransactionDto: IHasId<string>
    {
        /// <summary>
        /// Combination Id that makes it unique: upc+region+locationId
        /// </summary>
        [DataMember(Name = "Id")]
        public required string Id { get; set; }

        /// <summary>
        /// Location ID: store or site three digits id
        /// </summary>
        [DataMember(Name = "locationId")]
        public string? LocationId { get; set; }

        /// <summary>
        /// Region: e.g. USA or CAN
        /// </summary>
        [DataMember(Name = "region")]
        public string? Region { get; set; }

        /// <summary>
        /// Channel: e.g. 'E' for Employee Store
        /// </summary>
        [DataMember(Name = "channel")]
        public string? Channel { get; set; }

        /// <summary>
        /// Style
        /// </summary>
        [DataMember(Name = "style")]
        public string? Style { get; set; }

        /// <summary>
        /// Color: Three digits color ID
        /// </summary>
        [DataMember(Name = "color")]
        public string? Color { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        [DataMember(Name = "size")]
        public string? Size { get; set; }

        /// <summary>
        /// Dimension
        /// </summary>
        [DataMember(Name = "dimension")]
        public string? Dimension { get; set; }

        /// <summary>
        /// Status code
        /// </summary>
        [DataMember(Name = "upc")]
        public string? UPC { get; set; }

        /// <summary>
        /// Prices: Available price list for specific item
        /// </summary>
        [DataMember(Name = "prices")]
        public PriceDto[]? Prices { get; set; }

        /// <summary>
        /// Promotions: Available promotion list for specific item
        /// </summary>
        [DataMember(Name = "promotions")]
        public PromotionDto[]? Promotions { get; set; }

        /// <summary>
        /// Discounts: Available discount list for specific item
        /// </summary>
        [DataMember(Name = "discounts")]
        public DiscountDto[]? Discounts { get; set; }
    }
}
