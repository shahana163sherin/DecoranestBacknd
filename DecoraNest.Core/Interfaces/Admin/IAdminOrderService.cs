using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces.Admin
{
    public interface IAdminOrderService
    {
        Task<PagedResult<AdminOrderDTO>> GetAllOrdersAsync(int pagenumber, int limit);
        Task<ApiResponse<AdminOrderDTO?>> GetOrderByIdAsync(int id);
        Task<ApiResponse<bool>> UpdateOrderStatusAsync(int orderId, string status);
        Task<ApiResponse<bool>> DeleteOrderAsync(int orderId);
        Task<ApiResponse<IEnumerable<AdminOrderDTO>>> SearchOrders(string username);
        Task<ApiResponse<IEnumerable<AdminOrderDTO>>> GetOrdersByStatus(string status);
        Task<ApiResponse<IEnumerable<AdminOrderDTO>>>SortOrdersByDate(bool ascending);
        //Task<ApiResponse<IEnumerable<AdminOrderDTO>>>GetOrderByCategory(string categoryName);


    }
}
