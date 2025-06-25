using System.Runtime.Serialization;

namespace commercepricing.infrastructure.Models
{
    /// <summary>
    /// Price DTO: Different types of pricing list
    /// </summary>
    [DataContract]
    public class PriceDto : BaseUpdateableModel<PriceDto>
    {
        /// <summary>
        /// Type: e.g. MAP, CTP, ASP, etc.
        /// </summary>
        [DataMember(Name = "type")]
        public string? Type { get; set; }

        /// <summary>
        /// Price Group Code: e.g. 'EMP' for Employee
        /// </summary>
        [DataMember(Name = "priceGroupCode")]
        public string? PriceGroupCode { get; set; }

        /// <summary>
        /// Price: Current pricing
        /// </summary>
        [DataMember(Name = "price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Prior Price
        /// </summary>
        [DataMember(Name = "priorPrice")]
        public decimal? PriorPrice { get; set; }

        /// <summary>
        /// Effective From Date Time
        /// </summary>
        [DataMember(Name = "effectiveFromDateTime")]
        public string? EffectiveFromDateTime { get; set; }

        /// <summary>
        /// Effective To Date Time
        /// </summary>
        [DataMember(Name = "effectiveToDateTime")]
        public string? EffectiveToDateTime { get; set; }

        public override void Update(PriceDto model)
        {
            if (model == null)
                return;

            Type = model.Type;
            PriceGroupCode = model.PriceGroupCode;
            Price = model.Price;
            PriorPrice = model.PriorPrice;
            EffectiveFromDateTime = model.EffectiveFromDateTime;
            EffectiveToDateTime = model.EffectiveToDateTime;

        }
    }

}