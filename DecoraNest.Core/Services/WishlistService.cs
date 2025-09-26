using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class WishlistService:IWishlist
    {
        private readonly ApplicationDbContext _context;
        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishListDTO> AddToWishList(int userid, int productid)
        {

            var user = await _context.Users.FindAsync(userid);
            if(user == null)
            {
                throw new Exception("Invalid user");
            }
            var product = await _context.Products.FindAsync(productid);
            if (product == null)
            {
                throw new Exception("Invalid Product");
            }

            var existing = await _context.Wishlists.FirstOrDefaultAsync(w =>w.UserID==userid && w.ProductId == productid);
            if (existing != null)
            {
                throw new Exception("Product already in wishlist");
            }
            var wishlist=new Wishlist
            {
                UserID = userid,
                ProductId=productid,
                Price = product.Price,
                AddedAt = DateTime.UtcNow
            };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            return new WishListDTO
            {
                WishlistId = wishlist.WishListID,
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                ImgUrl = product.ImgUrl,
                Price = product.Price,
                AddedAt = DateTime.UtcNow
            };

        }

        public async Task<List<WishListDTO>> GetWishList(int userid)
        {
            if (userid <= 0)
            {
                throw new Exception("Invalid user");
            }

            var wish = await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserID == userid)
                .ToListAsync();

            if(wish  == null || wish.Count == 0)
            {
                return new List<WishListDTO>();
            }
            return wish.Select(w => new WishListDTO
            {
                WishlistId = w.WishListID,
                ProductId = w.ProductId,
                ProductName = w.Product.ProductName,
                ImgUrl = w.Product.ImgUrl,
                Price = w.Price,
                AddedAt = w.AddedAt
            }).ToList();
        }

        public async Task<bool> RemoveFromWishAsync(int userid,int wishlistid)
        {
          var wishItem=_context.Wishlists.FirstOrDefault(w=>w.UserID == userid && w.WishListID==wishlistid);
            if (wishItem == null)
            {
                return false;
            }
             _context.Wishlists.Remove(wishItem);
             await _context.SaveChangesAsync();
            return true;

        }

    }
}
