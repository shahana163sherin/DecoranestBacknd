using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Ecommerce.Shared.DTO;

namespace DecoranestBacknd.DecoraNest.Core.Interfaces
{
    public interface IUserService
    {
        Task<object> RegisterUserAsync(UserRegisterDTO dto);

        Task<object> LoginUserAsync(UserLoginDTO dto);
        Task<bool> ResetPasswordAsync(UserResetPasswordDTO dto);
        Task<object> RefreshTokenAsync(string token);


        //admin------------------------------>

        




    }
}
