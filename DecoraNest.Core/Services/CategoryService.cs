using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class CategoryService:ICategory
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllCategoryAsync()
        {
            return await _context.Category
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    Products = c.Products.Select(p => new ProductDto
                    {
                        Id = p.ProductID,
                        Name = p.ProductName,
                        Price = p.Price
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<Object> GetCategoryById(int categoryId)
        {
            var category = await _context.Category
                .Where(c => c.CategoryId == categoryId)
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    Products = c.Products.Select(p => new ProductDto
                    {
                        Id = p.ProductID,
                        Name = p.ProductName,
                        Price = p.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return category;
        }


        //public async Task<bool> AddCategoryAsync(Category category)
        //{
        //    _context.Category.Add(category);
        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public async Task<bool> UpdateCategoryAsync(int categoryId, Category category)
        //{
        //    var existingCategory = await _context.Category.FindAsync(categoryId);
        //    if(existingCategory == null)
        //    {
        //        return false;
        //    }
        //    existingCategory.CategoryName = category.CategoryName;
        //    return await _context.SaveChangesAsync() > 0;
        //}

        //public async Task<bool> DeleteCategoryAsync(int categoryId)
        //{
        //    var category = await _context.Category.FindAsync(categoryId);
        //        if(category == null)
        //    {
        //        return false;
        //    }
        //    _context.Category.Remove(category);
        //    return await _context.SaveChangesAsync() > 0;

        //}


    }
}
