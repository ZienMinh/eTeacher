using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(Order order);
        Task<List<Order>> GetRefundRequestsAsync();
        Task<ServiceResponse> ApproveRefundAsync(string orderId);
        Task<ServiceResponse> RejectRefundAsync(string orderId);
        Task<double> GetPlatformEarningsAsync();
        Task<ServiceResponse> MarkForRefundAsync(string classId, string userId);
    }
}
