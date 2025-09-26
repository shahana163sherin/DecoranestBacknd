using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IWishlist
    {
        Task<WishListDTO> AddToWishList(int userid, int productid);
        Task<List<WishListDTO>> GetWishList(int userid);
        Task<bool> RemoveFromWishAsync(int userid, int wishlistid);

    }
}
