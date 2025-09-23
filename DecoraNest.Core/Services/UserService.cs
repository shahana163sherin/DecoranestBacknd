using BCrypt.Net;
using DecoranestBacknd.Configurations;
using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly JWTSettings _jwtSettings;
        public UserService(ApplicationDbContext context, IOptions<JWTSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        }),
                NotBefore = DateTime.UtcNow, // token valid immediately
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays).AddSeconds(1), // ensure it's after NotBefore
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<Object> RegisterUserAsync(UserRegisterDTO dto)

        {
            try
            {
                if (_context.Users.Any(u => u.Email == dto.Email))
                {
                    throw new Exception("User with this email already exists");
                }
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                var user = new User
                {
                    Name = dto.UserName,
                    Email = dto.Email,
                    Password = hashedPassword,
                    Role = "User"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var jwtToken = GenerateJwtToken(user);
                return new
                {
                    status = "success",
                    jwt_token = jwtToken,
                    user = new
                    {
                        name = user.Name,
                        email = user.Email,
                        Role = user.Role
                    }
                };

               
            }
            catch(Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }
       

        public async Task<Object>LoginUserAsync(UserLoginDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email==dto.Email);
                if(user == null)
                {
                   return new {status="error",message="User Not Found"};
                }
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
                if (!isPasswordValid)
                {
                   return new { status = "error", message = "Invalid password" };
                }
                if (user.IsBlocked)
                {
                    return new { status = "error", message = "User is blocked" };
                }
                var jwtToken = GenerateJwtToken(user);
                return new
                {
                    status = "success",
                    jwt_token = jwtToken,
                    user = new
                    {
                        name = user.Name,
                        email = user.Email,
                        
                    }
                };


            }
            catch(Exception ex)
            {
                return "Error logging in: " + ex.Message;
            }
        }

        public async Task<bool>ResetPasswordAsync(UserResetPasswordDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                {
                    return false;
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                                throw new Exception("Error resetting password: " + ex.Message);
            }
        }

    }
}
