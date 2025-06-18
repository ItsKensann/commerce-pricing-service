using System.Runtime.Serialization;

namespace commercepricing.infrastructure.Models
{
    /// <summary>
    /// Promotion DTO: Different types of promotion list
    /// </summary>
    [DataContract]
    public class PromotionDto
    {
        /// <summary>
        /// Type
        /// </summary>
        [DataMember(Name = "type")]
        public string? Type { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "id")]
        public string? Id { get; set; }

        /// <summary>
        /// Description: e.g. BOGO Promo
        /// </summary>
        [DataMember(Name = "description")]
        public string? Description { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember(Name = "amount")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        [DataMember(Name = "currencyCode")]
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        [DataMember(Name = "quantity")]
        public int? Quantity { get; set; }
    }
}
