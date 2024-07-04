using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;
using BusinessObject.Models;
using Services.Interfaces;
using SWP391_eTeacherSystem.Helpers;
using DataAccess;
using System.Security.Claims;

namespace SWP391_eTeacherSystem.Pages
{
    public class PaymentCallbackModel : PageModel
    {
        private readonly IVNPayServices _vnPayService;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClassService _classService;
        private readonly IClassHourService _classHourService;

        public PaymentCallbackModel(IVNPayServices vnPayService, IOrderService orderService, IHttpContextAccessor httpContextAccessor, IClassService classService, IClassHourService classHourService)
        {
            _vnPayService = vnPayService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _classService = classService;
            _classHourService = classHourService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            var classId = TempData["ClassId"] as string;
            var orderTypeString = TempData["OrderType"] as string;
            var paymentStatusString = TempData["PaymentStatus"] as string;

            if (string.IsNullOrEmpty(classId) || string.IsNullOrEmpty(orderTypeString) || string.IsNullOrEmpty(paymentStatusString))
            {
                TempData["Message"] = "Không có lớp học nào trong phiên hoặc thông tin không đầy đủ.";
                return RedirectToPage("/PaymentFail");
            }

            byte orderType = byte.Parse(orderTypeString);
            byte paymentStatus = response != null && response.VnPayResponseCode == "00" ? (byte)1 : (byte)2;

            var orderId = DateTime.UtcNow.Ticks.ToString();
            var totalAmount = response != null ? response.Amount : 0.0;
            var userId = _classService.GetCurrentUserId();

            var order = new Order
            {
                Order_id = orderId,
                Order_time = DateTime.Now,
                User_id = userId,
                Class_id = classId,
                Order_type = orderType,
                Payment_status = paymentStatus,
                Transaction_id = response?.TransactionId,
                Amount = totalAmount
            };

            await _orderService.CreateOrderAsync(order);

            if (paymentStatus == 1)
            {
                TempData["Message"] = "Thanh toán VNPay thành công";
                return RedirectToPage("/PaymentSuccess");
            }
            else
            {
                TempData["Message"] = $"Lỗi thanh toán VN Pay: {response?.VnPayResponseCode}";
                return RedirectToPage("/PaymentError");
            }
        }
    }
}
