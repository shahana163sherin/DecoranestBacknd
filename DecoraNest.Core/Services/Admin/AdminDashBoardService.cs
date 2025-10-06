using DecoranestBacknd.DecoraNest.Core.Interfaces.Admin;
using DecoranestBacknd.Ecommerce.Shared.DTO.Adminn;
using DecoranestBacknd.Ecommerce.Shared.Responses;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net.Sockets;

namespace DecoranestBacknd.DecoraNest.Core.Services.Admin
{
    public class AdminDashBoardService:IAdminDashBoardService
    {
        private readonly ApplicationDbContext _context;
        public AdminDashBoardService(ApplicationDbContext context)
        {
            _context = context;
        }

      public async Task<ApiResponse<AdminDashBoardDTO>> GetDashBoardDataAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUses = await _context.Users.CountAsync(u => u.IsBlocked == false);
            var totalOrders=await _context.Orders.CountAsync();
            var pendingOrders=await _context.Orders.CountAsync(o => o.Status == "Pending");
            var deliveredOrders=await _context.Orders.CountAsync(o => o.Status == "Delivered");
            var totalProducts=await _context.Products.CountAsync();
            var totalRevenue=await _context.Orders.Where(o => o.Status == "Delivered").SumAsync(o => o.TotalAmount);
            var data = await _context.Orders
             .Where(o => o.Status == "Delivered")
             .GroupBy(o => o.OrderDate.Month)
             .Select(g => new { Month = g.Key, Revenue = g.Sum(x => x.TotalAmount) })
             .ToListAsync();

            var monthlyRevenue = data.Select(x => new MonthlyRevenueDTO
            {
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month),
                Revenue = x.Revenue
            }).ToList();
            var topProducts = await _context.OrderItems
                //.Include(oi => oi.Product)
                .GroupBy(oi => oi.ProductName)
                .Select(g => new TopSellingDTO
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToListAsync();

            return new ApiResponse<AdminDashBoardDTO>
            {
                Data = new AdminDashBoardDTO
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUses,
                    TotalOrders = totalOrders,
                    PendingOrders = pendingOrders,
                    DeliveredOrders = deliveredOrders,
                    TotalProducts = totalProducts,
                    TotalRevenue = totalRevenue,
                    MonthlyRevenues = monthlyRevenue,
                    TopSellingProducts = topProducts
                },
                Status = "Success",
                Message = "Dashboard data retrieved successfully"
            };
        }
    }
}
