using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces.Admin
{
    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync();
        Task<IEnumerable<AdminUserDTO>> GetAllUsersByRoleAsync(string role);
        Task<IEnumerable<AdminUserDTO>> GetAllUsersByStatus(bool isBlocked);
        Task<IEnumerable<AdminUserDTO>> SortUserByDateAsync(bool ascending = true);
        Task<AdminUserDTO?> SearchUserEmailAsync(string email);
        Task<AdminUserDTO?> GetUserByIdAsync(int id);
        Task <string?> BlockUnblockUserAsync(int id);
        //Task<bool> DeleteUserAsync(int id);



    }
}
