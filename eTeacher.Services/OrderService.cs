using BusinessObject.Models;
using DataAccess;
using Repositories;
using Repositories.IRepository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAttendanceService _attendanceService;

        public OrderService(IOrderRepository orderRepository, IAttendanceService attendanceService)
        {
            _orderRepository = orderRepository;
            _attendanceService = attendanceService;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _orderRepository.CreateOrderAsync(order);
        }

		public async Task<List<Order>> GetRefundRequestsAsync()
		{
			var orders = await _orderRepository.GetRefundRequestsAsync();

			foreach (var order in orders)
			{
				var attendances = await _attendanceService.GetAttendancesByClassIdAsync(order.Class_id);
				order.CompletedSessions = attendances.Count(a => a.Status == 2);
			}

			return orders;
		}

		public async Task<ServiceResponse> ApproveRefundAsync(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new ServiceResponse { IsSucceed = false, Message = "Order not found" };
            }

            // Get attendance list record
            var attendances = await _attendanceService.GetAttendancesByClassIdAsync(order.Class_id);
            var presenceCount = attendances.Count(a => a.Status == 2);
            var absenceCount = attendances.Count(a => a.Status == 3);

            if (absenceCount > 3)
            {
                return new ServiceResponse { IsSucceed = false, Message = "Too many absences. Refund denied." };
            }

			const double pricePerSession = 100000;
			var totalSessions = order.Class.Number_of_session;

			// Calculate the remaining sessions that have not yet been completed 
			var remainingSessions = totalSessions - presenceCount;
			var refundAmount = remainingSessions * pricePerSession;
			var tutorPayment = presenceCount * pricePerSession * 0.85; // Tutor gets 85%
			var platformFee = presenceCount * pricePerSession * 0.15; // Platform gets 15%

			// Adjust platform earnings
			var totalPaid = order.Amount;
			var totalRefund = refundAmount;
			var platformEarnings = totalPaid - totalRefund;

			// Update order details
			order.RefundAmount = refundAmount;
			order.Payment_status = 2; // Payment success
			order.CompletedSessions = presenceCount;
			order.PlatformEarnings = platformFee;
			order.Amount = refundAmount; // Remaining amount after deducting the used sessions

			await _orderRepository.UpdateOrderAsync(order);

			return new ServiceResponse { IsSucceed = true, Message = "Refund approved and processed" };
        }

        public async Task<ServiceResponse> RejectRefundAsync(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new ServiceResponse { IsSucceed = false, Message = "Order not found" };
            }

            order.Order_type = 4; // Refund rejected
            order.Payment_status = 3; // Refund rejected
            await _orderRepository.UpdateOrderAsync(order);

            return new ServiceResponse { IsSucceed = true, Message = "Refund rejected successfully" };
        }

        public async Task<ServiceResponse> MarkForRefundAsync(string classId, string userId)
        {
            var order = await _orderRepository.GetOrderByClassAndUserAsync(classId, userId);
            if (order == null)
            {
                return new ServiceResponse { IsSucceed = false, Message = "Order not found" };
            }

            order.Order_type = 3; // Marking order type as refund
            order.Payment_status = 1; // Pending approval
            await _orderRepository.UpdateOrderAsync(order);

            return new ServiceResponse { IsSucceed = true, Message = "Marked for refund review" };
        }

        public async Task<double> GetPlatformEarningsAsync()
        {
            return await _orderRepository.GetPlatformEarningsAsync();
        }
    }
}
