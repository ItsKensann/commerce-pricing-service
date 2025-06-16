using System;
using System.Diagnostics.CodeAnalysis;
using Columbia.Cosmos.Common.Interfaces;
using commercepricing.domain.Interfaces;

namespace commercepricing.domain.Models
{
    public class Transaction<PayloadDomainType, PayloadIdType>
        where PayloadDomainType : IHasId<PayloadIdType>
        where PayloadIdType : class
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public PayloadDomainType Payload { get; set; }
    }
}