using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces.Admin
{
    public interface IAdminPaymentService
    {
        Task<PagedResult<AdminPaymentDTO>> GetAllPaymentAsync(int pagenumber, int limit);
        Task<ApiResponse<AdminPaymentDTO?>> GetPaymentByIdAsync(int id);
        Task<ApiResponse<IEnumerable<AdminPaymentDTO>>> SearchByName(string username);
        Task<ApiResponse<IEnumerable<AdminPaymentDTO>>> GetByStatus(string status);
        Task<ApiResponse<IEnumerable<AdminPaymentDTO>>> SortByDate(bool ascending);
        Task<ApiResponse<bool>> UpdatePaymentStatusAsync(int paymentId, string status);
        Task<ApiResponse<bool>> DeletePaymentAsync(int paymentId);
    }
}
