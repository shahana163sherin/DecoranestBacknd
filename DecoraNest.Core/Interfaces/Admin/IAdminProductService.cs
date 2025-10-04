using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces.Admin
{
    public interface IAdminProductService
    {
        Task<PagedResult<AdminProductDTO>> GetAllProductsAsync(int pagenumber,int limit);
        Task<ApiResponse<AdminProductDTO?>> GetProductByIdAsync(int id);
        Task<ApiResponse<AdminProductDTO>> AddProductAsync(ProductUpdateCreateDTO dto);
        Task<ApiResponse<AdminProductDTO?>> UpdateProductAsync(int id, ProductUpdateCreateDTO dto);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
        Task<ApiResponse<AdminProductDTO?>> SearchByName(string name);
        Task<ApiResponse<IEnumerable<AdminProductDTO>>>GetByCategory(string categoryName);
    }
}
