namespace commercepricingservice.Common.Interfaces
{
    public interface IDistributedCacheWrapper
    {
        string GetString(string key);

        string SetString(string value);

        Task<string> GetStringAsync(string key);

        Task<string> SetStringAsync(string value);

        void Remove(string key);
    }
}
