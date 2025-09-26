using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<UserProductDTO>> GetAllProductAsync();
        Task<UserProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<UserProductDTO>> GetProductByCategoryAsync(string category);
    }
}
