using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AddDbContext _context;

        public OrderRepository(AddDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _context.Orders.Include(o => o.Class).FirstOrDefaultAsync(o => o.Order_id == orderId);
        }

		public async Task<List<Order>> GetRefundRequestsAsync()
		{
			return await _context.Orders
								 .Include(o => o.Class)
								 .Where(o => o.Order_type == 3 && o.Payment_status == 1)
								 .ToListAsync();
		}

		public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByClassAndUserAsync(string classId, string userId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Class_id == classId && o.User_id == userId);
        }

        public async Task<double> GetPlatformEarningsAsync()
        {
            return await _context.Orders.SumAsync(o => o.PlatformEarnings);
        }
    }
}
