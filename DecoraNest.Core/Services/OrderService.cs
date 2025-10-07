using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Ecommerce.Shared.DTO;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<OrderDTO> CreteOrderAsync(int userid, string address)
        {
            if(userid <= 0)
            {
                throw new Exception("Invalid User");
            }

            var cart = await _repo.GetCartByUserId(userid);

            if (cart == null || cart.CartItems.Count == 0)
            {
                throw new Exception("Cart is empty");
            }

            var user = await _repo.GetUserById(userid);
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
           await _repo.AddOrderAsync(order);
            await _repo.RemoveCartItemsAsync(cart);
            await _repo.SaveChangesAsync();

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

            var orders = await _repo.GetOrderByUserIdAsync(userid);

            if(orders == null || !orders.Any())
            {
                throw new Exception("There are no orders for this user.");
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
            var orders = await _repo.GetOrderByIdAsync(orderId, userId);

            if (orders == null)
            {
                return false;
            }

            orders.Status = "Cancelled";
           await _repo.SaveChangesAsync();
            return true;
        }


    }
}
