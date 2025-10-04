using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services.Admin
{
    public class AdminCategoryService:IAdminCategoryService
    {
        private readonly ApplicationDbContext _context;
        public AdminCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        private AdminCategoryDTO MapToDTO(Category c) => new AdminCategoryDTO
        {
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName
        };
        public async Task<IEnumerable<AdminCategoryDTO>> GetAllCategoriesAsync()
        {
            return await _context.Category
                .Select(c => new AdminCategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
        }

        public async Task<ApiResponse<AdminCategoryDTO?>> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
                       if(category == null)
            {
                return new ApiResponse<AdminCategoryDTO?>
                {
                    Status = "Error",
                    Message = "Category not found",
                    Data = null
                };
            }
            return new ApiResponse<AdminCategoryDTO?>
            {
                Status = "Success",
                Message = "Category found",
                Data = MapToDTO(category)
            };
        }

        public async Task<ApiResponse<AdminCategoryDTO>> AddCategoryAsync(AdminCreateCategoryDTO dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName
            };
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<AdminCategoryDTO>
            {
                Status = "Success",
                Message = "Category added successfully",
                Data = MapToDTO(category)
            };
        }

        public async Task<ApiResponse<AdminCategoryDTO?>> UpdateCategoyAsync(int id, AdminCategoryDTO dto)
        {
            var category = await _context.Category.FindAsync(id);
            if(category == null)
            {
                return new ApiResponse<AdminCategoryDTO?>
                {
                    Status = "Error",
                    Message = "Category not found",
                    Data = null
                };
            }
            category.CategoryName = dto.CategoryName;
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<AdminCategoryDTO?>
            {
                Status = "Success",
                Message = "Category updated successfully",
                Data = MapToDTO(category)
            };
        }

        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            var category = await _context.Category
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return new ApiResponse<bool>
                {
                    Status = "Error",
                    Message = "Category not found",
                    Data = false
                };
            }
            if (category.Products.Any())
            {
                return new ApiResponse<bool>
                {
                    Status = "error",
                    Message = "Cannot delete category with products",
                    Data = false
                };
            }
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Status = "Success",
                Message = "Category deleted successfully",
                Data = true
            };
        }


        
    }
}
