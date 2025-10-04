using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using DecoranestBacknd.Infrastructure.Data;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces.Admin
{
    public interface IAdminCategoryService
    {
       Task<IEnumerable<AdminCategoryDTO>> GetAllCategoriesAsync();
        Task<ApiResponse<AdminCategoryDTO?>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<AdminCategoryDTO>> AddCategoryAsync(AdminCreateCategoryDTO dto);
        Task<ApiResponse<AdminCategoryDTO?>> UpdateCategoyAsync(int id, AdminCategoryDTO dto);
        Task<ApiResponse<bool>> DeleteCategoryAsync(int id);
    }
}
