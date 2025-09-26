using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddToCartAsync(int? userId, int productId, int quantity)
        {
            
            if (userId <= 0)
                throw new Exception("Invalid user ID");

           
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = userId,
                    AddedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

          
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

          
            var existingItem = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
            }

            await _context.SaveChangesAsync();
            return cart;
        }


           public async Task<CartDto> GetAllCartItemsAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("Invalid user ID");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart == null || cart.CartItems.Count == 0)
            {

                return new CartDto
                {
                    CartId = cart?.CartId ?? 0,
                    AddedAt = DateTime.UtcNow,
                    CartItems = new List<CartItemDto>(),
                    Message = "There are no items in the cart"
                };
            }

            return new CartDto
            {
                CartId = cart.CartId,
                UserID =userId,
                AddedAt = cart.AddedAt,
                Message="Successfully completed",
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                    Product = new ProductDto
                    {
                        Id = ci.Product.ProductID,
                        Name = ci.Product.ProductName,
                        Price = ci.Product.Price
                    }
                }).ToList()
            };
        }

        public async Task<CartDto> RemoveItemAsync(int userId, int CartItemId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if(cart == null || cart.CartItems.Count == 0)
            {
                throw new Exception("Cart is empty");
            }

            var cartItem=cart.CartItems.FirstOrDefault(c=>c.CartItemId == CartItemId);
            if(cartItem == null)
            {
                throw new Exception("Cart item is not found");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return await GetAllCartItemsAsync(userId);
        }


        public async Task<CartDto> ClearItemsAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if(cart == null || cart.CartItems.Count == 0)
            {
                throw new Exception("Cart is already empty");
            }

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return await GetAllCartItemsAsync(userId);
        }

    }
}







