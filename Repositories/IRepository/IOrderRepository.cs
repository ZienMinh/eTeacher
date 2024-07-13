using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task UpdateOrderAsync(Order order);
        Task<List<Order>> GetRefundRequestsAsync();
        Task<Order> GetOrderByClassAndUserAsync(string classId, string userId);
        Task<double> GetPlatformEarningsAsync();
    }
}
