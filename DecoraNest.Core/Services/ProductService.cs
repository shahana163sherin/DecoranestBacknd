using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class ProductService:IProductService
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
                Category = p.Category,
                Description = p.Description,
                ImageUrl = p.ImgUrl
            })
        }
    }
}
