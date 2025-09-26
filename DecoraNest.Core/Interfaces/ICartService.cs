using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface ICartService
    {
        Task<Cart> AddToCartAsync(int? userId, int ProductId, int quantity);
        Task<CartDto> GetAllCartItemsAsync(int userId);
        Task<CartDto> RemoveItemAsync(int userId, int CartItemId);
        Task<CartDto> ClearItemsAsync(int userId);


    }
}
