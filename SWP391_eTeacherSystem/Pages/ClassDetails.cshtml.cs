using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using SWP391_eTeacherSystem.Helpers;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassDetailsModel : PageModel
    {
        private readonly AddDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IClassService _classService;
        private readonly IClassHourService _classHourService;
        private readonly ILogger<ClassDetailsModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IVNPayServices _vnPayService;


		public ClassDetailsModel(AddDbContext context, IAuthService authService,
            IUserService userService, IClassService classService, ILogger<ClassDetailsModel> logger, IClassHourService classHourService, IHttpContextAccessor httpContextAccessor, IVNPayServices vnPayService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _classService = classService;
            _classHourService = classHourService;
            _httpContextAccessor = httpContextAccessor;
            _vnPayService = vnPayService;
        }

        public UserDto UserDto { get; set; }
        public ClassHour ClassHour { get; set; }
        [BindProperty]
        public ClassDto ClassDto { get; set; }
        public ClassHourDto ClassHourDto { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ClassId { get; set; }
		private const double PricePerHour = 100000;

		public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classService.GenerateClassId();
                ClassDto = new ClassDto
                {
                    Tutor_id = ClassHour.User_id,
                    Student_id = userId,
                    Class_id = classId
                };
            }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Class ID is not provided.");
            }

            ClassId = id;

            ClassHour = await _context.ClassHours.FirstOrDefaultAsync(c => c.Class_id == id);

            if (ClassHour == null)
            {
                return NotFound();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassDto = new ClassDto
                {
                    Tutor_id = ClassHour.User_id,
                    Student_id = userId
                };
            }

            await InitializeClassDtoAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("ClassId");


            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassDto.Student_id = userId;

            try
            {

                var result = await _classService.CreateClassAsync(ClassDto, userId);
                if (result.IsSucceed)
                {

                    var classHourDto = new ClassHourDto
                    {
                        Class_id = ClassId,
                        Status = 2
                    };

                    var updateResult = await _classHourService.UpdateClassHourAsync(classHourDto);

                    if (updateResult.IsSucceed)
                    {
                        return RedirectToPage("/StudentPage", new { id = result.CreatedClass.Class_id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateResult.Message);
                    }
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

		public async Task<IActionResult> OnPostCheckoutPaymentAsync(string payment)
		{
			if (ModelState.IsValid)
			{
				if (payment == "Thanh To�n VNPay")
				{
					var userId = _authService.GetCurrentUserId();
					if (userId == null)
					{
						ModelState.AddModelError(string.Empty, "User is not authenticated.");
						return Page();
					}

                    //Automatic generation class
					var classId = _classService.GenerateClassId();
					ClassDto.Class_id = classId;
					ClassDto.Student_id = userId;

					ClassHour = await _context.ClassHours.FirstOrDefaultAsync(c => c.Class_id == ClassId);
					if (ClassHour == null)
					{
						ModelState.AddModelError(string.Empty, "ClassHour not found.");
						return Page();
					}

                    //Calculate time for payment
					var startTime = ClassHour.Start_time.Value;
					var endTime = ClassHour.End_time.Value;
					var duration = (endTime - startTime).TotalHours;
					var totalAmount = ClassHour.Number_of_session * duration * ClassHour.Price;

					ClassDto.Price = ClassHour.Price;
					ClassDto.Number_of_session = ClassHour.Number_of_session;

					var response = await _classService.CreateClassAsync(ClassDto, userId);

					if (!response.IsSucceed)
					{
						TempData["Error"] = response.Message;
						TempData["PaymentStatus"] = "2";
						return Page();
					}

                    // 1: for Class, 2: for ClassHour
					var orderType = ClassDto.Type_class == 1 ? 1 : 2;

					var vnPayModel = new VnPaymentRequestModel
					{
						Amount = totalAmount,
						CreatedDate = DateTime.Now,
						Description = "Thanh toán lớp học thuê theo giờ",
						FullName = userId,
						OrderId = classId
					};

					TempData["ClassId"] = classId;
					TempData["OrderType"] = orderType.ToString();
					TempData["PaymentStatus"] = "1";

					var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
					return Redirect(paymentUrl);
				}
			}

			return Page();
		}
	}
}
