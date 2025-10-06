using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces.Admin
{
    public interface IAdminDashBoardService
    {
        Task <ApiResponse<AdminDashBoardDTO>> GetDashBoardDataAsync();
    }
}
