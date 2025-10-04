using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DecoranestBacknd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/order")]
    [Authorize(Roles ="Admin")]
    public class AdminOrderController:ControllerBase
    {
        private readonly IAdminOrderService _service;
        public AdminOrderController(IAdminOrderService service)
        {
            _service = service;
        }

        [HttpGet("AllOrder")]
        public async Task <IActionResult>GetAllOrders(int pagenumber,int limit)
        {
            var result = await _service.GetAllOrdersAsync(pagenumber, limit);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult>GetOrderById(int id)
        {
            var result = await _service.GetOrderByIdAsync(id);
            if(result.Data == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult>UpdateOrder(int orderid,string status)
        {
            var result = await _service.UpdateOrderStatusAsync(orderid,status);
            if(!result.Data)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("deleteOrder")]
        public async Task<IActionResult>DeleteOrder(int orderid)
        {
            var response = await _service.DeleteOrderAsync(orderid);
            if(response.Status == "Error")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("searchByUsername")]
        public async Task<IActionResult>SearchByName(string username)
        {
            var search = await _service.SearchOrders(username);
            if(search.Data == null || !search.Data.Any())
            {
                return NotFound(search);
            }
            return Ok(search);
        }
        [HttpGet("filterByStatus")]
        public async Task<IActionResult>FilterByStatus(string status)
        {
            var filter = await _service.GetOrdersByStatus(status);
            if(filter.Data == null || !filter.Data.Any())
            {
                return NotFound(filter);
            }
            return Ok(filter);
        }
        [HttpGet("sortByDate")]
        public async Task<IActionResult>SortByDate(bool ascending=true)
        {
            var sort = await _service.SortOrdersByDate(ascending);
            if(sort.Data == null || !sort.Data.Any())
            {
                return NotFound(sort);
            }
            return Ok(sort);

        }
    }
}
