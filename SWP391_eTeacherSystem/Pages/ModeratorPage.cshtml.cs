using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories;
using Services;
using Services.Interfaces;

namespace SWP391_eTeacherSystem.Pages
{
    public class ModeratorPageModel : PageModel
    {
		private readonly IAuthService _authService;
		private readonly IReportService _reportService;
		private readonly IOrderService _orderService;
		private readonly IAttendanceService _attendanceService;

		public ModeratorPageModel(IAuthService authService, IReportService reportService, IOrderService orderService, IAttendanceService attendanceService)
		{
			_authService = authService;
			_reportService = reportService;
			_orderService = orderService;
			_attendanceService = attendanceService;
		}

		public List<ReportDto> Reports { get; set; }
        public List<Order> RefundRequests { get; set; }
        public double PlatformEarnings { get; set; }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var reportResponse = await _reportService.GetAllReportAsync(new ReportDto());
            if (reportResponse.IsSucceed)
            {
                Reports = reportResponse.Reports;
            }
            else
            {
                Reports = new List<ReportDto>();
            }

            RefundRequests = await _orderService.GetRefundRequestsAsync();
            PlatformEarnings = await _orderService.GetPlatformEarningsAsync();

            return Page();
        }

		public int GetPresenceCount(string classId)
		{
			var attendances = _attendanceService.GetAttendancesByClassIdAsync(classId).Result;
			return attendances.Count(a => a.Status == 2);
		}

		public int GetAbsenceCount(string classId)
		{
			var attendances = _attendanceService.GetAttendancesByClassIdAsync(classId).Result;
			return attendances.Count(a => a.Status == 3);
		}

		public async Task<IActionResult> OnPostApproveRefundAsync(string orderId)
        {
            var response = await _orderService.ApproveRefundAsync(orderId);
            if (!response.IsSucceed)
            {
                ModelState.AddModelError(string.Empty, response.Message);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectRefundAsync(string orderId)
        {
            var response = await _orderService.RejectRefundAsync(orderId);
            if (!response.IsSucceed)
            {
                ModelState.AddModelError(string.Empty, response.Message);
            }
            return RedirectToPage();
        }

    }
}
