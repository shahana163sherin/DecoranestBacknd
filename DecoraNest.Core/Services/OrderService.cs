using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDTO> CreteOrderAsync(int userid, string address)
        {
            if(userid <= 0)
            {
                throw new Exception("Invalid User");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(c=>c.Product)
                .FirstOrDefaultAsync(c => c.UserID == userid);

            if(cart == null || cart.CartItems.Count == 0)
            {
                throw new Exception("Cart is empty");
            }

            var user = await _context.Users.FindAsync(userid);
            var order = new Order
            {
                UserID = userid,
                UserName = user?.Name ?? "",
                OrderDate = DateTime.UtcNow,
                Status = "pending",
                Address = address,
                TotalAmount = cart.CartItems.Sum(ci => (ci.UnitPrice ) * ci.Quantity),
                Items = cart.CartItems.Where(ci => ci.Product != null)
                .Select(ci => new OrderItem
                {
                    ProductID = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    Price = ci.UnitPrice,
                    Quantity = ci.Quantity,
                    ImgUrl = ci.Product.ImgUrl
                }).ToList()
            };
            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return new OrderDTO
            {
                OrderId = order.OrderID,
                UserId = order.UserID,
                Username = order.UserName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Address = order.Address,
                Items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductId = i.ProductID,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImgUrl = i.ImgUrl
                }).ToList(),
            };

            
        }

        public async Task<List<OrderDTO>> GetAllOrderAsync(int userid)
        {
            if(userid <= 0)
            {
                throw new Exception("Invalid user");
            }

            var orders = await _context.Orders
           .Include(o => o.Items)
          .Where(o => o.UserID == userid)
          .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                throw new Exception("There is no order");
            }

            return orders.Select(order => new OrderDTO
            {
                OrderId = order.OrderID,
                UserId = order.UserID,
                Username = order.UserName,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Address = order.Address,
                Items = order.Items.Select(i => new OrderItemDTO
                {
                    ProductId = i.ProductID,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImgUrl = i.ImgUrl
                }).ToList()
            }).ToList();

        }
        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            var orders = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderId && o.UserID == userId);

            if(orders == null)
            {
                return false;
            }

            orders.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
