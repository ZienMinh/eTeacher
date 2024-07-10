using BusinessObject.Models;
using DataAccess;
using eTeacher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using Services.Interfaces;
using Stripe.Climate;

namespace SWP391_eTeacherSystem.Pages
{
	public class CancelClassModel : PageModel
	{
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly IScheduleService _scheduleService;
        private readonly IOrderService _orderService;

        public CancelClassModel(AddDbContext context, IAuthService authService,
                IUserService userService, IClassService classService, IScheduleService scheduleService, IOrderService orderService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _scheduleService = scheduleService;
            _orderService = orderService;
        }

        public UserDto UserDto { get; set; }

		public Class Class { get; set; }

		[BindProperty]
		public ClassDto ClassDto { get; set; }

		[BindProperty(SupportsGet = true)]
		public string ClassId { get; set; }

		public async Task InitializeClassDtoAsync()
		{
			var userId = _authService.GetCurrentUserId();
		}

		public async Task<IActionResult> OnGetAsync(string id)
		{

			if (string.IsNullOrEmpty(id))
			{
				return NotFound("Class ID is not provided.");
			}

			ClassId = id;

			Class = await _context.Classes.FirstOrDefaultAsync(c => c.Class_id == id);

			if (Class == null)
			{
				return NotFound();
			}

			var userId = _authService.GetCurrentUserId();
			await InitializeClassDtoAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var userId = _authService.GetCurrentUserId();
			if (userId == null)
			{
				ModelState.AddModelError(string.Empty, "User is not authenticated.");
				return Page();
			}

			ClassDto.Student_id = userId;

			try
			{
				var classDto = new ClassDto
				{
					Class_id = ClassId,
					Status = 2
				};

                var result = await _classService.UpdateClassAsync(classDto);
                if (result.IsSucceed)
                {
                    var refundResult = await _orderService.MarkForRefundAsync(ClassId, userId); // Marking for refund instead of processing
                    if (refundResult.IsSucceed)
                    {
                        await _scheduleService.DeleteSchedulesByClassIdAsync(ClassId);
                        TempData["Message"] = "Class has been cancelled and refund review process initiated.";
                    }
                    else
                    {
                        TempData["Message"] = "Class has been cancelled but refund review process failed.";
                    }

                    return RedirectToPage("/StudentPage");
                }
                else
				{
					ModelState.AddModelError(string.Empty, result.Message);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
			}
			return Page();
		}

		//private async Task<bool> ProcessRefund(string classId, string userId)
		//{
		//	try
		//	{
		//		var order = await _context.Orders.FirstOrDefaultAsync(o => o.Class_id == classId && o.User_id == userId);
		//		if (order != null)
		//		{
		//			int completedSessions = order.CompletedSessions;
		//			int totalSessions = order.Class.Number_of_session;
		//			int remainingSessions = totalSessions - completedSessions;
		//			double refundAmount = remainingSessions * order.Price_per_session;

		//			order.Order_type = 3; // 3: Refund
		//			order.Payment_status = 2; // 2: Payment success
		//			_context.Orders.Update(order);
		//			await _context.SaveChangesAsync();
		//			return true;
		//		}
		//		return false;
		//	}
		//	catch (Exception)
		//	{
		//		return false;
		//	}
		//}

	}
}
