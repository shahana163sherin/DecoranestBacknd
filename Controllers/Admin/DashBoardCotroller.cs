using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles ="Admin")]
    public class DashBoardCotroller:ControllerBase
    {
        private readonly IAdminDashBoardService _service;
        public DashBoardCotroller(IAdminDashBoardService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetDashBoardDataAsync()
        {
            var result = await _service.GetDashBoardDataAsync();
            if (result.Data == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

    }
}
