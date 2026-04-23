using Asp.Versioning;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.DecoraNest.Core.Services;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users/[controller]")]
    [Authorize (Roles ="User")] 

    public class WishlistController:ControllerBase
    {
        private readonly IWishlist _wishlist;
        public WishlistController(IWishlist wishlist)
        {
            _wishlist = wishlist;
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddToWishlist(AddWishDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { Status = "Error", Message = "Invalid Token" });

            }

            int userid = int.Parse(userIdClaim);
            try
            {
                var wish = await _wishlist.AddToWishList(userid, dto.ProductId);
                var response = new
                {
                    Status = "Success",
                    Message = "Product added to wishlist",
                    Data = wish
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("AllWishlist")]
        public async Task<IActionResult> GetAllWishList()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new Exception("Invalid Token");
                }
                var userid = int.Parse(userIdClaim);

                var wishItems = await _wishlist.GetWishList(userid);
                return Ok(new { Status = "Success",
                    Data = wishItems 
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
        [HttpDelete("RemoveFromWish")]
        public async Task<IActionResult> RemoveFromWish(int wishlistid)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new Exception("Invalid Token");
            }


            var userid = int.Parse(userIdClaim);
            var removed = await _wishlist.RemoveFromWishAsync(userid, wishlistid);
            if (!removed)
            {
                return NotFound("Items Not Found");
            }

            return Ok(new
            {

                Status = "Success",
                Message = "Removed From Wishlist"
            });


        }

        [HttpPost("toggle-wishlist")]
        public async Task<IActionResult> ToggleWishList([FromBody] TooglewishDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    return Unauthorized(new { status = "error", message = "Invalid token" });

                var result = await _wishlist.ToggleWishListAsync(userId, dto.productId);

                if (result == null || result.WishlistId == 0)
                {
                    return Ok(new { status = "success", message = "Product removed from wishlist.", wishlist = new List<object>() });
                }

                return Ok(new { status = "success", message = "Wishlist updated.", wishlist = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }


    }
}

