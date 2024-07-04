using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using SWP391_eTeacherSystem.Helpers;

namespace SWP391_eTeacherSystem.Pages
{
    public class ClassModel : PageModel
    {
        private readonly IClassHourService _classhourService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClassService _classService;
        private readonly IAuthService _authService;
        private readonly AddDbContext _context;
        private readonly ILogger<RequirementModel> _logger;
        private readonly IVNPayServices _vnPayService;
        private readonly UserManager<User> _userManager;
        public ClassModel(IClassService classService, IAuthService authService, AddDbContext context, ILogger<RequirementModel> logger, IVNPayServices vnPayService, IHttpContextAccessor httpContextAccessor)
        {
            _classhourService = classhourService;
            _authService = authService;
            _context = context;
            _logger = logger;
            _vnPayService = vnPayService;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public ClassHourDto ClassHourDto { get; set; }
        public ClassDto ClassDto { get; set; }

        public UserDto UserDto { get; set; }
        public List<Subject> Subjects { get; set; }

        public async Task InitializeClassDtoAsync()
        {
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                var classId = _classhourService.GenerateClassId();
                ClassHourDto = new ClassHourDto {User_id = userId, Class_id = classId };
            }
        }

        public async Task OnGetAsync()
        {
            Subjects = await _context.Subjects.ToListAsync();
            var userId = _authService.GetCurrentUserId();
            if (userId != null)
            {
                ClassHourDto = new ClassHourDto { User_id = userId };
            }
            await InitializeClassDtoAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogWarning($"Property: {state.Key} - Errors: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                await OnGetAsync();
                return Page();
            }

            var userId = _authService.GetCurrentUserId();
            if (userId == null)
            {
                _logger.LogWarning("User is not authenticated");
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return Page();
            }

            ClassHourDto.User_id = userId;

            try
            {
                _logger.LogInformation("Attempting to create class hour");
                var result = await _classhourService.CreateClassAsync(ClassHourDto, userId);
                if (result.IsSucceed)
                {
                    _logger.LogInformation("Class hour created successfully");
                    return RedirectToPage("/Index", new { id = result.CreatedClass.Class_id });
                }
                else
                {
                    _logger.LogWarning("Failed to create class hour: " + result.Message);
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while saving data: " + ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while saving data: " + ex.Message);
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCheckoutPayment(UserDto userDto, string payment)
        {
            if (ModelState.IsValid)
            {
                if (payment == "Thanh Toán VNPay")
                {
                    var sessionHelper = new SessionHelper(_httpContextAccessor);
                    var classList = sessionHelper.GetClassListFromSession();
                    var totalAmount = classList.Sum(c => c.GetTotalPrice());

                    var userId = _userManager.GetUserId(User);
                    var user = await _userManager.FindByIdAsync(userId);

                    var vnPayModel = new VnPaymentRequestModel
                    {
                        Amount = totalAmount,
                        CreatedDate = DateTime.Now,
                        Description = "Thanh toán lớp học thuê theo giờ",
                        FullName = user?.NormalizedUserName,
                        OrderId = new Random().Next(1000, 100000)
                    };

                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
            }

            return Page();  
        }

        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if(response == null || response.VnPayResponseCode != "00")
            {
				TempData["Message"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
				return RedirectToAction("PaymentFail");
			}

			TempData["Message"] = $"Thanh toán VNPay thành công";
			return RedirectToAction("PaymentSuccess");
		}
    }
}
