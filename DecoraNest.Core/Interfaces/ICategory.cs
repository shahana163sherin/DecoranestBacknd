using DecoranestBacknd.DecoraNest.Core.Entities;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface ICategory
    {
        Task<IEnumerable<object>> GetAllCategoryAsync();
        Task<object> GetCategoryById(int categoryId);
       
    }
}
