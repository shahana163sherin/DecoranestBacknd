using Azure;
using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController:ControllerBase
    {
        private readonly IAdminCategoryService _service;
        public AdminCategoryController(IAdminCategoryService service)
        {
            _service = service;
        }

        [HttpGet("AllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var category = await _service.GetAllCategoriesAsync();
            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetCategoryById(int id)
        {
            var result = await _service.GetCategoryByIdAsync(id);
            if(result.Data == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody]AdminCreateCategoryDTO dto)
        {
            var result = await _service.AddCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Data?.CategoryId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody]AdminCategoryDTO dto)
        {
            var result = await _service.UpdateCategoyAsync(id, dto);
            if(result.Data == null)
            {
                return NotFound(new { status = "error", message = "Category not found" });
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _service.DeleteCategoryAsync(id);
            if (result.Status == "error")
                return BadRequest(result);

            return Ok(result);
        }


    }
}
