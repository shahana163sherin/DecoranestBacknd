using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IUserService
    {
        Task<Object> RegisterUserAsync(UserRegisterDTO dto);

        Task<Object> LoginUserAsync(UserLoginDTO dto);
        Task<bool> ResetPasswordAsync(UserResetPasswordDTO dto);
        Task<Object> RefreshTokenAsync(string token);


        //admin------------------------------>

        




    }
}
