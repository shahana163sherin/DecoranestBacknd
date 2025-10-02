using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserProductDTO>> GetAllProductAsync()
        {
            return await _context.Products.Select(p => new UserProductDTO
            {
                ProductId = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                Category = p.Category.CategoryName,
                Description = p.Description,
                ImageUrl = p.ImgUrl
            }).ToListAsync();
        }
        public async Task<UserProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.Where(p => p.ProductID == id)
                .Select(p => new UserProductDTO
                {
                    ProductId = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = p.Category.CategoryName,
                    Description = p.Description,
                    ImageUrl = p.ImgUrl
                }).FirstOrDefaultAsync();

            return product;
        }
        public async Task<IEnumerable<UserProductDTO>> GetProductByCategoryAsync(string category)
        {
            var product = await _context.Products.Where(p => p.Category.CategoryName == category)
                .Select(p => new UserProductDTO
                {
                    ProductId = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = p.Category.CategoryName,
                    Description = p.Description,
                    ImageUrl = p.ImgUrl
                }).ToListAsync();
            return product;
        }
    }
}