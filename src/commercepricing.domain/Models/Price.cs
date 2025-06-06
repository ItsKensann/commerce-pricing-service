namespace commercepricing.domain.Models
{
    public class Price
    {
        public string PriceId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public string? Promotion { get; set; }
        public string PriceType { get; set; } = string.Empty;
        public string PriceGroup { get; set; } = string.Empty;
        public string Store { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
    }
}