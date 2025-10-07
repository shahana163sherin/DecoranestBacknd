using Asp.Versioning;
using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
    

        public async Task<IActionResult> GetAllProductAsync()
        {
           var products= await _productService.GetAllProductAsync();
            var response = new ApiResponse<IEnumerable<UserProductDTO>>
            {
                Status = "Success",
                Message = "Products fetched succesfully",
                Data = products
            };
            return Ok(response);
        }

        [HttpGet(":id")]
        public async Task<Object> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            var response = new ApiResponse<UserProductDTO>
            {
                Status = "Success",
                Message = "Product fetched by id",
                Data = product
            };
            return Ok(response);
        }

        [HttpGet("category")]

        public async Task<IActionResult> GetProductByCategoryAsync(string category)
        {
            var products = await _productService.GetProductByCategoryAsync(category);
            var response = new ApiResponse<IEnumerable<UserProductDTO>>
            {
                Status="Success",
                Message="Product by category",
                Data=products
            };
            return Ok(response);
        }
        
    }
}
