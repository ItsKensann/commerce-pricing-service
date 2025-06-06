using commercepricing.domain.Models;

namespace commercepricing.domain.Interfaces
{
    public interface IPriceRepository
    {
        Task<Price> GetPriceAsync(string productId);
        Task<IEnumerable<Price>> GetAllPricesAsync();
    }
}