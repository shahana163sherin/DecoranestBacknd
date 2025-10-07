using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services.Admin
{
    public class AdminUserService:IAdminUserService
    {
        private readonly ApplicationDbContext _context;
        public AdminUserService(ApplicationDbContext context)
        {
            _context = context;
        }
        private AdminUserDTO MapToAdminUserDTO(User u)
        {
            return new AdminUserDTO
            {
                User_Id = u.User_Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                IsBlocked = u.IsBlocked,
                CreatedAt = u.CreateAt
            };
        }
        public async Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            
            return users.Select(MapToAdminUserDTO).ToList();
        }

        public async Task<IEnumerable<AdminUserDTO>> GetAllUsersByRoleAsync(string role)
        {
            var users= await _context.Users.Where(u=>u.Role==role).ToListAsync();
            return users.Select(MapToAdminUserDTO).ToList();
        }

        public async Task<IEnumerable<AdminUserDTO>> GetAllUsersByStatus(bool isBlocked)
        {
            var result= await _context.Users.Where(u=>u.IsBlocked==isBlocked).ToListAsync();
            return result.Select(MapToAdminUserDTO).ToList();
        }

        public async Task<IEnumerable<AdminUserDTO>> SortUserByDateAsync(bool ascending = true)
        {
            var query = ascending
                ? _context.Users.OrderBy(u => u.CreateAt)
                : _context.Users.OrderByDescending(u => u.CreateAt);

            var users = await query.ToListAsync();
            return users.Select(MapToAdminUserDTO).ToList();
        }

        public async Task<AdminUserDTO?> SearchUserEmailAsync(string email)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            return user == null ? null : MapToAdminUserDTO(user);
        }


       public async  Task<AdminUserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToAdminUserDTO(user);
        }

        public async Task<string?> BlockUnblockUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return null;
            }
            user.IsBlocked = !user.IsBlocked;
            await _context.SaveChangesAsync();
            if (user.IsBlocked)
            {
                return (string)"Blocked";
            }
            else
            {
                return (string)"Unblocked";
            }
        }
       // public async Task<bool> DeleteUserAsync(int id)
       // {
       //     var user = await _context.Users
       //.Include(u => u.Orders)
       //.Include(u => u.Items)
       //.Include(u => u.WishlistItems)
       //.FirstOrDefaultAsync(u => u.Id == id);
       //     if (user == null)
       //     {
       //         return false;
       //     }
       //     _context.Users.Remove(user);
       //     await _context.SaveChangesAsync();
       //     return true;
       // }

    }
}
