using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<UserProductDTO>> GetAllProductAsync();
    }
}
