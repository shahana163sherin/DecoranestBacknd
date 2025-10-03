

using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [Route("api/users/[controller]")]
    [Authorize (Roles ="User")] 
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
//-------------------------------------------------------------------------------------------------------------------

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
        {
            try
            {
                
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { status = "error", message = "Invalid token" });
                }

                int userId = int.Parse(userIdClaim);

                
                var cart = await _cartService.AddToCartAsync(userId, dto.ProductID, dto.Quantity);

                var response = new
                {
                    status = "success",
                    message = "Product added to cart",
                    data = new
                    {
                        UserId = userId,
                        ProductId = dto.ProductID,
                        Quantity = dto.Quantity
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }
//-------------------------------------------------------------------------------------------------->
        [HttpGet("Items")]
        public async Task<IActionResult> GetAllCartItem()
        {
            try { 
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { Status = "Error", Message = "Invalid token" });
            }
            int userid = int.Parse(userIdClaim);
            var cart = await _cartService.GetAllCartItemsAsync(userid);
            return Ok(new
            {
                Status = "Success",
                Data = cart
            });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = ex.Message
                });
            }

        }
 //-------------------------------------------------------------------------------------------------->

        [HttpDelete("RemoveItem/{cartItemId}")]

        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { Status = "Error", Message = "Invalid Token" });
                }
                int userid = int.Parse(userIdClaim);
                var cartDto = await _cartService.RemoveItemAsync(userid, cartItemId);

                return Ok(new { Status = "Success", Data = cartDto });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //-------------------------------------------------------------------------------------------------->

        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { Status="Error",Message="Invalid Token" });
                }
                var userId = int.Parse(userIdClaim);
                var cartDto = _cartService.ClearItemsAsync(userId);
                return Ok(new { Status = "Success", Data = cartDto });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Error", Message = ex.Message });

            }
        }
    }
}



