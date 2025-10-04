using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    public class AdminProductController:ControllerBase
    {
        private readonly IAdminProductService _service;
        public AdminProductController(IAdminProductService service)
        {
            _service = service;
        }

        [HttpGet("AllProducts")]
        public async Task<IActionResult>GetAllProduct(int pagenumber,int limit)
        {
            var result = await _service.GetAllProductsAsync(pagenumber, limit);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult>GetProductById(int id)
        {
            var result = await _service.GetProductByIdAsync(id);
               if(result.Data == null)
               {             
                return NotFound(result);
               }
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult>AddProduct([FromForm]ProductUpdateCreateDTO dto)
        {
            var result = await _service.AddProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Data?.ProductId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>EditProduct(int id, [FromForm] ProductUpdateCreateDTO dto)
        {
            var product = await _service.UpdateProductAsync(id, dto);
            if(product.Data == null)
            {
                return NotFound(new { status = "error", message = "Product not found" });
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            var del = await _service.DeleteProductAsync(id);
            if(del.Status == "error")
            {
                return BadRequest(del);
            }
            return Ok(del);
        }
        [HttpGet("SearchByName")]
        public async Task<IActionResult>SearchByName(string name)
        {
            var product = await _service.SearchByName(name);
            if(product.Data == null)
            {
                return NotFound(product);
            }
            return Ok(product);
        }

        [HttpGet("GetByCategory")]
        public async Task<IActionResult> GetByCategory(string categoryName)
        {
            var cat = await _service.GetByCategory(categoryName);
            if (cat.Data == null)
            {
                return BadRequest(cat);
            }
            return Ok(cat);
        }
    }
}
