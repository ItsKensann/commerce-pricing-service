using System.Runtime.Serialization;

namespace commercepricing.infrastructure.Models
{
    /// <summary>
    /// Discount DTO: Different types of discount list
    /// </summary>
    public class DiscountDto
    {
        /// <summary>
        /// Type: e.g. Friends and Family
        /// </summary>
        [DataMember(Name = "type")]
        public string? Type { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "id")]
        public string? Id { get; set; }

        /// <summary>
        /// Description: e.g. F&F discount 123% off
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
