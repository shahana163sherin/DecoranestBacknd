using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [Route("api/users/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;
        public CategoryController(ICategory category)
        {
            _category = category;
        }

        [HttpGet("All")]

        public async Task<IActionResult> GetAllCategory()
        {
            var category = await _category.GetAllCategoryAsync();
            return Ok(category);
        }
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _category.GetCategoryById(categoryId);
            if(category == null)
            {
                return NotFound("Category not Found");
            }
            return Ok(category);
        }
    }
}
