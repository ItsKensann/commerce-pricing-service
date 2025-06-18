using commercepricingservice.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace commercepricingservice.Common.Caching
{
    /// <summary>
    /// Distributed Cache Service            
    /// </summary>
    public class DistributedCacheService : IDistributedCacheWrapper
    {
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Class constructor            
        /// </summary>
        /// <param name="distributedCache">IDistributedCache</param>
        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Get caching with key value non-async method.           
        /// </summary>
        /// <param name="key">Application settings</param>
        /// <returns>This method returns the caching using key as the param.</returns>
        public string GetString(string key)
        {
            return _distributedCache.GetString(key)!;
        }

        /// <summary>
        /// Set caching with string value non-async method.           
        /// </summary>
        /// <param name="value">Value for caching.</param>
        /// <returns>This method returns the caching key for confirmation.</returns>
        public string SetString(string value)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(1)
            };
            string key = Guid.NewGuid().ToString();

            _distributedCache.SetString(key, value, options);
            return key;
        }

        /// <summary>
        /// Set caching non-async method.          
        /// </summary>
        /// <param name="key">string key</param>
        /// <param name="value">string value</param>
        /// <param name="options">DistributedCacheEntryOptions</param>
        public void SetString(string key, string value, DistributedCacheEntryOptions options)
        {
            _distributedCache.SetString(key, value, options);
        }

        /// <summary>
        /// Get caching async method.           
        /// </summary>
        /// <param name="key">Using the key string</param>
        /// <returns>Get the string value from the stored cache key.</returns>
        public async Task<string> GetStringAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key)?? string.Empty;
        }

        /// <summary>
        /// Set caching async method.            
        /// </summary>
        /// <param name="value">Value for caching.</param>
        /// <returns>This method returns the caching key for confirmation.</returns>
        public async Task<string> SetStringAsync(string value)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(1)
            };
            string key = Guid.NewGuid().ToString();

            await _distributedCache.SetStringAsync(key, value, options);
            return key;
        }

        /// <summary>
        /// Removing cache data using key string.            
        /// </summary>
        /// <param name="key">string key value</param>
        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
