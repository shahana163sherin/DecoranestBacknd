using DecoranestBacknd.DecoraNest.Core.Entities;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface ICategory
    {
        Task<IEnumerable<Object>> GetAllCategoryAsync();
        Task<Object> GetCategoryById(int categoryId);
        //Task<bool> AddCategoryAsync(Category category);
        //Task<bool> UpdateCategoryAsync(int categoryId,Category category);
        //Task<bool>DeleteCategoryAsync(int categoryId);
    }
}
