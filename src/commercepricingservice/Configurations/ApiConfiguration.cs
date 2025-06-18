namespace commercepricingservice.Configurations
{
    /// <summary>
    /// Configuration for API-related settings
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        /// Base URL for the API gateway to be used in pagination links
        /// If not set, the request host will be used (perfect for local development)
        /// </summary>
        public string GatewayBaseUrl { get; set; }

        /// <summary>
        /// Gets the normalized gateway base URL, ensuring it ends with a trailing slash
        /// </summary>
        public string NormalizedGatewayBaseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(GatewayBaseUrl))
                {
                    return null!;
                }

                return GatewayBaseUrl.EndsWith("/")
                    ? GatewayBaseUrl
                    : GatewayBaseUrl + "/";
            }
        }

        /// <summary>
        /// Mapping of API versions to path suffixes in APIM
        /// </summary>
        public Dictionary<string, string> VersionPathMappings { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the appropriate path suffix for a specific API version in APIM
        /// Only used when GatewayBaseUrl is set (i.e., in deployed environments)
        /// </summary>
        public string GetPathSuffixForVersion(string version)
        {
            return VersionPathMappings.TryGetValue(version, out var suffix) ? suffix : string.Empty;
        }
    }
}
