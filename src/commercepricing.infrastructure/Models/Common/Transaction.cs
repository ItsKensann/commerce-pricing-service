using commercepricing.infrastructure.Interfaces;

namespace commercepricing.infrastructure.Models
{
    public class Transaction<PayloadDomainType, PayloadIdType>
        where PayloadDomainType : IHasId<PayloadIdType>
        where PayloadIdType : class
    {
        public Guid Id { get; set; }
        public string? EventType { get; set; }
        public PayloadDomainType? Payload { get; set; }
    }
}