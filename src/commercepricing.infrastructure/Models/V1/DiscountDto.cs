using System.Runtime.Serialization;

namespace commercepricing.infrastructure.Models
{
    /// <summary>
    /// Discount DTO: Different types of discount list
    /// </summary>
    public class DiscountDto : BaseUpdateableModel<DiscountDto>
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

        public override void Update(DiscountDto model)
        {
            if (model == null || Id != model.Id)
                return;

            Type = model.Type ?? Type;
            Description = model.Description ?? Description;
            Amount = model.Amount ?? Amount;
            CurrencyCode = model.CurrencyCode ?? CurrencyCode;
            Quantity = model.Quantity ?? Quantity;

        }
    }
}
