using Asp.Versioning;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.DecoraNest.Core.Services;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users/[controller]")]
    [Authorize(Roles = "User")]

    public class PaymentController:ControllerBase
    {
        private readonly IPaymentService pay;
        public PaymentController(IPaymentService _pay)
        {
            pay = _pay;
        }

        [HttpPost("Pay")]

        public async Task<IActionResult> CreatePayment([FromBody] CreatePayDTO dto) {
            var razorpayOrderId = await pay.CreatePaymentAsync(dto.OrderId,dto.Amount);
            return Ok(new { razorpayOrderId });
        }

        [HttpPost("Verify")]

        public async Task<IActionResult>VerifyPayment(PaymentDTO dto)
        {
            bool isValid = await pay.VerifyPayment(dto);
            if (isValid)
                return Ok("Payment Verified Successfully");
            return BadRequest("Payment Verification Failed");
        }
    }
}
