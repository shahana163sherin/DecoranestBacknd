using DecoranestBacknd.DecoraNest.Core.Entities;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Cart?> GetCartByUserId(int userid);
        Task<User?> GetUserById(int userid);
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetOrderByUserIdAsync(int userid);
        Task<Order?> GetOrderByIdAsync(int orderId, int userId, bool includePayment = false);

        Task SaveChangesAsync();
        Task RemoveCartItemsAsync(Cart cart);


    }
}
