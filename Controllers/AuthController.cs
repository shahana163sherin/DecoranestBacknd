using Asp.Versioning;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.DecoraNest.Core.Services;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
    
        public AuthController(IUserService userService)
        {
            _userService = userService;
          

        }

        [HttpPost("Register")]
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


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { status = "error", message = "Invalid request" });
                }

                var result = await _userService.LoginUserAsync(dto);

                if (result == null || result.GetType().GetProperty("jwt_token")?.GetValue(result) == null)
                {
                    return Unauthorized(new { status = "error", message = "Invalid email or password" });
                }

                return Ok(result); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }



        [HttpPatch("NewPassword")]
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Token))
            {
                return BadRequest(new { status = "error", message = "Token is required" });
            }

            var result = await _userService.RefreshTokenAsync(dto.Token);

            if (result is null)
            {
                return Unauthorized(new { status = "error", message = "Invalid token" });
            }

            return Ok(result);
        }




    }

}
