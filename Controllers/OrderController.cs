using Asp.Versioning;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [Authorize (Roles ="User")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user/[controller]")]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _ord;
        public OrderController(IOrderService ord)
        {
            _ord = ord;
        }

        [HttpPost("Create")]
        public async Task<IActionResult>CreateOrder([FromBody] CreateOrderDTO model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new
                    {
                        Status = "Error",
                        Message = "Invalid Token"
                    });
                }
                int userId = int.Parse(userIdClaim);

                var order = await _ord.CreteOrderAsync(userId, model.Address);
                return Ok(new
                {
                    Status = "Success",
                    Data = order
                });

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AllOrders")]
        public async Task<IActionResult> GetAllOrder()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Invalid Token");
                }
                
                int userId = int.Parse(userIdClaim);
                var order = await _ord.GetAllOrderAsync(userId);
                return Ok(new
                {

                    Status = "Success",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {   Status="Error",
                    Message=ex.Message
                });
            }
        }
        [HttpPost("Cancel")]
        public async Task<IActionResult> CancelOrder([FromBody]CancelOrderDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Invalid Token");
            }

            int userid = int.Parse(userIdClaim);

            var result = await _ord.CancelOrderAsync(dto.OrderId, userid);

            if (!result)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Order Not Found"
                });
            }

            return Ok(new
            {
                Status = "Success",
                Message = "Order Cancelled Successfully"
            });
        }


    }
}
