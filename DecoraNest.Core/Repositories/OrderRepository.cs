using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.DecoraNest.Core.Interfaces;
using DecoranestBacknd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DecoranestBacknd.DecoraNest.Core.Repositories
{
    public class OrderRepository : IOrderRepository 
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserId(int userid)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.UserID == userid);
        }

        public async Task<User?> GetUserById(int userid)
        {
            return await _context.Users.FindAsync(userid);
            
        }
        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
        }
        public async Task<List<Order>> GetOrderByUserIdAsync(int userid)
        {
            return await _context.Orders
                .Include(o=>o.Items)
                .Where(o=>o.UserID == userid)
                .ToListAsync();
        }
        public async Task<Order?>GetOrderByIdAsync(int orderid,int userid)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == orderid);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task RemoveCartItemsAsync(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
        }

    }
}
