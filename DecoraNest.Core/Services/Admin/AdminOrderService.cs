using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using DecoranestBacknd.Infrastructure.Data;
using DecoranestBacknd.DecoraNest.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace DecoranestBacknd.DecoraNest.Core.Services.Admin
{
    public class AdminOrderService:IAdminOrderService
    {
        private readonly ApplicationDbContext _context;
        public AdminOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        private AdminOrderDTO MapToDTO(Entities.Order o) => new AdminOrderDTO
        {
            OrdeId = o.OrderID,
            Username = o.User?.Name ?? "N/A",
            Email = o.User?.Email ?? "N/A",
            Status = o.Status,
            TotalAmount = o.TotalAmount,
            PaymentStatus = o.Payment?.Status ?? "Not Paid",
            OrderDate = o.OrderDate,
            Address = o.Address,
            Items = o.Items?.Select(i => new AdminOrderItemDTO
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                Price = i.Price,
                ImgUrl = i.ImgUrl
            }).ToList() ?? new List<AdminOrderItemDTO>()
        };



        public async Task<PagedResult<AdminOrderDTO>> GetAllOrdersAsync(int pagenumber, int limit)
        {
            var totalOrders = await _context.Orders.CountAsync();
            var orders = await _context.Orders
                .Include(u => u.User)
                .Include(u => u.Items)
                .Include(u=>u.Payment)
                .OrderBy(o=>o.OrderID)
                .Skip((pagenumber - 1) * limit)
                .Take(limit)
                .ToListAsync();
            var totalpages = (int)Math.Ceiling((double)totalOrders / limit);
            return new PagedResult<AdminOrderDTO>
            {
                Items = orders.Select(MapToDTO).ToList(),
                CurrentPage = pagenumber,
                PageSize = limit,
                TotalItems = totalOrders,
                TotalPages = totalpages
            };
        }

        public async Task<ApiResponse<AdminOrderDTO?>> GetOrderByIdAsync(int id)
        {
            var order=await _context.Orders
                .Include(u => u.User)
                .Include(u => u.Items)
                .Include(u => u.Payment)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return new ApiResponse<AdminOrderDTO?>
                {
                    Status = "Error",
                    Message = "Order not found",
                    Data = null
                };
            }
            return new ApiResponse<AdminOrderDTO?>
            {
                Status = "Success",
                Message = "Order found",
                Data = MapToDTO(order)
            };
        }

        public async Task<ApiResponse<bool>> UpdateOrderStatusAsync(int orderId, string status)
        {
            var product = await _context.Orders.FindAsync(orderId);
            if (product == null)
            {
                return new ApiResponse<bool>
                {
                    Status = "Error",
                    Message = "Order not found",
                    Data = false
                };
            }
            product.Status = status;
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Status = "Success",
                Message = "Order status updated successfully",
                Data = true
            };
        }
        public async Task<ApiResponse<bool>> DeleteOrderAsync(int orderId)
        {
            var ord = await _context.Orders.FindAsync(orderId);
            if(ord == null)
            {
                return new ApiResponse<bool>
                {
                    Status = "Error",
                    Message = "Order not found",
                    Data = false
                };
            }
            _context.Orders.Remove(ord);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>
            {
                Status = "Success",
                Message = "Order deleted successfully",
                Data = true
            };
        }

        public async Task<ApiResponse<IEnumerable<AdminOrderDTO>>> SearchOrders(string username)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.Items)
                .Where(o => o.User != null && o.User.Name.ToLower().Contains(username.ToLower()))
                .ToListAsync();

            var result = orders.Select(o => MapToDTO(o)).ToList();

            if (result == null || result.Count == 0)
            {
                return new ApiResponse<IEnumerable<AdminOrderDTO>>
                {
                    Status = "Error",
                    Message = "No orders found for the given username",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<AdminOrderDTO>>
            {
                Status = "Success",
                Message = "Orders found",
                Data = result
            };
        }

        // 🔹 Get orders by status
        public async Task<ApiResponse<IEnumerable<AdminOrderDTO>>> GetOrdersByStatus(string status)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.Items)
                .Where(o => o.Status.ToLower().Contains(status.ToLower()))
                .ToListAsync();

            var result = orders.Select(o => MapToDTO(o)).ToList();

            if (result == null || result.Count == 0)
            {
                return new ApiResponse<IEnumerable<AdminOrderDTO>>
                {
                    Status = "Error",
                    Message = "No orders found for the given status",
                    Data = null
                };
            }

            return new ApiResponse<IEnumerable<AdminOrderDTO>>
            {
                Status = "Success",
                Message = "Orders found",
                Data = result
            };
        }

        // 🔹 Sort orders by date (ascending/descending)
        public async Task<ApiResponse<IEnumerable<AdminOrderDTO>>> SortOrdersByDate(bool ascending)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Payment)
                .Include(o => o.Items);

            var orders = ascending
                ? await query.OrderBy(o => o.OrderDate).ToListAsync()
                : await query.OrderByDescending(o => o.OrderDate).ToListAsync();

            var result = orders.Select(o => MapToDTO(o)).ToList();

            return new ApiResponse<IEnumerable<AdminOrderDTO>>
            {
                Status = "Success",
                Message = ascending ? "Orders sorted ascending (oldest first)" : "Orders sorted descending (recent first)",
                Data = result
            };
        }


    }
}
