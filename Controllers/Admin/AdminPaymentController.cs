using Asp.Versioning;
using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/payment")]
    [Authorize(Roles = "Admin")]
    public class AdminPaymentController:ControllerBase
    {
        private readonly IAdminPaymentService _pay;
        public AdminPaymentController (IAdminPaymentService pay)
        {
            _pay = pay;
        }

        [HttpGet("getall")]
        public async Task <IActionResult> GetAllPayment(int pagenumber=1,int limit=10)
        {
            var result = await _pay.GetAllPaymentAsync(pagenumber, limit);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task <IActionResult>GetById(int id)
        {
            var byId = await _pay.GetPaymentByIdAsync(id);
            if(byId.Data == null)
            {
                return NotFound(byId);
            }
            return Ok(byId);
        }

        [HttpGet("search")]
        public async Task<IActionResult>SearchByName(string username)
        {
            var search = await _pay.SearchByName(username);
            if(search.Data == null || !search.Data.Any())
            {
                return NotFound(search);
            }
            return Ok(search);
        }

        [HttpGet("status")]
        public async Task<IActionResult>GetByStatus(string status)
        {
            var stat = await _pay.GetByStatus(status);
            if(stat.Data == null || !stat.Data.Any())
            {
                return NotFound(stat);
            }
            return Ok(stat);
        }

        [HttpGet("bydate")]
        public async Task<IActionResult>SortbDate(bool ascending = true)
        {
            var sort = await _pay.SortByDate(ascending);
            return Ok(sort);
        }
        [HttpPut("update")]
        public async Task<IActionResult>UpdateStatus(PaymentUpdateDTO dto)
        {
            var upd = await _pay.UpdatePaymentStatusAsync(dto.Id,dto.Status);

            if(!upd.Data)
            {
                return BadRequest(upd);
            }
            return Ok(upd);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult>DeletePayment(int id)
        {
            var del = await _pay.DeletePaymentAsync(id);
            if(!del.Data)
            {
                return BadRequest(del);
            }
            return Ok(del);
        }
    }
}
