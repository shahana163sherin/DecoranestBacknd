using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController:ControllerBase
    {
        private readonly IAdminUserService _service;
        public AdminUsersController(IAdminUserService service)
        {
            _service = service;
        }

        [HttpGet("AllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetByRole/{role}")]
        public async Task<IActionResult> GetByRole(string role)
        {
            var user = await _service.GetAllUsersByRoleAsync(role);
            if(user == null || !user.Any())
            {
                return NotFound($"No users found with role: {role}");
            }
            return Ok(user);
        }

        [HttpGet("GetByStatus/{isBlocked}")]
        public async Task<IActionResult>GetByStatus(bool isBlocked)
        {
            var users = await _service.GetAllUsersByStatus(isBlocked);
            if(users == null || !users.Any())
            {
                return NotFound($"No users found with status: {(isBlocked ? "Blocked" : "Active")}");
            }
            return Ok(users);
        }

        [HttpGet("SortByDate")]
        public async Task<IActionResult> SortByDate([FromQuery] bool ascending = true)
        {
            var users = await _service.SortUserByDateAsync(ascending);
            if(users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("SearchByEmail")]
        public async Task<IActionResult> SearchByEmail([FromQuery] string email)
        {
            var user = await _service.SearchUserEmailAsync(email);
            if(user == null)
            {
                return NotFound($"No user found with email: {email}");
            }
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                               return NotFound($"No user found with ID: {id}");
            }
            return Ok(user);
        }

        [HttpPut("BlockUnblock/{id}")]
        public async Task<IActionResult>BlockUnblock(int id)
        {
            var user=await _service.BlockUnblockUserAsync(id);
            if(user == null)
            {
                return NotFound($"No user found with ID: {id}");
            }
            return Ok(new { message =$"User {user}" });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteUser(int id)
        {
            var user = await _service.DeleteUserAsync(id);
            if (!user)
            {
                return NotFound($"No user found with ID: {id}");
            }
            return Ok(new { message = "User deleted successfully" });
        }

    }
}
