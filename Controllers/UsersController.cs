using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.DecoraNest.Core.Services;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("/users/Register")]
        public async Task<IActionResult> Register(UserRegisterDTO dto)
        {
            try
            {
                if(dto == null || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                                       return BadRequest("All fields are required");
                }
                var user = await _userService.RegisterUserAsync(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        [HttpPost("/users/Login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request");
                }
                var result = await _userService.LoginUserAsync(dto);
                return Ok(result);
                //if(result== "login Successful")
                //{
                //    return Ok(new { message = result });
                //}
                //return Unauthorized(new { message = result });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("/users/NewPassword")]
        public async Task<IActionResult>NewPassword(UserResetPasswordDTO dto)
        {
            try
            {
                if(dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.NewPassword))
                {
                    return BadRequest("Email and password are required");
                }
                var result = await _userService.ResetPasswordAsync(dto);
                if (!result)
                {
                    return NotFound("User Not Found");
                }
                return Ok(new
                {
                    status = "success",
                    user = new
                    {
                        Email = dto.Email,
                        NewPassword = dto.NewPassword
                    }
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
