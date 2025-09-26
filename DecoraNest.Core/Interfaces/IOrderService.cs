using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreteOrderAsync(int userid, string address);
        Task<List<OrderDTO>> GetAllOrderAsync(int userid);
        Task<bool> CancelOrderAsync(int orderId, int userId);


    }
}
