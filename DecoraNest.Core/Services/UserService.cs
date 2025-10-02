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
using System.Security.Cryptography;
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
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpirationInDays).AddSeconds(1),
                Issuer = _jwtSettings.Issuer,         
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,       // ✅ important
                CreatedByIp = "system",          // or pass real IP
                //IsActive = true,
                User_Id = user.User_Id
            };
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

                //var jwtToken = GenerateJwtToken(user);
                return new
                {
                    status = "success",
                    //jwt_token = jwtToken,
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


        public async Task<Object> LoginUserAsync(UserLoginDTO dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                {
                    return new { status = "error", message = "User Not Found" };
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

                var refreshToken = GenerateRefreshToken(user);
                refreshToken.User = user;  // explicitly link
                try
                {
                    _context.RefreshTokens.Add(refreshToken);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error saving refresh token: " + ex.InnerException?.Message ?? ex.Message);
                }


                return new
                {
                    status = "success",
                    jwt_token = jwtToken,
                    refresh_token=refreshToken.Token,
                    user = new
                    {
                        name = user.Name,
                        email = user.Email,

                    }
                };


            }
            catch (Exception ex)
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

        public async Task<Object> RefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                return new { status = "error", message = "Invalid or expired refresh token" };
            }

            // Revoke old refresh token
            refreshToken.Revoked = DateTime.UtcNow;

            // Generate new JWT & refresh token
            var jwtToken = GenerateJwtToken(refreshToken.User);
            var newRefreshToken = GenerateRefreshToken(refreshToken.User);

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new
            {
                status = "success",
                jwt_token = jwtToken,
                refresh_token = newRefreshToken.Token
            };
        }


    }
}
