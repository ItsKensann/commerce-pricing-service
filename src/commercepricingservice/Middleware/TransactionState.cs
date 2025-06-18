namespace commercepricingservice.Middleware
{
    /// <summary>
    /// Transaction State information
    /// </summary>
    public class TransactionState
    {
        /// <summary>
        /// Transaction Id
        /// </summary>
        public string? TransactionId { get; set; }

        /// <summary>
        /// Operation Name
        /// </summary>
        public string? OperationName { get; set; }

        /// <summary>
        /// Message Source
        /// </summary>
        public string? MessageSource { get; set; }
    }
}
